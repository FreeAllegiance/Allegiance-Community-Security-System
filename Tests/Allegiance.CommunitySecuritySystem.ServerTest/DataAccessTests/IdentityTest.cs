using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Allegiance.CommunitySecuritySystem.DataAccess;
using Allegiance.CommunitySecuritySystem.Server;
using Allegiance.CommunitySecuritySystem.Server.Contracts;
using System.Data.Linq.SqlClient;
using Allegiance.CommunitySecuritySystem.Common.Envelopes.AuthInfo;
using Allegiance.CommunitySecuritySystem.Common.Utility;

namespace Allegiance.CommunitySecuritySystem.ServerTest.DataAccessTest
{
	/// <summary>
	/// Summary description for Identity
	/// </summary>
	[TestClass]
	public class IdentityTest : BaseTest
	{
		public IdentityTest()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		[TestMethod]
		public void TestMatching_UniqueRecords()
		{
			LoadTestMachineRecords();

			MachineInformation machineInformation = new MachineInformation();
			machineInformation.MachineValues = new List<DeviceInfo>();
			
			using (CSSDataContext db = new CSSDataContext())
			{
				// Get all machine records for login 1.
				var machineRecords = db.MachineRecords.Where(p => p.LoginId == 1);
				foreach (var machineRecord in machineRecords)
				{
					machineInformation.MachineValues.Add(new DeviceInfo()
					{
						Name = machineRecord.MachineRecordType.Name,
						Type = (DeviceType)machineRecord.MachineRecordType.Id,
						Value = machineRecord.Identifier
					});
				}

				var login = db.Logins.FirstOrDefault(p => p.Id == 1);

				Identity identity;
				bool wasMerged;
				Identity.MatchIdentity(db, login, machineInformation, out identity, out wasMerged);

				db.SubmitChanges();

				Assert.IsTrue(login.Identity == identity);

				// Shouldn't be linked to any other accounts.
				Assert.AreEqual(0, login.Identity.Logins.Where(p => p.Id != login.Id).Count());
				Assert.AreEqual(0, login.Identity.MachineRecords.Where(p => p.LoginId != login.Id).Count());
				Assert.AreEqual(0, login.Identity.PollVotes.Where(p => p.LoginId != login.Id).Count());
				Assert.AreEqual(0, login.Identity.Bans.Where(p => p.LoginId != login.Id).Count());
			}
		}

		[TestMethod]
		public void TestMatching_DuplicateHardDriveRecords()
		{
			LoadTestMachineRecords();

			MachineInformation machineInformation = new MachineInformation();
			machineInformation.MachineValues = new List<DeviceInfo>();

			using (CSSDataContext db = new CSSDataContext())
			{
				// Get all machine records for login 12.
				var machineRecords = db.MachineRecords.Where(p => p.LoginId == 12);
				foreach (var machineRecord in machineRecords)
				{
					machineInformation.MachineValues.Add(new DeviceInfo()
					{
						Name = machineRecord.MachineRecordType.Name,
						Type = (DeviceType)machineRecord.MachineRecordType.Id,
						Value = machineRecord.Identifier
					});
				}

				machineInformation.MachineValues.Add(new DeviceInfo()
				{
					Name = DeviceType.HardDisk.ToString(),
					Type = DeviceType.HardDisk,
					Value = "This is a test harddisk!"
				});

				var login12 = db.Logins.FirstOrDefault(p => p.Id == 12);
				var login13 = db.Logins.FirstOrDefault(p => p.Id == 13);

				Identity identity;
				bool wasMerged;
				Identity.MatchIdentity(db, login12, machineInformation, out identity, out wasMerged);

				db.SubmitChanges();

				Assert.IsTrue(login12.Identity == identity);

				// Should be linked to account 13.
				Assert.AreEqual(2, db.Logins.FirstOrDefault(p => p.Id == 12).Identity.Logins.Count);

				Assert.AreEqual(1, login12.Identity.Logins.Where(p => p.Id != login12.Id).Count());
				Assert.AreEqual(1, login12.Identity.MachineRecords.Where(p => p.LoginId != login12.Id).Count());
				Assert.AreEqual(0, login12.Identity.PollVotes.Where(p => p.LoginId != login12.Id).Count());
				Assert.AreEqual(0, login12.Identity.Bans.Where(p => p.LoginId != login12.Id).Count());

				Assert.IsTrue(Identity.IsLinkedToAnOlderAccount(db, login13.Username));

				Assert.AreEqual(login12.Username, Identity.GetOldestLinkedAcccountUsername(db, login13.Username));

			}
		}

