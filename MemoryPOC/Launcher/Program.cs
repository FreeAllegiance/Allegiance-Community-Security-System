using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace Launcher
{
	class Program
	{
		private static ProcessHandler _processHandler = null;

		static void Main(string[] args)
		{
			ManualResetEvent reset = new ManualResetEvent(false);

			TaskHandler.RunTask(delegate(object value)
			{
				var parameters = value as object[];

				using (var pipe = new Pipe(@"\\.\pipe\allegqueue"))
				{
					pipe.Create();

					if (pipe.Connect())
					{
						string memoryLocation = pipe.Read();
						PerformStringUpdateInRemoteClient(memoryLocation);
					}
				}

			});

			_processHandler = ProcessHandler.Start(@"..\..\..\Client\Debug\Client.exe", "");

			Process process = null;
			do
			{
				_processHandler.TryGetAttachedProcess(out process);
			} while (process == null);

			

			process.WaitForExit();
		}

		private static void PerformStringUpdateInRemoteClient(string memoryLocation)
		{
			_processHandler.WriteMemory(Int64.Parse(memoryLocation), "RoxxorzMySoxxorz");
		}

	}
}
