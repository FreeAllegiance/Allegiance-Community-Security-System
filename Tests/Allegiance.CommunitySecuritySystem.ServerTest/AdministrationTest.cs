using System;
using System.Linq;
using Allegiance.CommunitySecuritySystem.Common.Enumerations;
using Allegiance.CommunitySecuritySystem.DataAccess;
using Allegiance.CommunitySecuritySystem.DataAccess.Enumerations;
using Allegiance.CommunitySecuritySystem.Server;
using Allegiance.CommunitySecuritySystem.Server.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlTypes;

namespace Allegiance.CommunitySecuritySystem.ServerTest
{
    /// <summary>
    /// Summary description for AdministrationTest
    /// </summary>
    [TestClass]
    public class AdministrationTest : BaseTest
    {
        #region Additional test attributes
        public AdministrationTest()
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

        private static bool _initialized = false;

        private BanType Initialize()
        {
			DataAccess.BanType banType;
            using (var db = new CSSDataContext())
            {
				banType = db.BanTypes.FirstOrDefault(p => p.RocNumber == 1);

                if (_initialized)
					return banType;

				if (db.Logins.FirstOrDefault(p => p.Username == "Admin") == null)
				{
					//Create new user
					CreateUser("Admin", "Test", "NA", 10);

					var admin = Login.FindLoginByUsernameOrCallsign(db, "Admin");
					admin.Login_Roles.Add(new Login_Role() { RoleId = (int)RoleType.Administrator });
					admin.Login_Roles.Add(new Login_Role() { RoleId = (int)RoleType.SuperAdministrator });
				}

				db.Bans.DeleteAllOnSubmit(db.Bans);

				var banClass = new BanClass()
				{
					Id = 1,
					Name = "Minor"
				};

				db.BanClasses.DeleteAllOnSubmit(db.BanClasses);
				db.BanClasses.InsertOnSubmit(banClass);

				banType = new BanType()
				{
					BanClassId = 1,
					BaseTimeInMinutes = 30,
					Description = "Harassment / Threats",
					IsIncremental = true,
					RocNumber = 1
				};

				db.BanTypes.DeleteAllOnSubmit(db.BanTypes);
				db.BanTypes.InsertOnSubmit(banType);

                db.SubmitChanges();
            }

            _initialized = true;

            return banType;
        }