		[TestMethod]
		public void TestMatching_DuplicateNetworkAdapterRecords()
		{
			LoadTestMachineRecords();

			MachineInformation machineInformation = new MachineInformation();
			machineInformation.MachineValues = new List<DeviceInfo>();

			using (CSSDataContext db = new CSSDataContext())
			{
				// Get all machine records for login 21.
				var machineRecords = db.MachineRecords.Where(p => p.LoginId == 21);
				foreach (var machineRecord in machineRecords)
				{
					machineInformation.MachineValues.Add(new DeviceInfo()
					{
						Name = machineRecord.MachineRecordType.Name,
						Type = (DeviceType)machineRecord.MachineRecordType.Id,
						Value = machineRecord.Identifier
					});
				}

				//machineInformation.MachineValues.Add(new DeviceInfo()
				//{
				//    Name = DeviceType.HardDisk.ToString(),
				//    Type = DeviceType.HardDisk,
				//    Value = "This is a test harddisk!"
				//});

				var login = db.Logins.FirstOrDefault(p => p.Id == 21);

				Identity identity;
				bool wasMerged;
				Identity.MatchIdentity(db, login, machineInformation, out identity, out wasMerged);

				db.SubmitChanges();

				Assert.IsTrue(login.Identity == identity);

				// Should NOT be linked to account 22.
				Assert.AreEqual(1, db.Logins.FirstOrDefault(p => p.Id == 21).Identity.Logins.Count);
			}
		}


		[TestMethod]
		public void TestMatchingForDisparateMachineRecord()
		{
			using (CSSDataContext db = new CSSDataContext())
			{
				db.MachineRecords.DeleteAllOnSubmit(db.MachineRecords);
				db.SubmitChanges();

				LoadTestMachineRecords();

				//// Remove any existing links for login 15.
				//var linkedItemsToDelete = db.LinkedItems.Where(p => p.TargetId == 15 && p.LinkedItemTypeId == 1);
				//db.LinkedItems.DeleteAllOnSubmit(linkedItemsToDelete);
				//db.Links.DeleteAllOnSubmit(linkedItemsToDelete.Select(p => p.Link));
				//db.SubmitChanges();

				//// Remove any existing links for login 16.
				//linkedItemsToDelete = db.LinkedItems.Where(p => p.TargetId == 16 && p.LinkedItemTypeId == 1);
				//db.LinkedItems.DeleteAllOnSubmit(linkedItemsToDelete);
				//db.Links.DeleteAllOnSubmit(linkedItemsToDelete.Select(p => p.Link));
				//db.SubmitChanges();

				//// Remove any existing links for login 17.
				//linkedItemsToDelete = db.LinkedItems.Where(p => p.TargetId == 17 && p.LinkedItemTypeId == 1);
				//db.LinkedItems.DeleteAllOnSubmit(linkedItemsToDelete);
				//db.Links.DeleteAllOnSubmit(linkedItemsToDelete.Select(p => p.Link));
				//db.SubmitChanges();

				MachineInformation machineInformation = new MachineInformation();
				machineInformation.MachineValues = new List<DeviceInfo>();

				//-------------------------------
				var machineRecords = db.MachineRecords.Where(p => p.LoginId == 15);
				foreach (var machineRecord in machineRecords)
				{
					machineInformation.MachineValues.Add(new DeviceInfo()
					{
						Name = machineRecord.MachineRecordType.Name,
						Type = (DeviceType)machineRecord.MachineRecordType.Id,
						Value = machineRecord.Identifier
					});
				}

				var login15 = db.Logins.FirstOrDefault(p => p.Id == 15);

				Identity identity;
				bool wasMerged;
				Identity.MatchIdentity(db, login15, machineInformation, out identity, out wasMerged);

				//-------------------------------

				machineInformation = new MachineInformation();
				machineInformation.MachineValues = new List<DeviceInfo>();

				machineRecords = db.MachineRecords.Where(p => p.LoginId == 16);
				foreach (var machineRecord in machineRecords)
				{
					machineInformation.MachineValues.Add(new DeviceInfo()
					{
						Name = machineRecord.MachineRecordType.Name,
						Type = (DeviceType)machineRecord.MachineRecordType.Id,
						Value = machineRecord.Identifier
					});
				}

				var login16 = db.Logins.FirstOrDefault(p => p.Id == 16);
				Identity.MatchIdentity(db, login16, machineInformation, out identity, out wasMerged);

				//-----------------------------

				machineInformation = new MachineInformation();
				machineInformation.MachineValues = new List<DeviceInfo>();

				machineRecords = db.MachineRecords.Where(p => p.LoginId == 17);
				foreach (var machineRecord in machineRecords)
				{
					machineInformation.MachineValues.Add(new DeviceInfo()
					{
						Name = machineRecord.MachineRecordType.Name,
						Type = (DeviceType)machineRecord.MachineRecordType.Id,
						Value = machineRecord.Identifier
					});
				}

				var login17 = db.Logins.FirstOrDefault(p => p.Id == 17);

				Identity.MatchIdentity(db, login17, machineInformation, out identity, out wasMerged);
				

				Assert.AreEqual(5, login15.Identity.MachineRecords.Count());
				Assert.AreEqual(5, login16.Identity.MachineRecords.Count());
				Assert.AreEqual(5, login17.Identity.MachineRecords.Count());
			}
		}


