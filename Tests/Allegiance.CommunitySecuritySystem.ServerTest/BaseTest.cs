using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Allegiance.CommunitySecuritySystem.Common.Enumerations;
using Allegiance.CommunitySecuritySystem.DataAccess;
using Microsoft.SqlServer.Management.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SMO = Microsoft.SqlServer.Management.Smo;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using Allegiance.CommunitySecuritySystem.Common.Utility;

namespace Allegiance.CommunitySecuritySystem.ServerTest
{
	[TestClass]
    public class BaseTest
    {
        private static bool _initialized = false;

        [TestInitialize]
        public void ClearDatabase()
        {
            if (_initialized)
                return;

            using (var db = new CSSDataContext())
            {
                //Clear data
                try
                {
                    db.DeleteDatabase();
                }
                catch(Exception ex)
				{
					Debug.WriteLine(ex.ToString());
					
				}

                db.CreateDatabase();

                //Create any other functions which weren't created
                SetupFunctions(db);

                //Insert any test data
                InsertTestData(db);

                //Submit changes
                db.SubmitChanges();

                _initialized = true;
            }
        }

		public void DeleteDatabase(CSSDataContext db)
		{
			db.DeleteDatabase();
			_initialized = false;
		}

        private void SetupFunctions(CSSDataContext db)
        {
            var path        = ConfigurationManager.AppSettings["TestRoot"];
            var script      = File.ReadAllText(Path.Combine(path, "CreateScript.sql"));
            var connection  = db.Connection as SqlConnection;

            var serverConnection    = new ServerConnection(connection);
            var server              = new SMO.Server(serverConnection);

            server.ConnectionContext.ExecuteNonQuery(script);
        }

        private void InsertTestData(CSSDataContext db)
        {
            //Create test user
            CreateUser("Orion", "Test", "Test", 10);
			CreateUser("BackTrak", "Test", "Test", 20);
			CreateUser("TheBored", "Test", "Test", 30);

			for (int i = 0; i < 30; i++)
				CreateUser("TestPilot" + i, "Test", "Test", i * 6);

			
            //Create a tranform method
            db.TransformMethods.InsertOnSubmit(new TransformMethod()
            { Text = "int len = rand.Next(189, 350); for (int i = rand.Next(15, 35); i < len; i += 2) sb.Append((char)rand.Next(48, 122));" });

            //Create session statuses
            db.SessionStatus.InsertOnSubmit(new SessionStatus() { Id = 1, Name = "Pending Verification" });
            db.SessionStatus.InsertOnSubmit(new SessionStatus() { Id = 2, Name = "Active" });
            db.SessionStatus.InsertOnSubmit(new SessionStatus() { Id = 3, Name = "Closed" });

            //Create Roles
            db.Roles.InsertOnSubmit(new Role() { Id = (int)RoleType.SuperAdministrator, Name = RoleType.SuperAdministrator.ToString() });
            db.Roles.InsertOnSubmit(new Role() { Id = (int)RoleType.Administrator, Name = RoleType.Administrator.ToString() });
            db.Roles.InsertOnSubmit(new Role() { Id = (int)RoleType.Moderator, Name = RoleType.Moderator.ToString() });
            db.Roles.InsertOnSubmit(new Role() { Id = (int)RoleType.ZoneLeader, Name = RoleType.ZoneLeader.ToString() });
            db.Roles.InsertOnSubmit(new Role() { Id = (int)RoleType.User, Name = RoleType.User.ToString() });

            //Create lobby
            db.Lobbies.InsertOnSubmit(new Lobby() { Name = "Production", IsRestrictive = false, IsEnabled = true, BasePath = "test", Host = "test.alleg.net" });

			// Create GroupRole
			db.GroupRoles.InsertOnSubmit(new GroupRole() { Token = '*', Name="Squad Leader" });
			db.GroupRoles.InsertOnSubmit(new GroupRole() { Token = '^', Name="Assistant Squad Leader" });
			db.GroupRoles.InsertOnSubmit(new GroupRole() { Token = '+', Name="Zone Lead" });
			db.GroupRoles.InsertOnSubmit(new GroupRole() { Token = '$', Name="Developer" });
			db.GroupRoles.InsertOnSubmit(new GroupRole() { Token = '?', Name = "Help Desk" });
			db.GroupRoles.InsertOnSubmit(new GroupRole() { Token = null, Name="Pilot" });

			// Add localhost game servers
			using (CSSStatsDataContext statsDB = new CSSStatsDataContext())
			{
				var gameServer = new DataAccess.GameServer() { GameServerName = "Test Server", GameServerOwnerName = "BackTrak" };
				statsDB.GameServers.InsertOnSubmit(gameServer);

				//db.GameServerIPs.InsertOnSubmit(new GameServerIP() { GameServer = gameServer, GameServerID = gameServer.GameServerID, IPAddress = "127.0.0.1" });
				statsDB.GameServerIPs.InsertOnSubmit(new GameServerIP() { GameServer = gameServer, IPAddress = "127.0.0.1" });

				statsDB.SubmitChanges();
			}

			//foreach(string name in Enum.GetNames(typeof(DataAccess.Enumerations.LinkedItemType)))
			//{
			//    db.LinkedItemTypes.InsertOnSubmit(new LinkedItemType()
			//    {
			//        Id = (int) Enum.Parse(typeof(DataAccess.Enumerations.LinkedItemType), name),
			//        Name = name
			//    });
			//}

			LoadTestMachineRecords();

			LoadTestMachineExclusions();

			LoadTestVirtualMachineMarkers();
        }

