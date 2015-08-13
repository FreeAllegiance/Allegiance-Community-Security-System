using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Allegiance.CommunitySecuritySystem.Client.Integration;
using Allegiance.CommunitySecuritySystem.Client.Service;
using Allegiance.CommunitySecuritySystem.Client.Utility;
using Allegiance.CommunitySecuritySystem.Client.ClientService;
using System.Threading;
using Microsoft.Win32;
using System.Collections.Generic;

namespace Allegiance.CommunitySecuritySystem.Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

			try
			{
				Initialize();
			}
			catch (System.Net.WebException ex)
			{
				File.WriteAllText("ExceptionLog.txt", ex.ToString());

				if (MessageBox.Show("There was an error connecting to the Free Allegiance System.\nYou can still play off-line, but you will not be able to connect to the Free Allegiance Lobby. \nWould you like to launch Allegiance and play off-line?", "Server Error, Play off-line?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					string allegiancePath = Path.Combine(AllegianceRegistry.LobbyPath, LobbyType.Production.ToString());
					string exePath = Path.Combine(allegiancePath, "Allegiance.exe");
					Process.Start(exePath);
				}
			}

			//while (_shutdownFlag == false)
			//{
			//    Thread.Sleep(100);
			//}
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            File.WriteAllText("ExceptionLog.txt", e.Exception.ToString());
        }

#if !DEBUG
        [DebuggerStepThrough]
#endif
		private static void Initialize()
        {
            try
            {
                //Check if a debugger is attached
                if (DebugDetector.Detect())
                {
                    Log.Write("Debugging CSS is prohibited.");
                    return;
                }

                //See if the client has recently been updated
                if (AutoUpdate.CheckTemporaryProcess())
                    return;

                //Ticket #46 - Prevent multiple instances of the launcher
				var runningProcesses = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
				int matchCounter = 0;
				foreach (var runningProcess in runningProcesses)
				{
					if (runningProcess.MainModule.FileName.Equals(Process.GetCurrentProcess().MainModule.FileName) == true)
						matchCounter++;
				}

				if (matchCounter > 1)
				{
					MessageBox.Show("Only one instance of " + Process.GetCurrentProcess().MainModule.FileName + " can be run at a time. Please close other versions of the ACSS Launcher and try again.", "ACSS Launcher Already Running", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                using (ServiceHandler.Service)
                {
					UpdateCheckForm updateCheckForm = new UpdateCheckForm();

					if (updateCheckForm.HasPendingUpdates == true)
					{
						if (updateCheckForm.ShowDialog() != DialogResult.OK)
							return;
					}

					// If you need to upgrade things on the end users system during an auto-update push, put them in here.
					string returnMessage;
					if (Install.Upgrade.PerformUpgradeTasks(out returnMessage) == false)
					{
						MessageBox.Show(returnMessage, "Upgrade canceled.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						return;
					}

					



					bool showLoginDialog = false;

                    //Check if the credentials are already stored
					if (ServiceHandler.CheckLogin() == true)
					{
						var launcherStartupProgress = new LauncherStartupProgress();
						launcherStartupProgress.TopMost = true;

						var launcherStartupProgressResult = launcherStartupProgress.ShowDialog();

						if (launcherStartupProgressResult == DialogResult.OK)
							StartMainForm();

						//else if (launcherStartupProgressResult == DialogResult.Abort)
						//	return;

						else 
							showLoginDialog = true;
					}
					
					if (ServiceHandler.CheckLogin() == false || showLoginDialog == true)
					{
						//Create Login prompt
						using (var loginForm = new LoginForm())
						{
							if (loginForm.ShowDialog() != DialogResult.OK)
								return;

							var launcherStartupProgress = new LauncherStartupProgress();
							launcherStartupProgress.TopMost = true;

							if (launcherStartupProgress.ShowDialog() != DialogResult.OK)
								return;

							StartMainForm();
						}
					}
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText("ExceptionLog.txt", ex.ToString());
                throw;
            }
        }

		private static void StartMainForm()
		{


			try
			{
				do
				{
					Application.Run(new MainForm());

					if (MainForm.Restart == true)
					{
						if (!ServiceHandler.CheckLogin())
						{
							using (var loginForm = new LoginForm())
							{
								if (loginForm.ShowDialog() != DialogResult.OK)
									return;

								//launcherSignInStatus = loginForm.LauncherSignInStatus;
							}
						}
					}
				} while (MainForm.Restart == true);
			}
			catch (Exception ex)
			{
				File.WriteAllText("ExceptionLog.txt", ex.ToString());
				throw;
			}
			finally
			{
				if (MainForm.LoggedIn)
					SessionNegotiator.Logout(true);

				SystemWatcher.Close();
				AllegianceLoader.ExitAllegiance();
			}
		}
    }
}