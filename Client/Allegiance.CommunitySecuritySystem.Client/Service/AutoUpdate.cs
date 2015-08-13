using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Windows.Forms;
using Allegiance.CommunitySecuritySystem.Client.Integration;
using Allegiance.CommunitySecuritySystem.Client.ClientService;
using Allegiance.CommunitySecuritySystem.Client.Utility;
using System.Threading;

namespace Allegiance.CommunitySecuritySystem.Client.Service
{
    public static class AutoUpdate
    {
		private const int MaximumTimeToWaitForFileReleaseMS = 120000;


		public delegate bool AutoupdateProgressCallback(string lobbyName, string message, int completionPercentage);

		private delegate void UiInteraction();

        #region Fields

        const int BufferSize = 30720; //bytes

        #endregion

        #region Methods

		private static string GetLobbyPath(LobbyResult lobby, string filename)
		{
			string root = null;
			string path;

			try
			{
				root = AllegianceRegistry.LobbyPath;
				path = Path.Combine(Path.Combine(root, lobby.Name), filename);
			}
			catch (Exception ex)
			{
				throw new Exception("Couldn't combine path, root: " + root + ", lobby.Name: " + lobby.Name + ", file.Filename: " + filename + ", registryRoot: " + AllegianceRegistry.Root + ", Version: " + AllegianceRegistry.Version, ex);
			}

			return path;
		}


		public static PendingUpdates GetPendingUpdateQueues(ClientService.ClientService service)
		{
			PendingUpdates returnValue = new PendingUpdates();

			var lobbies = ServiceHandler.Service.CheckAvailableLobbies();
			foreach (LobbyResult lobby in lobbies)
			{
				//Get autoupdate files associated with lobby
				var results = service.CheckForUpdates(lobby.LobbyId, true);

				List<FindAutoUpdateFilesResult> updateQueue = ProcessPendingUpdates(lobby, results);

				returnValue.AutoUpdateBaseAddress.Add(lobby.LobbyId, results.AutoUpdateBaseAddress);
				returnValue.AllFilesInUpdatePackage.Add(lobby.LobbyId, results.Files);
				returnValue.PendingUpdateList.Add(lobby.LobbyId, updateQueue);
			}

			return returnValue;
		}

