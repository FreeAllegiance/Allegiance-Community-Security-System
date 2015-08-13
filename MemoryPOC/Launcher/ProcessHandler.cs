using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Launcher
{
	class ProcessHandler
	{
		#region Interop Methods

		private const int STARTF_USESTDHANDLES = 0x00000100;
		
		[DllImport("wininet.dll", EntryPoint = "GetLastError")]
		public static extern long GetLastError();


		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesWritten);


		[DllImport("kernel32.dll")]
		static extern bool CreateProcess(
			string lpApplicationName,
			string lpCommandLine,
			IntPtr lpProcessAttributes,
			IntPtr lpThreadAttributes,
			bool bInheritHandles,
			ProcessCreationFlags dwCreationFlags,
			IntPtr lpEnvironment,
			string lpCurrentDirectory,
			ref STARTUPINFO lpStartupInfo,
			out PROCESS_INFORMATION lpProcessInformation);

		[DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
		static extern IntPtr OpenProcess(
			[MarshalAs(UnmanagedType.U4)] ProcessAccessPriviledges DesiredAccess,
			bool InheritHandle,
			uint ProcessId);

		[DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool CloseHandle(IntPtr hObject);

		[DllImport("kernel32.dll", EntryPoint = "WaitForDebugEvent")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool WaitForDebugEvent(
			[In] ref DEBUG_EVENT lpDebugEvent,
			uint dwMilliseconds);

		[DllImport("kernel32.dll")]
		static extern bool ContinueDebugEvent(
			uint dwProcessId,
			uint dwThreadId,
			DebugStates dwContinueStatus);

		#endregion

		#region Fields

		private Thread _activeThread;
		private IntPtr _handle;
		private IntPtr _fileHandle;
		private volatile uint _processId;
		private volatile bool _continue = false;
		private volatile bool _signalEnd = false;
		private volatile bool _processStarted = false;

		#endregion

		#region Event Declarations

		public event EventHandler OnExiting;


		public delegate void OnStandardOutHandler(string data);
		public event OnStandardOutHandler OnStandardOut;

		public delegate void OnStandardErrorHandler(string data);
		public event OnStandardErrorHandler OnStandardError;

		#endregion

		#region Constructors

		private ProcessHandler(string filename, string arguments)
		{
			//Start debugging thread
			_activeThread = TaskHandler.RunTask(Debugger, filename, arguments);
		}

		#endregion

		#region Methods

		public static ProcessHandler Start(string filename, string arguments)
		{
			return new ProcessHandler(filename, arguments);
		}

		public void ForceClose()
		{
			Process process;
			if (TryGetAttachedProcess(out process) == true)
			{
				process.WaitForInputIdle();
				process.CloseMainWindow();
				process.WaitForExit();
			}

			_continue = false;

			if (_activeThread.IsAlive)
				_activeThread.Join();
		}

		public void Close()
		{
			_signalEnd = true;

			if (_activeThread.IsAlive)
				_activeThread.Join();
		}

		private void End()
		{
			if (_handle != IntPtr.Zero)
			{
				if (!CloseHandle(_handle))
					throw new Exception("Failed to close Allegiance.");

				_handle = IntPtr.Zero;
			}

			if (_fileHandle != IntPtr.Zero)
			{
				CloseHandle(_fileHandle);

				_fileHandle = IntPtr.Zero;
			}
		}


		/// <summary>
		/// Waits for _processID to become non-0 then returns the process object. 
		/// </summary>
		/// <param name="process"></param>
		/// <returns>true if process was available, else false.</returns>
		public bool TryGetAttachedProcess(out Process process)
		{
			process = null;

			for (int i = 0; i < 300 && (int)_processId == 0; i++)
				Thread.Sleep(100);

			if ((int)_processId == 0)
				return false;

			//Process.GetProcessById() will throw an exception if the process id is not running.
			foreach (Process availableProcess in Process.GetProcesses())
			{
				if (availableProcess.Id == _processId)
				{
					process = availableProcess;
					return true;
				}
			}

			return false;
		}

		public bool IsProcessAvailable()
		{
			Process process;
			return TryGetAttachedProcess(out process);
		}

		private void Debugger(object value)
		{
			_continue = true;
			var parameters = value as object[];
			var filename = parameters[0] as string;
			var arguments = parameters[1] as string;

			if (!string.IsNullOrEmpty(arguments) && !arguments.StartsWith(" "))
				arguments = string.Concat(" ", arguments);

			var si = new STARTUPINFO();
			PROCESS_INFORMATION pi;

			//Create Process

			if (!CreateProcess(null, string.Format("\"{0}\"{1}", filename, arguments),
				IntPtr.Zero, IntPtr.Zero, true,
				ProcessCreationFlags.DEBUG_ONLY_THIS_PROCESS | ProcessCreationFlags.DEBUG_PROCESS,
				IntPtr.Zero, null, ref si, out pi))
				throw new Exception(string.Format("Failed to start {0}.", Path.GetFileName(filename)));

			//if (!CreateProcess(null, string.Format("\"{0}\"{1}", filename, arguments),
			//   IntPtr.Zero, IntPtr.Zero, false,
			//   ProcessCreationFlags.NORMAL_PRIORITY_CLASS,
			//   IntPtr.Zero, null, ref si, out pi))
			//    throw new Exception(string.Format("Failed to start {0}.", Path.GetFileName(filename)));



			//Open Process Handle
			_handle = OpenProcess(ProcessAccessPriviledges.PROCESS_VM_READ | ProcessAccessPriviledges.PROCESS_TERMINATE | ProcessAccessPriviledges.PROCESS_VM_WRITE | ProcessAccessPriviledges.PROCESS_VM_OPERATION,
				false, pi.dwProcessId);

			//Create debug event
			var debugEvent = new DEBUG_EVENT()
			{
				dwProcessId = pi.dwProcessId,
				dwThreadId = pi.dwThreadId
			};

			

			try
			{
				do
				{
					//DebugDetector.AssertCheckRunning();

					if (WaitForDebugEvent(ref debugEvent, 100))
					{
						switch (debugEvent.dwDebugEventCode)
						{
							//Continue
							case DebugEventTypes.EXCEPTION_DEBUG_EVENT:
							case DebugEventTypes.CREATE_THREAD_DEBUG_EVENT:
							case DebugEventTypes.LOAD_DLL_DEBUG_EVENT:
							case DebugEventTypes.UNLOAD_DLL_DEBUG_EVENT:
							case DebugEventTypes.OUTPUT_DEBUG_STRING_EVENT:
							case DebugEventTypes.EXIT_THREAD_DEBUG_EVENT:
								break;

							case DebugEventTypes.CREATE_PROCESS_DEBUG_EVENT:
								_fileHandle = debugEvent.u.CreateProcessInfo.hFile;
								_processId = debugEvent.dwProcessId;
								_processStarted = true;
								break;

							//Exit the loop
							case DebugEventTypes.EXIT_PROCESS_DEBUG_EVENT:
							case DebugEventTypes.RIP_EVENT:
							default:
								_continue = false;
								break;
						}
					}

					if (_signalEnd && _processStarted)
					{
						Process process;
						if (TryGetAttachedProcess(out process) == true)
							process.CloseMainWindow();
					}

					ContinueDebugEvent(debugEvent.dwProcessId,
						debugEvent.dwThreadId, DebugStates.DBG_CONTINUE);
				}
				while (_continue);
			}
			finally
			{
				End();

				TaskHandler.RunTask(delegate(object input)
				{
					if (OnExiting != null)
						OnExiting(this, EventArgs.Empty);
				});
			}
		}

		private void AttachOutputRedirection(PROCESS_INFORMATION processInformation)
		{
			Process process = Process.GetProcessById((int)processInformation.dwProcessId);

			process.OutputDataReceived += new DataReceivedEventHandler(OutputDataReceived);
			process.ErrorDataReceived += new DataReceivedEventHandler(ErrorDataReceived);

			process.BeginOutputReadLine();
			process.BeginErrorReadLine();
		}

		void ErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (OnStandardError != null)
				OnStandardError(e.Data);
		}

		void OutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (OnStandardOut != null)
				OnStandardOut(e.Data);
		}

		#endregion

		#region Enums

		[Flags]
		public enum DebugStates
		{
			DBG_CONTINUE = 0x10002,
			DBG_CONTROL_BREAK = 0x40010008,
			DBG_CONTROL_C = 0x40010005,
			DBG_EXCEPTION_NOT_HANDLED = -2147418111,
			DBG_TERMINATE_PROCESS = 0x40010004,
			DBG_TERMINATE_THREAD = 0x40010003
		}

		[Flags]
		enum ProcessAccessPriviledges
		{
			PROCESS_ALL_ACCESS = 0x100fff,
			PROCESS_CREATE_PROCESS = 0x80,
			PROCESS_CREATE_THREAD = 2,
			PROCESS_DUP_HANDLE = 0x40,
			PROCESS_QUERY_INFORMATION = 0x400,
			PROCESS_SET_INFORMATION = 0x200,
			PROCESS_SET_QUOTA = 0x100,
			PROCESS_SET_SESSIONID = 4,
			PROCESS_SYNCHRONISE = 0x100000,
			PROCESS_TERMINATE = 1,
			PROCESS_VM_OPERATION = 8,
			PROCESS_VM_READ = 0x10,
			PROCESS_VM_WRITE = 0x20
		}

		[Flags]
		enum ProcessCreationFlags : uint
		{
			CREATE_DEFAULT_ERROR_MODE = 0x4000000,
			CREATE_FORCEDOS = 0x2000,
			CREATE_NEW_CONSOLE = 0x10,
			CREATE_NEW_PROCESS_GROUP = 0x200,
			CREATE_NO_WINDOW = 0x8000000,
			CREATE_SEPARATE_WOW_VDM = 0x800,
			CREATE_SHARED_WOW_VDM = 0x1000,
			CREATE_SUSPENDED = 4,
			CREATE_UNICODE_ENVIRONMENT = 0x400,
			DEBUG_ONLY_THIS_PROCESS = 2,
			DEBUG_PROCESS = 1,
			DETACHED_PROCESS = 8,
			HIGH_PRIORITY_CLASS = 0x80,
			IDLE_PRIORITY_CLASS = 0x40,
			NORMAL_PRIORITY_CLASS = 0x20,
			REALTIME_PRIORITY_CLASS = 0x100
		}

		enum DebugEventTypes
		{
			CREATE_PROCESS_DEBUG_EVENT = 3,
			CREATE_THREAD_DEBUG_EVENT = 2,
			EXCEPTION_DEBUG_EVENT = 1,
			EXIT_PROCESS_DEBUG_EVENT = 5,
			EXIT_THREAD_DEBUG_EVENT = 4,
			LOAD_DLL_DEBUG_EVENT = 6,
			OUTPUT_DEBUG_STRING_EVENT = 8,
			RIP_EVENT = 9,
			UNLOAD_DLL_DEBUG_EVENT = 7
		}

		#endregion

		#region Structures

		[StructLayout(LayoutKind.Sequential)]
		protected struct PROCESS_INFORMATION
		{
			public IntPtr hProcess;
			public IntPtr hThread;
			public uint dwProcessId;
			public uint dwThreadId;
		}

		[StructLayout(LayoutKind.Sequential)]
		protected struct STARTUPINFO
		{
			public uint cb;
			public string lpReserved;
			public string lpDesktop;
			public string lpTitle;
			public uint dwX;
			public uint dwY;
			public uint dwXSize;
			public uint dwYSize;
			public uint dwXCountChars;
			public uint dwYCountChars;
			public uint dwFillAttribute;
			public uint dwFlags;
			public short wShowWindow;
			public short cbReserved2;
			public IntPtr lpReserved2;
			public IntPtr hStdInput;
			public IntPtr hStdOutput;
			public IntPtr hStdError;
		}

		#region Debug Union Info

		[StructLayout(LayoutKind.Sequential)]
		public struct CREATE_PROCESS_DEBUG_INFO
		{
			public IntPtr hFile;
			public IntPtr hProcess;
			public IntPtr hThread;
			public IntPtr lpBaseOfImage;
			public uint dwDebugInfoFileOffset;
			public uint nDebugInfoSize;
			public IntPtr lpThreadLocalBase;
			public uint lpStartAddress;
			public IntPtr lpImageName;
			public ushort fUnicode;
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct Union
		{
			[FieldOffset(0)]
			public CREATE_PROCESS_DEBUG_INFO CreateProcessInfo;
		}

		#endregion

		[StructLayout(LayoutKind.Sequential)]
		struct DEBUG_EVENT
		{
			public DebugEventTypes dwDebugEventCode;
			public uint dwProcessId;
			public uint dwThreadId;
			public Union u;
		}

		#endregion

		internal void WriteMemory(Int64 memoryLocation, string value)
		{
			// Adding the space on the end so that the string will be null terminated when it's written in to raw memory.
			byte [] valueBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(value + " ");
			
			// Add the null termination.
			valueBytes[valueBytes.Length - 1] = (byte) '\0';

			int bytesWritten;
		
			WriteProcessMemory(_handle, new IntPtr(memoryLocation), valueBytes, (uint) valueBytes.Length, out bytesWritten);

			int lastError = Marshal.GetLastWin32Error();
		}
	}
}