		[TestMethod]
		public void TestMatchingOnLoginWithNewMatchingHardDiskMachineInfo()
		{
			using (CSSDataContext db = new CSSDataContext())
			{
				db.MachineRecords.DeleteAllOnSubmit(db.MachineRecords);
				db.SubmitChanges();

				LoadTestMachineRecords();

				var login23 = db.Logins.FirstOrDefault(p => p.Id == 23);
				var login17 = db.Logins.FirstOrDefault(p => p.Id == 17);
				var hardDiskRecord = login23.MachineRecords.FirstOrDefault(p => p.DeviceType == DeviceType.HardDisk);

				List<DeviceInfo> machineValues = new List<DeviceInfo>();
				machineValues.Add(new DeviceInfo()
				{
					Name = hardDiskRecord.DeviceType.ToString(),
					Type = hardDiskRecord.DeviceType,
					Value = hardDiskRecord.Identifier
				});

				var machineInformation = new MachineInformation()
				{
					MachineValues = machineValues
				};

				Assert.AreEqual(1, login17.Identity.Logins.Count());

				Identity identity;
				bool wasMerged;
				Identity.MatchIdentity(db, login17, machineInformation, out identity, out wasMerged);

				Assert.AreEqual(2, login17.Identity.Logins.Count());
				Assert.IsTrue(wasMerged);

				Identity.MatchIdentity(db, login17, machineInformation, out identity, out wasMerged);

				Assert.AreEqual(2, login17.Identity.Logins.Count());
				Assert.IsFalse(wasMerged);
			}
		}

		[TestMethod]
		public void TestMerging()
		{
			using (DataAccess.CSSDataContext db = new CSSDataContext())
			{
				db.PollVotes.DeleteAllOnSubmit(db.PollVotes);
				db.Bans.DeleteAllOnSubmit(db.Bans);
				db.MachineRecords.DeleteAllOnSubmit(db.MachineRecords);
				db.SubmitChanges();
			}

			Login login1;
			Login login2;
			Login login3;

			CreateTestData(out login1, out login2, out login3);

			int identityId1 = login1.Identity.Id;
			int identityId2 = login2.Identity.Id;
			int identityId3 = login3.Identity.Id;

			using (var db = new CSSDataContext())
			{
				var identities = db.Identities.Where(p => 
					p.Id == login1.Identity.Id
					|| p.Id == login2.Identity.Id
					|| p.Id == login3.Identity.Id);

				DataAccess.Identity.MergeIdentities(db, (IEnumerable<DataAccess.Identity>) identities.ToList());

				db.SubmitChanges();

				// Get the updated login.
				var mergedLogin = DataAccess.Login.FindLoginByUsernameOrCallsign(db, login1.Aliases.First().Callsign);

				// After the merge, the second and third account identities should have been deleted.
				Assert.AreEqual(0, db.Identities.Count(p => p.Id == identityId2));
				Assert.AreEqual(0, db.Identities.Count(p => p.Id == identityId3));

				Assert.AreEqual(3, mergedLogin.Identity.Logins.Count(), "mergedLogin.Identity: " + mergedLogin.Identity.Id);
				Assert.AreEqual(2, mergedLogin.Identity.Bans.Count(), "mergedLogin.Identity: " + mergedLogin.Identity.Id);
				Assert.AreEqual(2, mergedLogin.Identity.MachineRecords.Count(), "mergedLogin.Identity: " + mergedLogin.Identity.Id);
				Assert.AreEqual(1, mergedLogin.Identity.PollVotes.Count(), "mergedLogin.Identity: " + mergedLogin.Identity.Id);
			}
		}