		public static List<FindAutoUpdateFilesResult> ProcessPendingUpdates(LobbyResult lobby, AutoUpdateResult results)
		{
			//Initialize Checksum class to use SHA1
			var encryption = new Encryption<SHA1>();

			//var root        = AllegianceRegistry.EXEPath; //Allegiance root directory
			var autoUpdate = DataStore.Open("autoupdate.ds", "Ga46^#a042");
			string dataKey = "Files_" + lobby.LobbyId;
			var lastResults = autoUpdate[dataKey] as Dictionary<string, FindAutoUpdateFilesResult>;
			List<FindAutoUpdateFilesResult> updateQueue = new List<FindAutoUpdateFilesResult>();

			//Check files which need update
			foreach (var file in results.Files)
			{
				DebugDetector.AssertCheckRunning();

				// You can put this in if you are testing the launcher with the production CSS server. 
				// Turn off file protection on the launcher on the CSS server's Auto Update version of the launcher, 
				// then drop the debug version of the launcher into your local game directory. Then you can 
				// launch the launcher from the debugger and work with it.
//#if DEBUG
//                if (file.Filename.EndsWith("Launcher.exe", StringComparison.InvariantCultureIgnoreCase) == true
//                    || file.Filename.EndsWith("Launcher.pdb", StringComparison.InvariantCultureIgnoreCase) == true)
//                    continue;
//#endif 

				string path;

				if (string.Equals(file.Filename, GlobalSettings.ClientExecutableName)
					|| string.Equals(file.Filename, GlobalSettings.ClientExecutablePDB))
					path = Path.Combine(AllegianceRegistry.LobbyPath, file.Filename);
				else
					path = GetLobbyPath(lobby, file.Filename);

				//Check that all files exist
				if (!File.Exists(path))
				{
					Log.Write("File did not exist: " + path + ", will update.");
					updateQueue.Add(file);
					continue;
				}

				//Check that all file versions match
				if (lastResults != null && lastResults.ContainsKey(file.Filename)
					&& (file.CurrentVersion != lastResults[file.Filename].CurrentVersion))
				{
					Log.Write("File version mismatch, will update: " + file.Filename + ", server version: " + file.CurrentVersion + ", local version: " + lastResults[file.Filename].CurrentVersion);

					updateQueue.Add(file);
					continue;
				}

				// Test for checksum match, and if they don't replace the file if:
				// * This is the first time AutoUpdate has run for this installation.
				// * This is the first time this file has been seen by AutoUpdate
				// * The file has the protected flag turned on.
				var checksum = encryption.Calculate(path);
				if (!string.Equals(file.ValidChecksum, checksum))
				{
					// If there haven't been any updates at all, and the checksums don't match, then overwrite the unknown file. (old installation / left over files from a previous uninstall)
					if (lastResults == null)
					{
						Log.Write("No prior autoupdate records, will updated: " + path);
						updateQueue.Add(file);
						continue;
					}

					// If there wasn't a prior update applied for the file, and the checksums don't match, then overwrite the unknown file. (An older file is on disk, and was re-added to the auto update system)
					if (!lastResults.ContainsKey(file.Filename))
					{
						Log.Write("No record of file in autoupdate history, will update: " + path);
						updateQueue.Add(file);
						continue;
					}

					// If the file was changed on the server, but not modified by the user, then update.
					if (lastResults[file.Filename].DateModified == file.DateModified)
					{
						Log.Write("file was not user modified, and a change was detected from AU, will update: " + path);
						updateQueue.Add(file);
						continue;
					}

					// If the file is protected and the hashes don't match, then update.
					if (file.IsProtected)
					{
						Log.Write("File checksum mismatch, will update: " + path + ", server checksum: " + file.ValidChecksum + ", local checksum: " + checksum);
						updateQueue.Add(file);
						continue;
					}
				}
			}

			return updateQueue;
		}


#if !DEBUG
        [DebuggerStepThrough]
#endif
		public static bool ProcessPendingUpdates(PendingUpdates pendingUpdates, LobbyResult lobby, AutoupdateProgressCallback progressCallback, Control parentControl)
        {
			string dataKey = "Files_" + lobby.LobbyId; 
			var autoUpdate = DataStore.Open("autoupdate.ds", "Ga46^#a042");
			bool updateExe = false;
			int received = 0;
			bool continueDownloadingFiles = true;

            //Get files from auto-update
			foreach (var file in pendingUpdates.PendingUpdateList[lobby.LobbyId])
            {
				if (continueDownloadingFiles == false)
					return false;

                var filename    = file.Filename;
				var downloadPath = String.Empty;

                //Check if filename matches the ClientExecutableName
				if (string.Equals(file.Filename, GlobalSettings.ClientExecutableName))
				{
					filename = GlobalSettings.TempExecutableName;
					updateExe = true; 
					downloadPath = Path.Combine(AllegianceRegistry.LobbyPath, filename);
				}
				// Launcher.PDB also goes into the lobby root.
				else if (string.Equals(file.Filename, GlobalSettings.ClientExecutablePDB))
				{
					filename = GlobalSettings.TempExecutablePDB;
					downloadPath = Path.Combine(AllegianceRegistry.LobbyPath, filename);
				}
				else
				{
					downloadPath = GetLobbyPath(lobby, filename);
				}

                if (!Directory.Exists(Path.GetDirectoryName(downloadPath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(downloadPath));

                var message = string.Format("Updating {0}...", file.Filename);

                try
                {
					continueDownloadingFiles = DownloadFile(++received, pendingUpdates.PendingUpdateList[lobby.LobbyId].Count, pendingUpdates.AutoUpdateBaseAddress[lobby.LobbyId], 
                        lobby.LobbyId, file.Filename, downloadPath, lobby.Name, progressCallback);

                    message += " Succeeded.";
                }
                catch (Exception error)
                {
                    message += string.Concat(" Failed: ", error.Message);
					message += "\n autoUpdateURL: " + pendingUpdates.AutoUpdateBaseAddress[lobby.LobbyId];
					message += "\n lobby.LobbyId: " + lobby.LobbyId;
					message += "\n received: " + received;
					message += "\n updateQueue.Count: " + pendingUpdates.PendingUpdateList[lobby.LobbyId].Count;
					message += "\n file.Filename: " + file.Filename;
					message += "\n downloadPath: " + downloadPath;
					message += "\n exception details: " + error.ToString();
                    throw;
                }
                finally
                {
                    Log.Write(message);
                }
            }

            //Save updated AutoUpdateResults dictionary to datastore
			if (pendingUpdates.PendingUpdateList[lobby.LobbyId].Count != 0)
            {
                var dictionary = new Dictionary<string, FindAutoUpdateFilesResult>();
				foreach (var f in pendingUpdates.AllFilesInUpdatePackage[lobby.LobbyId])
                    dictionary.Add(f.Filename, f);

				autoUpdate[dataKey] = dictionary;
                autoUpdate.Save();
            }

			if (updateExe) //Start the new executable, which will wait for this process to complete its autoupdate
			{
				var signal = new UiInteraction(delegate()
				{
					MessageBox.Show("The Allegiance Security Client has an update and must restart.");
					var tempLauncher = Path.Combine(AllegianceRegistry.LobbyPath, GlobalSettings.TempExecutableName);
					Process.Start(tempLauncher);
					Application.Exit();
				});

				if (parentControl.InvokeRequired == true)
					parentControl.Invoke(signal);
				else
					signal();
			}
            else
            {
                //Update the registry to assign the configuration url to the lobby's host value
				UpdateConfigurationUrlForSelectedLobby(lobby);
            }

            return updateExe;
        }

		/// <summary>
		/// If the CfgFile is not set in the allegiance registry key, then point it to the current lobby.
		/// If the CfgFile does not match the current lobby host, then point it to the config file hosted at the lobby.
		/// Note: If you are debugging, be sure to add a mime header for text/plain to your local IIS 
		/// to enable it to serve .cfg files.
		/// </summary>
		/// <param name="lobby"></param>
		private static void UpdateConfigurationUrlForSelectedLobby(LobbyResult lobby)
		{
			Uri currentConfigurationUri;

			if (String.IsNullOrEmpty(AllegianceRegistry.CfgFile) == true || Uri.IsWellFormedUriString(AllegianceRegistry.CfgFile, UriKind.Absolute) == false)
				currentConfigurationUri = new Uri(string.Format("http://{0}/allegiance.cfg", lobby.Host));
			else
				currentConfigurationUri = new Uri(AllegianceRegistry.CfgFile);

			if(currentConfigurationUri.Host.Equals(lobby.Host, StringComparison.InvariantCultureIgnoreCase) == false)
				currentConfigurationUri = new Uri(string.Format("http://{0}/allegiance.cfg", lobby.Host));

		}

        private static bool SetProgress(AutoupdateProgressCallback progressCallback, string lobbyName, int numBytes, int totalBytes, int downloadedFiles, int totalFiles, DateTime downloadStart)
        {
			if (numBytes <= 0 || totalBytes <= 0)
				return true;

            int progress = (int)((float)numBytes / totalBytes * 100);

			if (progressCallback != null)
				return progressCallback(
					lobbyName, 
					string.Format("Downloading Updates ({0}/{1})\n\n{2} kB of {3} kB - {4} kB/s",
						downloadedFiles, 
						totalFiles,
						numBytes / 1024,
						totalBytes / 1024,
						Math.Round(numBytes / 1024 / DateTime.Now.Subtract(downloadStart).TotalSeconds)), 
					progress);

			return true;
        }

        private static bool DownloadFile(int index, int count, string baseUrl, 
            int lobbyId, string filename, string destination, string lobbyName, AutoupdateProgressCallback progressCallback)
        {
			bool continueDownloading = true;

            const int interval  = 800;
            int lastUpdate      = Environment.TickCount - interval;

			var request = HttpWebRequest.Create(string.Format("{0}/Files/{1}/{2}", baseUrl, lobbyId, filename));

            using (var response = request.GetResponse())
            using (var sr       = new BinaryReader(response.GetResponseStream()))
            using (var sw       = new BinaryWriter(File.Open(destination, FileMode.Create)))
            {
				int length;

				if (Int32.TryParse(response.Headers["Content-Length"], out length) == false)
					length = 0;

                int read;
                var buffer = new byte[BufferSize];
				DateTime downloadStart = DateTime.Now;
				int numBytesRead;

				while ((read = sr.Read(buffer, 0, BufferSize)) > 0 && continueDownloading == true)
                {
                    sw.Write(buffer, 0, read);

                    if (Environment.TickCount - lastUpdate > interval)
                    {
						numBytesRead = (int)sw.BaseStream.Position;

						if (length == 0)
							numBytesRead = 0;

						continueDownloading = SetProgress(progressCallback, lobbyName, numBytesRead, length, index, count, downloadStart);
                        lastUpdate = Environment.TickCount;

                    }
                }

				numBytesRead = (int)sw.BaseStream.Position;

				if (length == 0)
					numBytesRead = 0;

				continueDownloading = SetProgress(progressCallback, lobbyName, numBytesRead, length, index, count, downloadStart);
            }

			return continueDownloading;
        }

        /// <summary>
        /// Check running process to see if this is temporary
        /// </summary>
        internal static bool CheckTemporaryProcess()
        {
            try
            {
                var current = Process.GetCurrentProcess();

                //See if the client has recently been updated
                if (string.Equals(GlobalSettings.TempProcessName,
                    Path.GetFileName(current.ProcessName)))
                {
                    //Copy this file to to replace the existing launcher
                    AutoUpdate.WaitForEnd(GlobalSettings.ClientProcessName);
					AutoUpdate.WaitForFileRelease(GlobalSettings.ClientExecutableName, MaximumTimeToWaitForFileReleaseMS);
                    File.Copy(GlobalSettings.TempExecutableName, GlobalSettings.ClientExecutableName, true);

					if(File.Exists(GlobalSettings.TempExecutablePDB) == true)
						File.Copy(GlobalSettings.TempExecutablePDB, GlobalSettings.ClientExecutablePDB, true);

					var launcherPath = Path.Combine(AllegianceRegistry.LobbyPath, GlobalSettings.ClientExecutableName);
					 
					Process.Start(launcherPath);
                    return true; 
                }
                else if (File.Exists(GlobalSettings.TempExecutableName))
                {
                    AutoUpdate.WaitForEnd(GlobalSettings.TempProcessName);
					AutoUpdate.WaitForFileRelease(GlobalSettings.TempExecutableName, MaximumTimeToWaitForFileReleaseMS);
                    File.Delete(GlobalSettings.TempExecutableName);

					if (File.Exists(GlobalSettings.TempExecutablePDB) == true)
						File.Delete(GlobalSettings.TempExecutablePDB);
                }
            }
            catch (Exception error)
            {
				Log.Write(error);
				MessageBox.Show("Error updating launcher:\n" + error.Message + "\n\nSee log file for details.", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

		private static void WaitForFileRelease(string fileName, int timeoutInMilliseconds)
		{
			if (File.Exists(fileName) == false)
				return;

			for (int i = 0; i < timeoutInMilliseconds; i += 100)
			{
				try
				{
					File.OpenWrite(fileName).Close();
					break;
				}
				catch
				{
					Thread.Sleep(100);
				}
			}
		}

		/// <summary>
		/// Wait until specified process has ended
		/// </summary>
		private static void WaitForEnd(string process)
        {
            var processes = Process.GetProcessesByName(process);

            if (processes != null)
            {
                foreach (var p in processes)
                    p.WaitForExit();
            }
        }

        #endregion
    }
}