using System;
using System.Linq;
using System.Collections.Generic;
using Allegiance.CommunitySecuritySystem.BlackboxGenerator;
using Allegiance.CommunitySecuritySystem.Common.Enumerations;
using Allegiance.CommunitySecuritySystem.DataAccess;
using Allegiance.CommunitySecuritySystem.DataAccess.Enumerations;
using Allegiance.CommunitySecuritySystem.Server;
using Allegiance.CommunitySecuritySystem.Server.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Allegiance.CommunitySecuritySystem.DataAccess.Model;

namespace Allegiance.CommunitySecuritySystem.ServerTest
{
    /// <summary>
    /// Summary description for MessagingTest
    /// </summary>
    [TestClass]
    public class MessagingTest : BaseTest
    {
        #region Additional test attributes
        public MessagingTest()
        {
            
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

        private void Initialize()
        {
			var sender = CreateUser("TestMessageSender", "porkmuffins", "nick@chi-town.com", 10);
            var firstUser   = CreateUser("One", "1", "1@1.com", 20);
            var secondUser  = CreateUser("Two", "2", "2@2.com", 30);
            var thirdUser   = CreateUser("Three", "3", "3@3.com", 40);
            var fourthUser  = CreateUser("Four", "4", "4@4.com", 50);

            using (var db = new CSSDataContext())
            {
                Group.CreateGroup(db, "GroupA", false, "@GA");
                Group.CreateGroup(db, "GroupB", false, "@GB");

                var role = new GroupRole()
                {
                    Name = "Member",
                    Token = null
                };

                db.GroupRoles.InsertOnSubmit(role);
                db.SubmitChanges();

                Group.AddAlias(db, "GroupA", "One");
                Group.AddAlias(db, "GroupA", "Two");
                Group.AddAlias(db, "GroupB", "Three");
                Group.AddAlias(db, "GroupB", "Four");
				db.SubmitChanges();

                GroupMessage.NewMessage(db, "SUBJECT", "THIS MESSAGE IS FOR GROUP A WHICH CONSISTS OF \"One\" AND \"Two\"",
                                        "GroupA", DateTime.Now, sender.Aliases.FirstOrDefault());
                GroupMessage.NewMessage(db, "SUBJECT", "THIS MESSAGE IS FOR GROUP A WHICH CONSISTS OF \"Three\" AND \"Four\"",
										"GroupB", DateTime.Now, sender.Aliases.FirstOrDefault());

				GroupMessage.NewMessage(db, "Global Message", "This message should go to all users.", null, DateTime.Now, sender.Aliases.FirstOrDefault());

				db.SubmitChanges();

                PersonalMessage.NewMessage(db, "SUBJECT", "THIS MESSAGE IS FOR \"One\"",
                                            firstUser.Aliases.First(), Login.FindLoginByUsernameOrCallsign(db, "One"), DateTime.Now);

				db.SubmitChanges();

                PersonalMessage.NewMessage(db, "SUBJECT", "THIS MESSAGE IS FOR \"Two\"",
											firstUser.Aliases.First(), Login.FindLoginByUsernameOrCallsign(db, "Two"), DateTime.Now);
                PersonalMessage.NewMessage(db, "SUBJECT", "THIS MESSAGE IS FOR \"Three\"",
											firstUser.Aliases.First(), Login.FindLoginByUsernameOrCallsign(db, "Three"), DateTime.Now);
                PersonalMessage.NewMessage(db, "SUBJECT", "THIS MESSAGE IS FOR \"Four\"",
											firstUser.Aliases.First(), Login.FindLoginByUsernameOrCallsign(db, "Four"), DateTime.Now);

				db.SubmitChanges();
            }
        }

        [TestMethod]
        public void TestRetrieveMessages()
        {
            Initialize();

            var logins = new string[,] { { "One", "1" }, { "Two", "2" }, { "Three", "3" }, { "Four", "4" } };
            
            //Try for each login to check that group messaging works as intended
            for (int i = 0; i <= logins.GetUpperBound(0); i++)
            {
                var service = new ClientService();

                ListMessageResult messagesFull = service.ListMessages(new AuthenticatedData()
                {
                    Username = logins[i, 0],
                    Password = logins[i, 1]
                });

                Assert.AreEqual(3, messagesFull.Messages.Count);

                //Check again, should be no new messages 
				ListMessageResult messagesEmpty = service.ListMessages(new AuthenticatedData()
                {
                    Username = logins[i,0],
                    Password = logins[i,1]
                });

                Assert.AreEqual(0, messagesEmpty.Messages.Count);
            }
        }
    }
}