		private void LoadTestVirtualMachineMarkers()
		{
			using (CSSDataContext db = new CSSDataContext())
			{
				db.VirtualMachineMarkers.InsertOnSubmit(new VirtualMachineMarker()
				{
					IdentifierMask = "%|Virtual HD|%",
					RecordTypeId = 2
				});

				db.VirtualMachineMarkers.InsertOnSubmit(new VirtualMachineMarker()
				{
					IdentifierMask = "%|VirtualBox %",
					RecordTypeId = 1
				});

				db.VirtualMachineMarkers.InsertOnSubmit(new VirtualMachineMarker()
				{
					IdentifierMask = "%|VMware %",
					RecordTypeId = 1
				});

				db.VirtualMachineMarkers.InsertOnSubmit(new VirtualMachineMarker()
				{
					IdentifierMask = "%|QEMU HARDDISK|%",
					RecordTypeId = 2
				});

				db.SubmitChanges();
			}
		}

		private void LoadTestMachineExclusions()
		{
			using (CSSDataContext db = new CSSDataContext())
			{
				db.MachineRecordExclusions.InsertOnSubmit(new MachineRecordExclusion()
				{
					Id = 1,
					IdentifierMask = "Volume0",
					RecordTypeId = 2
				});

				db.MachineRecordExclusions.InsertOnSubmit(new MachineRecordExclusion()
				{
					Id = 2,
					IdentifierMask = "%Volume1",
					RecordTypeId = 2
				});

				db.MachineRecordExclusions.InsertOnSubmit(new MachineRecordExclusion()
				{
					Id = 3,
					IdentifierMask = "Volume2%",
					RecordTypeId = 2
				});

				db.MachineRecordExclusions.InsertOnSubmit(new MachineRecordExclusion()
				{
					Id = 4,
					IdentifierMask = "%Volume3%",
					RecordTypeId = 2
				});

				db.SubmitChanges();
			}
		}

