using System;
using System.Linq;
using System.Text;
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
    /// Summary description for PollingTest
    /// </summary>
    [TestClass]
    public class PollingTest : BaseTest
    {
        public PollingTest()
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

        protected void Initialize()
        {
			using (DataAccess.CSSDataContext db = new CSSDataContext())
			{
				db.PollVotes.DeleteAllOnSubmit(db.PollVotes);
				db.PollOptions.DeleteAllOnSubmit(db.PollOptions);
				db.Polls.DeleteAllOnSubmit(db.Polls);
				db.SubmitChanges();
			}

            CreateUser("One", "1", "1@1.com", 10);
            CreateUser("Two", "2", "2@2.com", 20);
            CreateUser("Three", "3", "3@3.com", 30);
            CreateUser("Four", "4", "4@4.com", 40);

			string[] options = { "Option1", "Option2", "Option3" };
            Poll.NewPoll(new CSSDataContext(), "Test Poll A", options, DateTime.Now.AddYears(1));

            Poll.NewPoll(new CSSDataContext(), "Test Poll B", options, DateTime.Now.AddYears(1));
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
        public void TestPolls()
        {
            Initialize();

            var logins = new string[,] {{"One","1"},{"Two","2"},{"Three","3"},{"Four","4"}};

            for (int i = 0; i <= logins.GetUpperBound(0); i++)
            {
                var messaging = new ClientService();
                var polls = messaging.ListPolls(new AuthenticatedData()
                {
                    Username = logins[i,0],
                    Password = logins[i,1]
                });
                Assert.AreEqual(2, polls.Count);

                PollData vote = new PollData()
                {
                    //Each user votes for their option %3 (result being 1st option has 2 votes, other 2 have 1 vote)
                    OptionId = polls[0].PollOptions[(Convert.ToInt16(logins[i,1])-1)%3].Id,
                    Username = logins[i,0],
                    Password = logins[i,1]
                };
                messaging.ApplyVote(vote);
                messaging.ApplyVote(vote); //Send the vote again, prevent anyone from voting more than once.
            }

            using (var db = new CSSDataContext())
            {
                Assert.AreEqual(2, db.PollVotes.Count(pv => pv.PollOption.Option == "Option1"));
				Assert.AreEqual(1, db.PollVotes.Count(pv => pv.PollOption.Option == "Option2"));
            }
        }

		[TestMethod]
		public void TestPollRecalculation()
		{
			Initialize();

			using (var db = new CSSDataContext())
			{
				var userOne = db.Logins.FirstOrDefault(p => p.Username == "One");
				var userTwo = db.Logins.FirstOrDefault(p => p.Username == "Two");
				var userThree = db.Logins.FirstOrDefault(p => p.Username == "Three");
				var userFour = db.Logins.FirstOrDefault(p => p.Username == "Four");

				// Link two logins.
				userTwo.Identity = userOne.Identity;

				var poll = db.Polls.FirstOrDefault(p => p.Question == "Test Poll");
				var pollOption1 = poll.PollOptions.FirstOrDefault(p => p.Option == "Option1");
				var pollOption2 = poll.PollOptions.FirstOrDefault(p => p.Option == "Option2");
				var pollOption3 = poll.PollOptions.FirstOrDefault(p => p.Option == "Option3");

				// User two's vote shouldn't count because it's linked to user one.
				pollOption1.PollVotes.Add(new PollVote() { Login = userOne });
				pollOption2.PollVotes.Add(new PollVote() { Login = userTwo });
				pollOption3.PollVotes.Add(new PollVote() { Login = userThree });
				pollOption3.PollVotes.Add(new PollVote() { Login = userFour });

				db.SubmitChanges();
			}

			Poll.RecalculateAllPolls();

			using (var db = new CSSDataContext())
			{
				var poll = db.Polls.FirstOrDefault(p => p.Question == "Test Poll");
				var pollOption1 = poll.PollOptions.FirstOrDefault(p => p.Option == "Option1");
				var pollOption2 = poll.PollOptions.FirstOrDefault(p => p.Option == "Option2");
				var pollOption3 = poll.PollOptions.FirstOrDefault(p => p.Option == "Option3");

				Assert.AreEqual(1, pollOption1.VoteCount);
				Assert.AreEqual(0, pollOption2.VoteCount, "User two's vote shouldn't count because it's linked to user one.");
				Assert.AreEqual(2, pollOption3.VoteCount);
			}
		}
    }
}
