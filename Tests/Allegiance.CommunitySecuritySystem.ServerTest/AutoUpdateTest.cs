using System;
using System.Linq;
using Allegiance.CommunitySecuritySystem.DataAccess;
using Allegiance.CommunitySecuritySystem.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Allegiance.CommunitySecuritySystem.ServerTest
{
    /// <summary>
    /// Summary description for AutoUpdateTest
    /// </summary>
    [TestClass]
    public class AutoUpdateTest : BaseTest
    {
        #region Additional test attributes
        public AutoUpdateTest()
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

        [TestMethod]
        public void TestCheckForUpdates()
        {
            int lobbyId = 0;
            using (var db = new CSSDataContext())
            {
                //Create autoupdate files
                var file = new AutoUpdateFile() { Filename = "mdl", IsProtected = true };
                db.AutoUpdateFiles.InsertOnSubmit(file);

                var lobby   = db.Lobbies.FirstOrDefault();
                lobbyId     = lobby.Id;

                file.AutoUpdateFile_Lobbies.Add(new AutoUpdateFile_Lobby()
                {
                    CurrentVersion  = "1.0",
                    DateCreated     = DateTime.Now,
                    DateModified    = DateTime.Now,
                    Lobby           = lobby,
                    ValidChecksum   = "VALID"
                });

                db.SubmitChanges();
            }

            var clientService   = new ClientService();
            var files           = clientService.CheckForUpdates(lobbyId);
            var fileResult      = files.Files.FirstOrDefault();

            Assert.AreEqual(1, files.Files.Count);
            Assert.IsTrue(fileResult.IsProtected);
            Assert.AreEqual("mdl", fileResult.Filename);
            Assert.AreEqual("VALID", fileResult.ValidChecksum.Trim());
            Assert.AreEqual("1.0", fileResult.CurrentVersion);
            Assert.AreEqual(lobbyId, fileResult.LobbyId);
        }
    }
}