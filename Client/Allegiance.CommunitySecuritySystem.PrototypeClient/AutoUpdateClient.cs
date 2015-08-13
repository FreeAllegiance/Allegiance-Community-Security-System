using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using Allegiance.CommunitySecuritySystem.PrototypeClient.localhost;

namespace Allegiance.CommunitySecuritySystem.PrototypeClient
{
    public static class AutoUpdateClient
    {
        public static void Check(ClientService service)
        {
            //Initialize Checksum class to use SHA1
            Checksum.Initialize<SHA1>();

            //Get available lobbies
            var lobbies     = service.CheckAvailableLobbies(new AuthenticatedData());

            //Get production lobby
            var production  = GetProduction(lobbies);

            //Get autoupdate files associated with lobby
            var results     = service.CheckForUpdates(production.LobbyId, true);

            var root        = @"C:\AutoUpdate";
            var updateQueue = new List<FindAutoUpdateFilesResult>();
            var dataStore   = DataStore.Open(Path.Combine(root, "configuration.ds"), "doodly");
            var lastResults = dataStore["AutoUpdate"] as Dictionary<string, FindAutoUpdateFilesResult>;

            foreach(var file in results.Files)
            {
                var path = Path.Combine(root, file.Filename);

                //Check that all files exist
                if (!File.Exists(path))
                {
                    updateQueue.Add(file);
                    continue;
                }

                if (lastResults == null)
                {
                    updateQueue.Add(file);
                    continue;
                }

                //Check that all file versions match
                if (lastResults.ContainsKey(file.Filename))
                {
                    if (file.CurrentVersion != lastResults[file.Filename].CurrentVersion)
                    {
                        updateQueue.Add(file);
                        continue;
                    }
                }

                //Compare hashes with all protected files to is on-disk
                if (file.IsProtected)
                {
                    var checksum = Checksum.Calculate(path);
                    if (file.ValidChecksum != checksum)
                        updateQueue.Add(file);
                }
            }

            //Get files from auto-update
            foreach(var file in updateQueue)
            {
                var downloadPath    = Path.Combine(root, file.Filename);

                if (!Directory.Exists(Path.GetDirectoryName(downloadPath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(downloadPath));

                DownloadFile(production.LobbyId, file.Filename, downloadPath);
            }

            //Save updated AutoUpdateResults dictionary to datastore
            if (updateQueue.Count != 0)
            {
                var dictionary = new Dictionary<string, FindAutoUpdateFilesResult>();
                foreach (var f in results.Files)
                    dictionary.Add(f.Filename, f);

                dataStore["AutoUpdate"] = dictionary;

                dataStore.Save();
            }
        }

        private static void DownloadFile(int lobbyId, string filename, string destination)
        {
            var request = HttpWebRequest.Create(string.Format("http://localhost/autoupdate/Files/{0}/{1}", lobbyId, filename));

            using (var response = request.GetResponse())
            using (var sr = new BinaryReader(response.GetResponseStream()))
            using (var sw = new BinaryWriter(File.Open(destination, FileMode.Create)))
            {
                byte[] buffer = new byte[512];
                int read;
                while ((read = sr.Read(buffer, 0, 512)) > 0)
                    sw.Write(buffer, 0, read);
            }
        }

        private static LobbyResult GetProduction(LobbyResult[] results)
        {
            foreach (var l in results)
            {
                if (l.Name == "Production")
                    return l;
            }
            throw new Exception("Could not find specified lobby.");
        }
    }
}