		[TestMethod]
		public void TestUnlinking()
		{
			TestMerging();

			using(DataAccess.CSSDataContext db = new CSSDataContext())
			{
				Identity principal = Alias.GetAliasByCallsign(db, "User1").Login.Identity;
				Login loginToUnlink = Alias.GetAliasByCallsign(db, "User2").Login;

				Identity.UnlinkLogin(db, principal, loginToUnlink);

				loginToUnlink = Alias.GetAliasByCallsign(db, "User3").Login;
				Identity.UnlinkLogin(db, principal, loginToUnlink);

				db.SubmitChanges();

				var user3 = Alias.GetAliasByCallsign(db, "User3").Login;
				Assert.AreEqual(2, user3.Identity.MachineRecords.Count());
				Assert.AreEqual(1, user3.Identity.PollVotes.Count());
				Assert.AreEqual(0, user3.Identity.Bans.Count());

				var user2 = Alias.GetAliasByCallsign(db, "User2").Login;
				Assert.AreEqual(0, user2.Identity.MachineRecords.Count());
				Assert.AreEqual(0, user2.Identity.PollVotes.Count());
				Assert.AreEqual(2, user2.Identity.Bans.Count());

				var user1 = Alias.GetAliasByCallsign(db, "User1").Login;
				Assert.AreEqual(0, user1.Identity.MachineRecords.Count());
				Assert.AreEqual(0, user1.Identity.PollVotes.Count());
				Assert.AreEqual(0, user1.Identity.Bans.Count());
			}
		}

		[TestMethod]
		public void TestUnlinkedLoginMerge()
		{
			Login login1;
			Login login2;
			Login login3;

			CreateTestData(out login1, out login2, out login3);

			using (var db = new CSSDataContext())
			{
				db.Login_UnlinkedLogins.InsertOnSubmit(new Login_UnlinkedLogin()
				{
					LoginId1 = login1.Id,
					LoginId2 = login2.Id
				});

				db.SubmitChanges();

				var identities = db.Identities.Where(p =>
					p.Id == login1.Identity.Id
					|| p.Id == login2.Identity.Id
					|| p.Id == login3.Identity.Id);

				DataAccess.Identity.MergeIdentities(db, (IEnumerable<DataAccess.Identity>)identities.ToList());

				db.SubmitChanges();

				// Get the updated login.
				var mergedLogin = DataAccess.Login.FindLoginByUsernameOrCallsign(db, login1.Aliases.First().Callsign);

				Assert.AreEqual(1, db.Identities.Count(p => p.Id == login2.Identity.Id));
				Assert.AreEqual(0, db.Identities.Count(p => p.Id == login3.Identity.Id));

				Assert.AreEqual(2, mergedLogin.Identity.Logins.Count());
				Assert.AreEqual(0, mergedLogin.Identity.Bans.Count());
				Assert.AreEqual(2, mergedLogin.Identity.MachineRecords.Count());
				Assert.AreEqual(1, mergedLogin.Identity.PollVotes.Count());

				// User 1 has IPs 10, 11, 12, 13, 14 and User 3 has IPs 13, 14, 15, 16, 17. 
				// If the accounts are merged, then the resule should be User 1 + User 3's IPs in the log table assiged to the merged identity.
				Assert.AreEqual(8, mergedLogin.Identity.LogIPs.Count());
			}

		}