		[TestMethod]
		public void TestMinorBanDurationCalculations()
		{
			Initialize();

			DataAccess.BanClass minorBanClass = new BanClass()
			{
				Id = (int)BanClassType.Minor,
				Name = "Minor"
			};

			DataAccess.BanType minorBanType = new BanType()
			{
				BanClass = minorBanClass,
				BanClassId = minorBanClass.Id,
				BaseTimeInMinutes = 30,
				Description = "Minor 30 minute ban.",
				IsIncremental = true,
				RocNumber = 1
			};

			DataAccess.Login testUser = CreateUser(Guid.NewGuid().ToString().Substring(0, 20), "Test", "NA", 10);
			DataAccess.Identity identity = testUser.Identity;

			// Test 1x Minor Ban - 30 minutes
			TimeSpan? duration = Ban.CalculateDuration(identity, minorBanType);
			Assert.AreEqual(30, duration.Value.TotalMinutes);

			// Test 2x Minor Ban - 15 hours
			testUser.Bans.Add(CreateBan(testUser, minorBanType));
			duration = Ban.CalculateDuration(identity, minorBanType);
			Assert.AreEqual(900, duration.Value.TotalMinutes);

			// Test 3x Minor Ban - 5 days
			testUser.Bans.Add(CreateBan(testUser, minorBanType));
			duration = Ban.CalculateDuration(identity, minorBanType);
			Assert.AreEqual(7200, duration.Value.TotalMinutes);

			// Test 4x Minor Ban - 5 days
			testUser.Bans.Add(CreateBan(testUser, minorBanType));
			duration = Ban.CalculateDuration(identity, minorBanType);
			Assert.AreEqual(7200, duration.Value.TotalMinutes);

			// Test 5x Minor Ban - 5 days
			testUser.Bans.Add(CreateBan(testUser, minorBanType));
			duration = Ban.CalculateDuration(identity, minorBanType);
			Assert.AreEqual(7200, duration.Value.TotalMinutes);

			// Test 6x Minor Ban - 10 days
			testUser.Bans.Add(CreateBan(testUser, minorBanType));
			duration = Ban.CalculateDuration(identity, minorBanType);
			Assert.AreEqual(1440 * 10, duration.Value.TotalMinutes);

			// TODO: re-add when proper logic is in place.
			//// Test 7x Minor Ban - 30 days
			//identity.Bans.Add(CreateBan(testUser, minorBanType));
			//duration = Ban.CalculateDuration(identity, minorBanType);
			//Assert.AreEqual(1440 * 30, duration.Value.TotalMinutes);

			//// Test 8x Minor Ban - 90 days
			//identity.Bans.Add(CreateBan(testUser, minorBanType));
			//duration = Ban.CalculateDuration(identity, minorBanType);
			//Assert.AreEqual(1440 * 90, duration.Value.TotalMinutes);

			//// Test 9x Minor Ban - 90 days
			//identity.Bans.Add(CreateBan(testUser, minorBanType));
			//duration = Ban.CalculateDuration(identity, minorBanType);
			//Assert.AreEqual(1440 * 90, duration.Value.TotalMinutes);

			// test rolling window for minor bans.
			testUser.Bans.Clear();
			for(int i = 0; i < 5; i++)
				testUser.Bans.Add(CreateBan(testUser, minorBanType));

			Assert.AreEqual(5, identity.Bans.Count());

			// Test rolling window -- 4 recent bans == 5 days
			testUser.Bans[0].DateCreated = DateTime.Now.AddDays(-91);
			duration = Ban.CalculateDuration(identity, minorBanType);
			Assert.AreEqual(7200, duration.Value.TotalMinutes);

			// Test rolling window -- 3 recent bans == 5 days
			testUser.Bans[1].DateCreated = DateTime.Now.AddDays(-91);
			duration = Ban.CalculateDuration(identity, minorBanType);
			Assert.AreEqual(7200, duration.Value.TotalMinutes);

			// Test rolling window -- 2 recent bans == 5 days
			testUser.Bans[2].DateCreated = DateTime.Now.AddDays(-91);
			duration = Ban.CalculateDuration(identity, minorBanType);
			Assert.AreEqual(7200, duration.Value.TotalMinutes);

			// Test rolling window -- 1 recent ban == 15 hours
			testUser.Bans[3].DateCreated = DateTime.Now.AddDays(-91);
			duration = Ban.CalculateDuration(identity, minorBanType);
			Assert.AreEqual(900, duration.Value.TotalMinutes);

			// Test rolling window -- 0 recent bans == 30 minutes
			testUser.Bans[4].DateCreated = DateTime.Now.AddDays(-91);
			duration = Ban.CalculateDuration(testUser.Identity, minorBanType);
			Assert.AreEqual(30, duration.Value.TotalMinutes);	
		}


		[TestMethod]
		public void TestMajorBanDurationCalculations()
		{
			Initialize();

			DataAccess.BanClass banClass = new BanClass()
			{
				Id = (int)BanClassType.Major,
				Name = "Major"
			};

			DataAccess.BanType banType = new BanType()
			{
				BanClass = banClass,
				BanClassId = banClass.Id,
				BaseTimeInMinutes = 30,
				Description = "Major 30 minute ban.",
				IsIncremental = true,
				SrNumber = 4
			};

			DataAccess.Login testUser = CreateUser(Guid.NewGuid().ToString().Substring(0, 20), "Test", "NA", 10);
			DataAccess.Identity identity = testUser.Identity;

			// Test 1x Ban - 30 minutes
			TimeSpan? duration = Ban.CalculateDuration(identity, banType);
			Assert.AreEqual(30, duration.Value.TotalMinutes);

			// Test 2x Ban - 120 minutes
			testUser.Bans.Add(CreateBan(testUser, banType));
			duration = Ban.CalculateDuration(identity, banType);
			Assert.AreEqual(120, duration.Value.TotalMinutes);

			// Test 3x Ban - 600 minutes
			testUser.Bans.Add(CreateBan(testUser, banType));
			duration = Ban.CalculateDuration(identity, banType);
			Assert.AreEqual(600, duration.Value.TotalMinutes);

			// Test 4x Minor Ban - 30 days
			testUser.Bans.Add(CreateBan(testUser, banType));
			duration = Ban.CalculateDuration(identity, banType);
			Assert.AreEqual(1440 * 30, duration.Value.TotalMinutes);

			/* TODO: re-add when proper logic is in place.
			// Test 5x Ban - 60 days
			identity.Bans.Add(CreateBan(testUser, banType));
			duration = Ban.CalculateDuration(identity, banType);
			Assert.AreEqual(1440 * 60, duration.Value.TotalMinutes);

			// Test 6x Ban - Permanent
			identity.Bans.Add(CreateBan(testUser, banType));
			duration = Ban.CalculateDuration(identity, banType);
			Assert.AreEqual(1440 * 10, duration.Value.TotalMinutes);
			*/ 

			// test rolling window for major bans.
			testUser.Bans.Clear();
			for (int i = 0; i < 3; i++)
				testUser.Bans.Add(CreateBan(testUser, banType));

			duration = Ban.CalculateDuration(identity, banType);
			Assert.AreEqual(1440 * 30, duration.Value.TotalMinutes);

			// Test rolling window -- 4 recent bans 
			testUser.Bans[0].DateCreated = DateTime.Now.AddDays(-180);
			duration = Ban.CalculateDuration(identity, banType);
			Assert.AreEqual(600, duration.Value.TotalMinutes);

			// Test rolling window -- 3 recent bans
			testUser.Bans[1].DateCreated = DateTime.Now.AddDays(-180);
			duration = Ban.CalculateDuration(identity, banType);
			Assert.AreEqual(120, duration.Value.TotalMinutes);

			// Test rolling window -- 2 recent bans
			testUser.Bans[2].DateCreated = DateTime.Now.AddDays(-180);
			duration = Ban.CalculateDuration(identity, banType);
			Assert.AreEqual(30, duration.Value.TotalMinutes);
		}

