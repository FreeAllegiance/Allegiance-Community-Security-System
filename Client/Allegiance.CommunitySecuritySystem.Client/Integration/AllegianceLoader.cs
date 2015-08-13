using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Allegiance.CommunitySecuritySystem.Client.Utility;
using Allegiance.CommunitySecuritySystem.Client.Properties;
using System.IO;
using System.Configuration;

namespace Allegiance.CommunitySecuritySystem.Client.Integration
{
    class AllegianceLoader
    {
		public static event EventHandler AllegianceExit;


        #region Fields

        private const string ProcessName = "Allegiance";

        private const int PipeTimeout = 180000;

        private static ProcessHandler _allegianceProcess = null;
        private static ProcessMonitor _allegianceProcessMonitor = null;

        #endregion

        #region Properties

        public static ProcessHandler AllegianceProcess
        {
            get { return _allegianceProcess; }
        }

        #endregion

        #region Events

        static void process_OnExiting(object sender, EventArgs e)
        {
			Log.Write("AllegianceLoader::process_OnExiting - Called!");

            _allegianceProcess = null;
            _allegianceProcessMonitor = null;


            //Disable system watcher
			SystemWatcher.Close();

			Log.Write("AllegianceLoader::process_OnExiting - System Watcher Closed.");

			if (AllegianceExit != null)
			{
				Log.Write("AllegianceLoader::process_OnExiting - Calling Allegiance Exit.");
				AllegianceExit(sender, e);
			}

			AllegianceExit = null;
        }

        #endregion

        #region Methods

#if !DEBUG
        [DebuggerStepThrough]
#endif
        public static void StartAllegiance(string ticket, LobbyType lobbyType, string alias, TaskDelegate onCompleteDelegate)
        {
            DebugDetector.AssertCheckRunning();

            TaskHandler.RunTask(delegate(object p)
            {
                var param = p as object[];
                var sessionTicket = param[0] as string;
                var signal = param[1] as TaskDelegate;
                var succeeded = false;

                try
                {
					AllegianceRegistry.OutputDebugString = DataStore.Preferences.DebugLog;
					AllegianceRegistry.LogToFile = DataStore.Preferences.DebugLog;
                    AllegianceRegistry.LogChat = DataStore.Preferences.LogChat;

                    //Create commandline
                    var commandLine = new StringBuilder("-authenticated")
						.AppendFormat(" -callsign={0}", alias);

					if (DataStore.Preferences.DebugLog)
						commandLine.Append(" -debug");
						
                    if (DataStore.Preferences.LaunchWindowed)
                        commandLine.Append(" -windowed");

                    if (DataStore.Preferences.NoMovies)
                        commandLine.Append(" -nomovies");

                    //Start Allegiance
                    string lobbyPath = Path.Combine(AllegianceRegistry.LobbyPath, lobbyType.ToString());

                    string allegiancePath = Path.Combine(lobbyPath, "Allegiance.exe");

#if DEBUG
						if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["AllegianceExeOverride"]) == false)
						{
							Log.Write("Allegiance path was overridden by configuration setting.");
							allegiancePath = ConfigurationManager.AppSettings["AllegianceExeOverride"];
						}
#endif

                    Log.Write("Using: " + allegiancePath + " " + commandLine.ToString() + " to launch...");

                    ProcessHandler process = ProcessHandler.Start(allegiancePath, commandLine.ToString());

					process.OnExiting += new EventHandler(process_OnExiting);

					_allegianceProcess = process;
					_allegianceProcessMonitor = new ProcessMonitor(_allegianceProcess);

					// If launching into a lobby, then relay the security token to the allegiance process.
					if (lobbyType != LobbyType.None)
					{
						//Open Pipe
						using (var reset = new ManualResetEvent(false))
						{
							TaskHandler.RunTask(delegate(object value)
							{
								var parameters = value as object[];
								var localProcessHandler = parameters[0] as ProcessHandler;
								var localSessionTicket = parameters[1] as String;
								var localReset = parameters[2] as ManualResetEvent;

								using (var pipe = new Pipe(@"\\.\pipe\allegqueue"))
								{
									pipe.Create();

									if (pipe.Connect())
									{
										Int64 memoryLocation = Int64.Parse(pipe.Read());

										localProcessHandler.WriteMemory(memoryLocation, localSessionTicket + (char) 0x00 + Process.GetCurrentProcess().Id);

										localReset.Set();
									}
								}

							}, process, sessionTicket, reset);


							//Wait X seconds, if allegiance does not retrieve the ticket, exit Allegiance.
							if (!reset.WaitOne(PipeTimeout))
							{
								try
								{
									process.ForceClose();

									//Connect to the pipe in order to close out the connector thread.
									using (var pipe = new Pipe(@"\\.\pipe\allegqueue"))
									{
										pipe.OpenExisting();
										pipe.Read();
									}
								}
								catch { }
								finally
								{
									throw new Exception("Allegiance did not respond within the time allotted.");
								}
							}

							// The memory address was retrived from the pipe, write the ticket onto the target process.
							//process.WriteMemory((Int64) memoryLocation, sessionTicket);      
						}
					}

                    succeeded = true;
                }
                catch (Exception error)
                {
                    Log.Write(error);
                }
                finally
                {
                    signal(succeeded);
                }
            }, ticket, onCompleteDelegate);
        }

        public static void ExitAllegiance()
        {
            if (_allegianceProcessMonitor != null)
                _allegianceProcessMonitor = null;

            if (_allegianceProcess != null)
            {
                _allegianceProcess.ForceClose();
                _allegianceProcess = null;
            }

            //Close all instances of Allegiance
            var processes = Process.GetProcessesByName(ProcessName);
            if (processes.Length > 0)
            {
                foreach (var process in processes)
                    CloseProcess(process);
            }
        }

        private static void CloseProcess(Process process)
        {
            if (!process.HasExited)
            {
                if (!process.CloseMainWindow())
                    process.Kill();
                if (!process.HasExited)
                    process.WaitForExit();
            }

            process.Close();
        }

        #endregion
    }
}