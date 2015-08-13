using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Allegiance.CommunitySecuritySystem.Client.Utility;
using Allegiance.CommunitySecuritySystem.Client.Service;
using System.Threading;

namespace Allegiance.CommunitySecuritySystem.Client.Integration
{
    class SystemWatcher
    {
        #region Fields

        private static string _basePath = null;

        private static FileSystemWatcher _watcher;

        private static List<string> _protectedFiles;

        #endregion

        #region Events

        static void _watcher_Changed(object sender, FileSystemEventArgs e)
        {
            DebugDetector.AssertCheckRunning();

            AssertNotProtected(GetRelativePath(e.FullPath));
        }

        static void _watcher_Renamed(object sender, RenamedEventArgs e)
        {
            DebugDetector.AssertCheckRunning();

            AssertNotProtected(GetRelativePath(e.FullPath));
        }

        #endregion

        #region Methods

		public static void InitializeWithAutoupdateProtectedFileList()
		{
			var pendingUpdates = AutoUpdate.GetPendingUpdateQueues(ServiceHandler.Service);

			var protectedList = new List<string>();
			foreach (var filesInLobbyUpdatePackage in pendingUpdates.AllFilesInUpdatePackage)
			{
				foreach (var item in filesInLobbyUpdatePackage.Value)
				{
					string basePath = String.Empty;

					if (item.LobbyId != null)
						basePath = ((LobbyType)item.LobbyId).ToString();

					if (item.IsProtected)
					{
						var filename = item.Filename.ToLower();

						if (filename.StartsWith(@"\")) //Remove preceding '\'
							filename = filename.Remove(0, 1);

						protectedList.Add(Path.Combine(basePath, filename).ToLower());
					}
				}
			}

			SystemWatcher.Initialize(protectedList);
		}

        public static void Initialize(List<string> list)
        {
            _basePath       = AllegianceRegistry.LobbyPath;
            _protectedFiles = list;

            _watcher = new FileSystemWatcher(_basePath)
            {
                IncludeSubdirectories   = true,
                EnableRaisingEvents     = true,
            };

            _watcher.Renamed += new RenamedEventHandler(_watcher_Renamed);
            _watcher.Changed += new FileSystemEventHandler(_watcher_Changed);
        }

        private static string GetRelativePath(string fullPath)
        {
            var filename = fullPath.Remove(0, _basePath.Length).ToLower();

            if (filename.StartsWith(@"\")) //Remove preceding '\'
                filename = filename.Remove(0, 1);

            return filename;
        }

        private static void AssertNotProtected(string filename)
        {
            if (_protectedFiles.Contains(filename))
            {
                const string message = "Protected file modified while Allegiance was running.";
				Log.Write(message);

				// Give enough time for Allegiance to arrive.
				Thread.Sleep(3000);

                AllegianceLoader.ExitAllegiance();

				MessageBox.Show(message, "Please do not modify protected Allegiance files.", MessageBoxButtons.OK, MessageBoxIcon.Hand);

				//Application.Exit();
               
            }
        }

        public static void Close()
        {
            if (_watcher != null)
            {
                _watcher.Dispose();
                _watcher = null;
            }
        }

        #endregion
    }
}