		[TestMethod]
		public void TestPermanentBanCalculations()
		{
			Initialize();

			DataAccess.BanClass banClass = new BanClass()
			{
				Id = (int)BanClassType.Major,
				Name = "Major"
			};

			DataAccess.BanType banType = new BanType()
			{
				BanClass = banClass,
				BanClassId = banClass.Id,
				BaseTimeInMinutes = 30,
				Description = "Permanent ban after one infraction.",
				IsIncremental = true,
				InfractionsBeforePermanentBan = 1,
				SrNumber = 13
			};

			DataAccess.Login testUser = CreateUser(Guid.NewGuid().ToString().Substring(0, 20), "Test", "NA", 10);
			DataAccess.Identity identity = testUser.Identity;

			// Test 1x Ban - 30 minutes
			TimeSpan? duration = Ban.CalculateDuration(identity, banType);
			Assert.AreEqual(30, duration.Value.TotalMinutes);

			// Test 2x Ban - Permanent.
			testUser.Bans.Add(CreateBan(testUser, banType));
			duration = Ban.CalculateDuration(identity, banType);
			Assert.AreEqual(TimeSpan.MaxValue, duration.Value);

			// Test permanent ban on first infraction
			banType = new BanType()
			{
				BanClass = banClass,
				BanClassId = banClass.Id,
				BaseTimeInMinutes = 30,
				Description = "Permanent ban on first infraction.",
				IsIncremental = true,
				InfractionsBeforePermanentBan = 0,
				SrNumber = 9
			};

			testUser = CreateUser(Guid.NewGuid().ToString().Substring(0, 20), "Test", "NA", 10);
			identity = testUser.Identity;

			// Test 1x Ban - Permanent.
			testUser.Bans.Add(CreateBan(testUser, banType));
			duration = Ban.CalculateDuration(identity, banType);
			Assert.AreEqual(TimeSpan.MaxValue, duration.Value);
		}


		private Ban CreateBan(Login user, BanType banType)
		{
			TimeSpan? duration = Ban.CalculateDuration(user.Identity, banType);
			DateTime expirationDate;

			if (duration == TimeSpan.MaxValue)
				expirationDate = SqlDateTime.MaxValue.Value;
			else
				expirationDate = DateTime.Now.Add(duration.Value);

			return new Ban()
			{
				BannedByLoginId = user.Id,
				BanType = banType,
				BanTypeId = banType.Id,
				DateCreated = DateTime.Now,
				DateExpires = expirationDate,
				InEffect = true,
				Login = user
			};
		}