		[TestMethod()]
		public void TestIdentityCreate()
		{
			string usernameStartsWithBadWord = "fucktrak";
			string usernameEndsWithBadWord = "backfuck";
			string usernameContainsBadWord = "backfucktrak";
			string usernameOk = "BackTrakTest";
			string usernameStartsWithMixedCaseBadWord = "FucKtrak";

			using (CSSDataContext db = new CSSDataContext())
			{
				Identity newIdentity;
				
				Assert.AreEqual(System.Web.Security.MembershipCreateStatus.UserRejected, Identity.TryCreateIdentity(db, usernameStartsWithBadWord, PasswordHash.CreateHash("porkmuffins12"), "nick@chi-town.com", out newIdentity));
				Assert.AreEqual(System.Web.Security.MembershipCreateStatus.UserRejected, Identity.TryCreateIdentity(db, usernameEndsWithBadWord, PasswordHash.CreateHash("porkmuffins12"), "nick@chi-town.com", out newIdentity));
				Assert.AreEqual(System.Web.Security.MembershipCreateStatus.UserRejected, Identity.TryCreateIdentity(db, usernameContainsBadWord, PasswordHash.CreateHash("porkmuffins12"), "nick@chi-town.com", out newIdentity));
				Assert.AreEqual(System.Web.Security.MembershipCreateStatus.UserRejected, Identity.TryCreateIdentity(db, usernameStartsWithMixedCaseBadWord, PasswordHash.CreateHash("porkmuffins12"), "nick@chi-town.com", out newIdentity));
				Assert.AreEqual(System.Web.Security.MembershipCreateStatus.Success, Identity.TryCreateIdentity(db, usernameOk, PasswordHash.CreateHash("porkmuffins12"), "nick@chi-town.com", out newIdentity));

				db.SubmitChanges();
			}
		}

		[TestMethod()]
		public void TestVirtualMachineDetection()
		{
			Login login1;
			Login login2;
			Login login3;

			CreateTestData(out login1, out login2, out login3);

			MachineInformation machineInformation = new MachineInformation();

			machineInformation.MachineValues = new List<DeviceInfo>();

			using (CSSDataContext db = new CSSDataContext())
			{
				

				// Get all machine records for login 1.
				var machineRecords = db.MachineRecords.Where(p => p.LoginId == 1);
				foreach (var machineRecord in machineRecords)
				{
					machineInformation.MachineValues.Add(new DeviceInfo()
					{
						Name = machineRecord.MachineRecordType.Name,
						Type = (DeviceType)machineRecord.MachineRecordType.Id,
						Value = machineRecord.Identifier
					});
				}

				Assert.IsFalse(VirtualMachineMarker.IsMachineInformationFromAVirtualMachine(db, machineInformation, login1));

				machineInformation.MachineValues.Add(new DeviceInfo()
				{
					Name = "HardDisk",
					Type = DeviceType.HardDisk,
					Value = "3951160193|Virtual HD|0|Virtual HD"
				});

				Assert.IsTrue(VirtualMachineMarker.IsMachineInformationFromAVirtualMachine(db, machineInformation, login1));
			}
		}



		private void ClearDataForCallsign(string callsign)
		{
			using (var db = new CSSDataContext())
			{
				var login = Login.FindLoginByUsernameOrCallsign(db, callsign);

				if (login != null)
				{
					foreach (var associatedLogin in login.Identity.Logins)
					{
						db.Alias.DeleteAllOnSubmit(associatedLogin.Aliases);
						db.Login_UnlinkedLogins.DeleteAllOnSubmit(associatedLogin.Login_UnlinkedLogins);
						db.Bans.DeleteAllOnSubmit(associatedLogin.Bans);
						db.Bans.DeleteAllOnSubmit(associatedLogin.IssuedBans);
						db.PollVotes.DeleteAllOnSubmit(associatedLogin.PollVotes);
						db.MachineRecords.DeleteAllOnSubmit(associatedLogin.MachineRecords);
						db.Logins.DeleteOnSubmit(associatedLogin);
					}

					db.PollVotes.DeleteAllOnSubmit(login.PollVotes);
					db.Bans.DeleteAllOnSubmit(login.Bans);
					db.Bans.DeleteAllOnSubmit(login.IssuedBans);
					db.MachineRecords.DeleteAllOnSubmit(login.MachineRecords);
					db.Logins.DeleteAllOnSubmit(db.Logins.Where(p => p.Id == login.Id));
					db.Identities.DeleteOnSubmit(login.Identity);
					db.LogIPs.DeleteAllOnSubmit(login.Identity.LogIPs);

					db.SubmitChanges();
				}
			}
		}

