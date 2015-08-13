using Allegiance.CommunitySecuritySystem.BlackboxGenerator;
using Allegiance.CommunitySecuritySystem.Common.Enumerations;
using Allegiance.CommunitySecuritySystem.DataAccess;
using Allegiance.CommunitySecuritySystem.DataAccess.Enumerations;
using Allegiance.CommunitySecuritySystem.Server;
using Allegiance.CommunitySecuritySystem.Server.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System;

namespace Allegiance.CommunitySecuritySystem.ServerTest
{
    /// <summary>
    /// Summary description for AuthenticationTests
    /// </summary>
    [TestClass]
    public class AuthenticationTest : BaseTest
    {
        #region Additional test attributes
        public AuthenticationTest()
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

        const string username = "Orion", password = "Test";

        [TestMethod]
        public void TestLogin()
        {
            var clientService = new ClientService();

            var result = clientService.Login(new LoginData()
            {
                Username    = username,
                Password    = password,
                Alias       = username,
                LobbyId     = 1 //Production
            });
            Assert.AreEqual(LoginStatus.Authenticated, result.Status);

            result = clientService.Login(new LoginData()
            {
                Username    = "Bogus",
                Password    = "Bogus",
                Alias       = "BogusAlias"
            });
            Assert.AreEqual(LoginStatus.InvalidCredentials, result.Status);

            result = clientService.Login(new LoginData()
            {
                Username    = username,
                Password    = password,
                Alias       = "NewAlias",
                LobbyId     = 1
            });
            Assert.AreEqual(LoginStatus.Authenticated, result.Status);

            result = clientService.Login(new LoginData()
            {
                Username    = username,
                Password    = password,
                Alias       = "Invalid Alias",
                LobbyId     = 1
            });
            Assert.AreEqual(LoginStatus.Authenticated, result.Status);
			Assert.AreEqual(username, result.AcceptedAlias, "Logging in with an invalid alias but a valid account will revert back to the oldest alias for that account.");
        }

        [TestMethod]
        public void TestAlias()
        {
            var clientService = new ClientService();

            //Create a different user
            CreateUser("Jimminy", "Cricket", "jcricket@wtf.com", 10);

            var result = clientService.CheckAlias(new LoginData()
            {
                Username = username,
                Password = password,
                Alias    = "NoobStick"
            });
            Assert.AreEqual(CheckAliasResult.Available, result);

            result = clientService.CheckAlias(new LoginData()
            {
                Username = username,
                Password = password,
                Alias    = "Jimminy"
            });
            Assert.AreEqual(CheckAliasResult.Unavailable, result);

            result = clientService.CheckAlias(new LoginData()
            {
                Username = username,
                Password = password,
                Alias    = username
            });
            Assert.AreEqual(CheckAliasResult.Registered, result);
        }

		[TestMethod]
		public void TestLegacyAlias()
		{
			var clientService = new ClientService();

			// acsstest1/acsstest12

			//Create a different user
			CreateUser("acsstest_mainaccount", "acsstest12", "jcricket@wtf.com", 10);

			// ASGS callsigns and passwords match; this user can have this alias.
			var result = clientService.CheckAlias(new LoginData()
			{
				Username = "acsstest_mainaccount",
				Password = "acsstest12",
				Alias = "acsstest1"
			});

			Assert.AreEqual(CheckAliasResult.Available, result);

			CreateUser("acsstest_mainacct2", "acsstest12_nope", "jcricket@wtf.com", 20);

			// ASGS callsigns match, invalid password; this user can't have this alias.
			result = clientService.CheckAlias(new LoginData()
			{
				Username = "acsstest_mainacct2",
				Password = "acsstest12_nope",
				Alias = "acsstest1"
			});

			Assert.AreEqual(CheckAliasResult.InvalidLegacyPassword, result);

			// ASGS callsign doesn't match, this user can have this alias.
			result = clientService.CheckAlias(new LoginData()
			{
				Username = username,
				Password = password,
				Alias = "acsstest1xx"
			});

			Assert.AreEqual(CheckAliasResult.Available, result);


			//result = clientService.CheckAlias(new LoginData()
			//{
			//    Username = username,
			//    Password = password,
			//    Alias = "aarmstrong"
			//});

			//Assert.AreEqual(CheckAliasResult.InvalidLegacyPassword, result);
		}

		[TestMethod]
		public void TestDefaultAlias()
		{
			var clientService = new ClientService();

			Login createdLogin = CreateUser("MultiAlias", "Alias", "mralias@wtf.com", 10);

			using(CSSDataContext db = new CSSDataContext())
			{
				var login = db.Logins.FirstOrDefault(p => p.Id == createdLogin.Id);
				login.Aliases.Add(new Alias()
				{
					Callsign = "alias1",
					IsActive = true,
					DateCreated = DateTime.Now
				});

				login.Aliases.Add(new Alias()
				{
					Callsign = "alias2",
					IsActive = true,
					DateCreated = DateTime.Now
				});

				login.Aliases.Add(new Alias()
				{
					Callsign = "alias3",
					IsActive = true,
					DateCreated = DateTime.Now
				});

				db.SubmitChanges();

				login = db.Logins.FirstOrDefault(p => p.Id == createdLogin.Id);

				Assert.AreEqual(4, login.Aliases.Count);
				Assert.IsTrue(login.Aliases.FirstOrDefault(p => p.Callsign == "MultiAlias").IsDefault);
				Assert.AreEqual(1, login.Aliases.Where(p => p.IsDefault == true).Count());

				var alias2 = login.Aliases.FirstOrDefault(p => p.Callsign == "alias2");

				// This should fail because the credentials are wrong.
				clientService.SetDefaultAlias(new SetDefaultAliasData()
				{
					AliasId = alias2.Id,
					Username = username,
					Password = password
				});

				Assert.IsFalse(login.Aliases.FirstOrDefault(p => p.Callsign == "alias2").IsDefault);
				Assert.AreEqual(1, login.Aliases.Where(p => p.IsDefault == true).Count());

				// This one should work.
				clientService.SetDefaultAlias(new SetDefaultAliasData()
				{
					AliasId = alias2.Id,
					Username = "MultiAlias",
					Password = "Alias"
				});

				Assert.IsFalse(login.Aliases.FirstOrDefault(p => p.Callsign == "alias2").IsDefault);
				Assert.AreEqual(1, login.Aliases.Where(p => p.IsDefault == true).Count());
			}
			
		}

        [TestMethod]
        public void TestRoles()
        {
            using (var db = new CSSDataContext())
            {
				string loginUserName;

				var result = Validation.ValidateLogin(username, password, out loginUserName, RoleType.Administrator);
                Assert.IsFalse(result);

                //Create role for this login
                var login = Login.FindLoginByUsernameOrCallsign(db, username);

                login.Login_Roles.Add(new Login_Role()
                {
                    RoleId = (int)RoleType.Administrator
                });
                db.SubmitChanges();

				result = Validation.ValidateLogin(username, password, out loginUserName, RoleType.Administrator);
                Assert.IsTrue(result);
            }
        }
    }
}