		protected void LoadTestMachineRecords()
		{
			using (CSSDataContext db = new CSSDataContext())
			{
				db.MachineRecords.DeleteAllOnSubmit(db.MachineRecords);
				db.MachineRecordTypes.DeleteAllOnSubmit(db.MachineRecordTypes);

				db.MachineRecordTypes.InsertOnSubmit(new MachineRecordType() { Id = 1, Name = "Network" });
				db.MachineRecordTypes.InsertOnSubmit(new MachineRecordType() { Id = 2, Name = "HardDisk" });
				db.MachineRecordTypes.InsertOnSubmit(new MachineRecordType() { Id = 3, Name = "EDID" });
				db.MachineRecordTypes.InsertOnSubmit(new MachineRecordType() { Id = 4, Name = "Serial" });
				db.MachineRecordTypes.InsertOnSubmit(new MachineRecordType() { Id = 5, Name = "Misc" });

				db.SubmitChanges();
			}

			//Regex: (?<index>\d+),(?<identityID>\d+),(?<recordTypeID>\d+),(?<identifier>.*?),(?<loginID>\d+)
			Regex rowFinder = new Regex(
				  "(?<index>\\d+),(?<identityID>\\d+),(?<recordTypeID>\\d+),(?<" +
				  "identifier>.*?),(?<loginID>\\d+)",
				RegexOptions.IgnoreCase
				| RegexOptions.Singleline
				| RegexOptions.CultureInvariant
				| RegexOptions.Compiled
				);

			string machineRecordsFile = Path.Combine(ConfigurationManager.AppSettings["gameTestDataDirectory"], "machinerecords.csv");
			string allRecords = File.ReadAllText(machineRecordsFile);

			using (CSSDataContext db = new CSSDataContext())
			{
				foreach (Match match in rowFinder.Matches(allRecords))
				{
					db.MachineRecords.InsertOnSubmit(new MachineRecord()
					{
						//IdentityId = db.Logins.FirstOrDefault(p => p.Id == Int32.Parse(match.Groups["identityID"].Value)).IdentityId,
						RecordTypeId = Int32.Parse(match.Groups["recordTypeID"].Value),
						Identifier = match.Groups["identifier"].Value,
						LoginId = Int32.Parse(match.Groups["loginID"].Value)
					});

					db.SubmitChanges();
				}

				db.SubmitChanges();
			}
		}
        
        public static Login CreateUser(string user, string password, string email, int ipBaseIndex)
        {
            using (var db = new CSSDataContext())
            {
				var existingAlias = Alias.GetAliasByCallsign(db, user);
				if (existingAlias != null)
				{
					db.Group_Alias_GroupRoles.DeleteAllOnSubmit(existingAlias.Login.Aliases.SelectMany(p => p.Group_Alias_GroupRoles));
					db.PollVotes.DeleteAllOnSubmit(existingAlias.Login.Identity.Logins.SelectMany(p => p.PollVotes));
					db.GroupMessage_Alias.DeleteAllOnSubmit(existingAlias.Login.Aliases.SelectMany(p => p.GroupMessage_Alias));
					db.PersonalMessages.DeleteAllOnSubmit(existingAlias.Login.Aliases.SelectMany(p => p.PersonalMessages));

					var loginsToDelete = existingAlias.Login.Identity.Logins.ToList();
					var identityToDelete = existingAlias.Login.Identity;
					//List<Identity> identiesToDelete = existingAlias.Login.Identity.Logins.SelectMany(p => p.Identity).ToList();

					db.MachineRecords.DeleteAllOnSubmit(existingAlias.Login.Identity.MachineRecords);
					db.SubmitChanges();

					db.Alias.DeleteAllOnSubmit(existingAlias.Login.Identity.Logins.SelectMany(p  => p.Aliases));
					db.SubmitChanges();
					
					db.Logins.DeleteAllOnSubmit(loginsToDelete);
					db.SubmitChanges();

					db.LogIPs.DeleteAllOnSubmit(identityToDelete.LogIPs);
					db.Identities.DeleteOnSubmit(identityToDelete);
					db.SubmitChanges();
				}

                var identity = new Identity()
                {
                    DateLastLogin = DateTime.Now,
					LastGlobalMessageDelivery = DateTime.Now
                };

                var login = new Login()
                {
                    Username    = user,
                    Password    = PasswordHash.CreateHash(password),
                    Email       = email,
                    DateCreated = DateTime.Now,
                };

                var alias = new Alias()
                {
                    Callsign    = user,
                    DateCreated = DateTime.Now,
                    IsDefault   = true,
                };

                login.Aliases.Add(alias);
                identity.Logins.Add(login);

                db.Identities.InsertOnSubmit(identity);

				for (int i = 0; i < 5; i++)
				{
					identity.LogIPs.Add(new LogIP()
					{
						IPAddress = "192.168.1." + (ipBaseIndex + i).ToString(),
						LastAccessed = DateTime.Now
					});
				}


                db.SubmitChanges();

				

                return login;
            }
        }

		//public static string Hash(string password)
		//{
		//    return password;
		//    //return PasswordHash.CreateHash(password);
		//}
    }
}