        [TestMethod]
        public void TestBan()
        {
            DataAccess.BanType banType = Initialize();

            //Test Set, ListBans, Remove, ensure user has requisite permissions.
            var adminService = new Administration();
            var result = adminService.SetBan(new BanData()
            {
				BanMode				= BanMode.Auto,
				BanTypeId			= banType.Id,
                Username            = "Admin",
                Password            = "Test",
                Reason              = "#1 Orion is being a general dick.",
                Alias               = "Orion",
            });

            Assert.IsTrue(result);

            //Ensure banned user can no longer log in
            var clientService = new ClientService();
            var loginResult = clientService.Login(new LoginData()
            {
                Username    = "Orion",
                Password    = "Test",
                Alias       = "Orion"
            });
            Assert.AreEqual(LoginStatus.AccountLocked, loginResult.Status);

            //List bans
            var bans = adminService.ListBans(new AuthenticatedData()
            {
                Username = "Admin",
                Password = "Test"
            }, "Orion");

            //Remove all bans
            var ban = bans.FirstOrDefault();
            adminService.RemoveBan(new BanData()
            {
                Username    = "Admin",
                Password    = "Test",
                BanId       = ban.Id
            });

			bans = adminService.ListBans(new AuthenticatedData()
			{
				Username = "Admin",
				Password = "Test"
			}, "Orion");

			Assert.AreEqual(0, bans.Where(p => p.InEffect == true).Count());

            //Ensure user can log in.
            clientService = new ClientService();
            loginResult = clientService.Login(new LoginData()
            {
                Username    = "Orion",
                Password    = "Test",
                Alias       = "Orion",
                LobbyId     = 1 
            });
            Assert.AreEqual(LoginStatus.Authenticated, loginResult.Status);

            //Set two sequential bans.
            result = adminService.SetBan(new BanData()
            {
                BanMode             = BanMode.Auto,
                Username            = "Admin",
                Password            = "Test",
                BanTypeId			= banType.Id,
                Reason              = "#2 Orion is being a general dick.",
                Alias               = "Orion",
            });
            Assert.IsTrue(result);
            
            result = adminService.SetBan(new BanData()
            {
                BanMode             = BanMode.Auto,
                Username            = "Admin",
                Password            = "Test",
                BanTypeId			= banType.Id,
                Reason              = "#3 Orion is /STILL/ being a general dick.",
                Alias               = "Orion",
            });
            Assert.IsTrue(result);

            //Retrieve ban
            bans = adminService.ListBans(new AuthenticatedData()
            {
                Username = "Admin",
                Password = "Test"
            }, "Orion");

			Assert.AreEqual(2, bans.Where(p => p.InEffect == true).Count());

            var firstBan	= bans.Where(p => p.InEffect == true).OrderBy(p => p.DateCreated).First();
			var lastBan		= bans.Where(p => p.InEffect == true).OrderBy(p => p.DateCreated).Last();

            var firstDuration   = (firstBan.DateExpires.Value - firstBan.DateCreated).TotalMinutes;
            var lastDuration    = (lastBan.DateExpires.Value - lastBan.DateCreated).TotalMinutes;

            Assert.IsTrue(lastDuration > firstDuration);

            //Reset ban length
            adminService.SetBan(new BanData()
            {
                Username    = "Admin",
                Password    = "Test",
                BanId       = firstBan.Id,
                BanMode     = BanMode.Custom,
                Duration    = TimeSpan.MinValue,
				Alias		= "Orion"
            });

            adminService.SetBan(new BanData()
            {
                Username    = "Admin",
                Password    = "Test",
                BanId       = lastBan.Id,
                BanMode     = BanMode.Custom,
                Duration    = TimeSpan.MinValue,
				Alias		= "Orion"
            });
            
            bans = adminService.ListBans(new AuthenticatedData()
            {
                Username = "Admin",
                Password = "Test"
            }, "Orion");

			Assert.AreEqual(2, bans.Where(p => p.InEffect == true).Count());

			Assert.AreEqual(0, bans.Where(p => p.InEffect == true && p.DateExpires > DateTime.Now).Count());
        }

        [TestMethod]
        public void TestListAliases()
        {
            Initialize();

            var adminService = new Administration();
            var results = adminService.ListAliases(new AuthenticatedData()
            {
                Username = "Admin",
                Password = "Test"
            }, "Admin");
            Assert.AreEqual(1, results.Count);

            // Try to log in with a non-existant alias.
            var clientService = new ClientService();
            var loginResult = clientService.Login(new LoginData()
            {
                Username = "Admin",
                Password = "Test",
                Alias    = "Jerky",
                LobbyId  = 1
            });
            Assert.AreEqual(LoginStatus.Authenticated, loginResult.Status);
			Assert.AreEqual("Admin", loginResult.AcceptedAlias, "Logging in with an invalid alias reverts back to the oldest available alias.");

            results = adminService.ListAliases(new AuthenticatedData()
            {
                Username = "Admin",
                Password = "Test"
            }, "Admin");
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestListInfractionTypes()
        {
            Initialize();

            var adminService = new Administration();
            var result = adminService.ListInfractionTypes(new AuthenticatedData()
            {
                Username = "Admin",
                Password = "Test",
            });

			Assert.AreEqual(1, result.Count);
        }
    }
}
