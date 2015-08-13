using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using Allegiance.CommunitySecuritySystem.Client.Utility;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Allegiance.CommunitySecuritySystem.Client.Integration
{
	/// <summary>
	/// Monitors a process handler for that handlers process to close. If the process closes
	/// then the processhandler's ForceClose() method is called which fires all 
	/// listing events. This was required to have the launcher detect that allegiance has exited
	/// when there was an error on launch (missing artwork folder hang) which then required 
	/// allegiance.exe to be killed from the task manager.
	/// </summary>
	class ProcessMonitor
	{
		[DllImport("Kernel32.dll", SetLastError = true, ExactSpelling = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool CheckRemoteDebuggerPresent(SafeHandle hProcess, [MarshalAs(UnmanagedType.Bool)]ref bool isDebuggerPresent);

		private Timer _timer = null;
		private ProcessHandler _processHandler;

		public ProcessMonitor(ProcessHandler processHandler)
		{
			_processHandler = processHandler;

//#if(!DEBUG)

			_timer = new Timer(new TimerCallback(delegate(object param)
				{
					ProcessMonitor processMonitor = (ProcessMonitor)param;

					if (processMonitor._processHandler.IsProcessAvailable() == false)
					{
						Log.Write("Allegiance process has ended, forcing processHandler to close.");
						processMonitor._processHandler.ForceClose();
					}

					try
					{
						//// Double check that the process id is actually available in the process list.
						Process process = Process.GetProcessById((int)processMonitor._processHandler.ProcessId);

						if (process == null)
							processMonitor._processHandler.ForceClose();


						bool isDebuggerPresent = false;
						if (CheckRemoteDebuggerPresent(new SafeFileHandle(process.Handle, true), ref isDebuggerPresent) == true && isDebuggerPresent == true)
						{
							Log.Write("Debugger detected, will exit.");
							processMonitor._processHandler.ForceClose();
							_timer.Dispose();
						}
					}
					catch (Exception ex)
					{
						Log.Write("Allegiance process was not found, this is normal. The message was: " + ex.ToString());
						processMonitor._processHandler.ForceClose();
						_timer.Dispose();
					}

				}), this, 0, 1000);
//#endif
		}

	}
}
