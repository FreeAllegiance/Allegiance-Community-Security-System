using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Allegiance.CommunitySecuritySystem.DataAccess;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Allegiance.CommunitySecuritySystem.Common.Utility;

namespace Allegiance.CommunitySecuritySystem.BlackboxGenerator
{
    public static class Test
    {
        public static void CreateUser(string user, string password, string email)
        {
            using (var db = new CSSDataContext())
            {
                var identity = new Identity()
                {
                    DateLastLogin = DateTime.Now,
					LastGlobalMessageDelivery = DateTime.Now
                };

                var login = new Login()
                {
                    Username    = user,
                    Password    = Hash(password),
                    Email       = email,
                    DateCreated = DateTime.Now,
                };

                var alias = new Alias()
                {
                    Callsign    = user,
                    DateCreated = DateTime.Now,
                    IsDefault   = true,
					IsActive = true
                };

                login.Aliases.Add(alias);
                identity.Logins.Add(login);
                login.Lobby_Logins.Add(new Lobby_Login() { Lobby = db.Lobbies.First(p => p.Name == "Production") });

                db.Identities.InsertOnSubmit(identity);
                db.SubmitChanges();
            }
        }

        public static string Hash(string password)
        {
			return PasswordHash.CreateHash(password);
        }

        private static string HashFile(string path)
        {
            using (var sha = new SHA1Managed())
                return Convert.ToBase64String(sha.ComputeHash(File.ReadAllBytes(path)));
        }

        public static void AddAutoUpdateFiles(string baseDirectory)
        {
            using (var db = new CSSDataContext())
            {
                foreach (var path in Directory.GetDirectories(baseDirectory))
                {
                    int lobbyId;
                    if (!int.TryParse(Path.GetFileName(path), out lobbyId))
                        continue;

                    //Find lobby
                    var lobby = db.Lobbies.FirstOrDefault(p => p.Id == lobbyId);
                    var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);

                    foreach (var filePath in files)
                    {
                        string file = filePath.Substring(path.Length, filePath.Length - path.Length);
                        if (file.StartsWith(@"\"))
                            file = file.Remove(0, 1);

                        //Check if file_lobby exists
                        var fileLobby = lobby.AutoUpdateFile_Lobbies.FirstOrDefault(p => p.AutoUpdateFile.Filename == file);
                        if (fileLobby == null)
                        {
                            //Check if file exists
                            var autoUpdateFile = db.AutoUpdateFiles.FirstOrDefault(p => p.Filename == file);

                            if (autoUpdateFile == null)
                            {
                                //Create autoupdatefile
                                autoUpdateFile = new AutoUpdateFile()
                                {
                                    Filename    = file,
                                    IsProtected = true
                                };
                            }

                            fileLobby = new AutoUpdateFile_Lobby()
                            {
                                AutoUpdateFile  = autoUpdateFile,
                                CurrentVersion  = new Version(1, 0).ToString(),
                                ValidChecksum   = HashFile(filePath),
                                DateCreated     = DateTime.Now,
                                DateModified    = DateTime.Now,
                            };

                            lobby.AutoUpdateFile_Lobbies.Add(fileLobby);
                        }
                        else
                        {
                            var version                 = new Version(fileLobby.CurrentVersion);
                            var nextVersion             = new Version(version.Minor, version.Minor + 1);
                            fileLobby.CurrentVersion    = nextVersion.ToString();
                            fileLobby.ValidChecksum     = HashFile(filePath);
                            fileLobby.DateModified      = DateTime.Now;
                        }
                    }

                    db.SubmitChanges();
                }
            }
        }
    }
}