		private void CreateTestData(out Login login1, out Login login2, out Login login3)
		{
			using (var db = new CSSDataContext())
			{
				ClearDataForCallsign("User1");
				ClearDataForCallsign("User2");
				ClearDataForCallsign("User3");
				//ClearDataForCallsign("User4");

				login1 = CreateUser("User1", "Password1", "email1@email.com", 10);
				login2 = CreateUser("User2", "Password2", "email2@email.com", 20);
				login3 = CreateUser("User3", "Password3", "email3@email.com", 13); // User1 and 3 should have some duplicate IPs
				//login4 = CreateUser("User4", "Password4", "email4@email.com");

				BanType banType = db.BanTypes.FirstOrDefault();

				if (banType == null)
				{
					var banClass = new BanClass()
					{
						Name = "Auto"
					};

					db.BanClasses.InsertOnSubmit(banClass);
					db.SubmitChanges();

					banType = new BanType()
					{
						//BanClass = banClass,
						BanClassId = banClass.Id,
						BaseTimeInMinutes = 30,
						Description = "Harassment / Threats",
						IsIncremental = true,
						RocNumber = 1
					};

					db.BanTypes.InsertOnSubmit(banType);
					db.SubmitChanges();
				}

				db.Bans.InsertOnSubmit(new Ban()
				{
					BannedByLoginId = 1,
					//BanType = banType,
					BanTypeId = banType.Id,
					DateCreated = DateTime.Now,
					DateExpires = DateTime.Now.AddDays(1),
					InEffect = true,
					Reason = "Pork Muffins!",
					LoginId = login2.Id
				});

				db.SubmitChanges();

				db.Bans.InsertOnSubmit(new Ban()
				{
					BannedByLoginId = 1,
					//BanType = banType,
					BanTypeId = banType.Id,
					DateCreated = DateTime.Now.AddDays(-30),
					DateExpires = DateTime.Now.AddDays(-29),
					InEffect = false,
					Reason = "Old ban.",
					LoginId = login2.Id,
				});

				db.SubmitChanges();

				MachineRecordType machineRecordType = db.MachineRecordTypes.FirstOrDefault();

				if (machineRecordType == null)
				{
					machineRecordType = new MachineRecordType()
					{
						Id = 1,
						Name = "Network"
					};

					db.MachineRecordTypes.InsertOnSubmit(machineRecordType);

					machineRecordType = new MachineRecordType()
					{
						Id = 2,
						Name = "HardDisk"
					};

					machineRecordType = new MachineRecordType()
					{
						Id = 3,
						Name = "EDID"
					};

					machineRecordType = new MachineRecordType()
					{
						Id = 4,
						Name = "Serial"
					};

					machineRecordType = new MachineRecordType()
					{
						Id = 5,
						Name = "Misc"
					};

					db.MachineRecordTypes.InsertOnSubmit(machineRecordType);

					db.SubmitChanges();
				}
			
				db.MachineRecords.InsertOnSubmit(new MachineRecord()
				{
					DeviceType = Allegiance.CommunitySecuritySystem.Common.Envelopes.AuthInfo.DeviceType.HardDisk,
					Identifier = "1234567890",
					LoginId = login3.Id,
					RecordTypeId = (int)Allegiance.CommunitySecuritySystem.Common.Envelopes.AuthInfo.DeviceType.HardDisk
				});

				db.SubmitChanges();

				db.MachineRecords.InsertOnSubmit(new MachineRecord()
				{
					DeviceType = Allegiance.CommunitySecuritySystem.Common.Envelopes.AuthInfo.DeviceType.Serial,
					Identifier = "ABCDEFGHIJKLMNOP",
					LoginId = login3.Id,
					RecordTypeId = (int)Allegiance.CommunitySecuritySystem.Common.Envelopes.AuthInfo.DeviceType.Serial
				});

				db.SubmitChanges();

				Poll poll = db.Polls.FirstOrDefault();
				PollOption pollOption1 = null;

				if (poll == null)
				{
					poll = new Poll()
					{
						DateCreated = DateTime.Now,
						DateExpires = DateTime.Now.AddDays(30),
						Question = "This is the question.",
						LastRecalculation = DateTime.Now
					};

					db.Polls.InsertOnSubmit(poll);
					db.SubmitChanges();

					pollOption1 = new PollOption()
					{
						Option = "Option 1",
						PollId = poll.Id,
						VoteCount = 0
					};

					db.PollOptions.InsertOnSubmit(pollOption1);
					db.SubmitChanges();
				}
				else
				{
					pollOption1 = db.PollOptions.First();
				}

				db.PollVotes.InsertOnSubmit(new PollVote()
				{
					LoginId = login3.Id,
					PollOptionId = pollOption1.Id
				});



				db.SubmitChanges();
			}
		}
	}
}
