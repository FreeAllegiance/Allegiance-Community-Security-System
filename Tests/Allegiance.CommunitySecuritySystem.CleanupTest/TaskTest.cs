using Allegiance.CommunitySecuritySystem.Cleanup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Allegiance.CommunitySecuritySystem.ServerTest;
using Allegiance.CommunitySecuritySystem.DataAccess;
using System.Linq;
using Allegiance.CommunitySecuritySystem.Common.Envelopes.AuthInfo;

namespace Allegiance.CommunitySecuritySystem.CleanupTest
{
    
    
    /// <summary>
    ///This is a test class for TaskTest and is intended
    ///to contain all TaskTest Unit Tests
    ///</summary>
	[TestClass()]
	public class TaskTest : PollingTest
	{
		
		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		////
		////Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
			
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion

		/// <summary>
		///A test for CleanupPolls
		///</summary>
		[TestMethod()]
		[DeploymentItem("Allegiance.CommunitySecuritySystem.Cleanup.dll")]
		public void CleanupPollsTest()
		{
			base.Initialize();

			var logins = new string[,] { { "One", "1" }, { "Two", "2" }, { "Three", "3" }, { "Four", "4" } };

			using (CSSDataContext db = new CSSDataContext())
			{
				var login1 = Login.FindLoginByUsernameOrCallsign(db, logins[0, 0]);
				var login2 = Login.FindLoginByUsernameOrCallsign(db, logins[1, 0]);
				var login3 = Login.FindLoginByUsernameOrCallsign(db, logins[2, 0]);
				var login4 = Login.FindLoginByUsernameOrCallsign(db, logins[3, 0]);

				var targetPollAOption = db.Polls.FirstOrDefault(p => p.Question == "Test Poll A").PollOptions.FirstOrDefault();
                var targetPollBOption = db.Polls.FirstOrDefault(p => p.Question == "Test Poll B").PollOptions.FirstOrDefault();

                Assert.IsNotNull(targetPollAOption);
                Assert.IsNotNull(targetPollBOption);
                Assert.AreNotEqual(targetPollAOption.Id, targetPollBOption.Id);

				login1.MachineRecords.Add(new MachineRecord()
				{
					DeviceType = DeviceType.HardDisk,
					Identifier = "1234HARDDISK!",
					Login = login1
				});

				login2.MachineRecords.Add(new MachineRecord()
				{
					DeviceType = DeviceType.HardDisk,
					Identifier = "1234HARDDISK!",
					Login = login2
				});

				login1.PollVotes.Add(new PollVote()
				{
					Login = login1,
					PollOption = targetPollAOption
				});

				login2.PollVotes.Add(new PollVote()
				{
					Login = login2,
					PollOption = targetPollAOption
				});

				login3.PollVotes.Add(new PollVote()
				{
					Login = login3,
					PollOption = targetPollAOption
				});

                login3.PollVotes.Add(new PollVote()
                {
                    Login = login3,
                    PollOption = targetPollBOption
                });

				login4.PollVotes.Add(new PollVote()
				{
					Login = login4,
					PollOption = targetPollAOption
				});

                login4.PollVotes.Add(new PollVote()
                {
                    Login = login4,
                    PollOption = targetPollBOption
                });

				db.SubmitChanges();

				Identity principal;
				bool wasMerged;
				Identity.MatchIdentity(db, login1, new MachineInformation() { MachineValues = new System.Collections.Generic.List<DeviceInfo>(), Token = String.Empty }, out principal, out wasMerged);

				Assert.IsTrue(wasMerged);

				db.SubmitChanges();

				Assert.AreEqual(6, db.PollVotes.Count());

				Task_Accessor.CleanupPolls();

				db.SubmitChanges();

				Assert.AreEqual(5, db.PollVotes.Count());
			}
		}

		///// <summary>
		/////A test for CleanupBlackboxes
		/////</summary>
		//[TestMethod()]
		//[DeploymentItem("Allegiance.CommunitySecuritySystem.Cleanup.dll")]
		//public void CleanupBlackboxesTest()
		//{
		//    Task_Accessor.CleanupBlackboxes();
		//    Assert.Inconclusive("A method that does not return a value cannot be verified.");
		//}

		///// <summary>
		/////A test for CleanupCaptchas
		/////</summary>
		//[TestMethod()]
		//[DeploymentItem("Allegiance.CommunitySecuritySystem.Cleanup.dll")]
		//public void CleanupCaptchasTest()
		//{
		//    Task_Accessor.CleanupCaptchas();
		//    Assert.Inconclusive("A method that does not return a value cannot be verified.");
		//}

		

		///// <summary>
		/////A test for CleanupSessions
		/////</summary>
		//[TestMethod()]
		//[DeploymentItem("Allegiance.CommunitySecuritySystem.Cleanup.dll")]
		//public void CleanupSessionsTest()
		//{
		//    Task_Accessor.CleanupSessions();
		//    Assert.Inconclusive("A method that does not return a value cannot be verified.");
		//}

		///// <summary>
		/////A test for Execute
		/////</summary>
		//[TestMethod()]
		//public void ExecuteTest()
		//{
		//    Task.Execute();
		//    Assert.Inconclusive("A method that does not return a value cannot be verified.");
		//}

		///// <summary>
		/////A test for Execute
		/////</summary>
		//[TestMethod()]
		//public void ExecuteTest1()
		//{
		//    bool cleanupSessions = false; // TODO: Initialize to an appropriate value
		//    Task.Execute(cleanupSessions);
		//    Assert.Inconclusive("A method that does not return a value cannot be verified.");
		//}

		///// <summary>
		/////A test for Execute
		/////</summary>
		//[TestMethod()]
		//public void ExecuteTest2()
		//{
		//    bool cleanupSessions = false; // TODO: Initialize to an appropriate value
		//    bool cleanupKeys = false; // TODO: Initialize to an appropriate value
		//    Task.Execute(cleanupSessions, cleanupKeys);
		//    Assert.Inconclusive("A method that does not return a value cannot be verified.");
		//}

		///// <summary>
		/////A test for Execute
		/////</summary>
		//[TestMethod()]
		//public void ExecuteTest3()
		//{
		//    bool cleanupSessions = false; // TODO: Initialize to an appropriate value
		//    bool cleanupKeys = false; // TODO: Initialize to an appropriate value
		//    bool cleanupPolls = false; // TODO: Initialize to an appropriate value
		//    bool cleanupCaptchas = false; // TODO: Initialize to an appropriate value
		//    bool recalculatePolls = false; // TODO: Initialize to an appropriate value
		//    Task.Execute(cleanupSessions, cleanupKeys, cleanupPolls, cleanupCaptchas, recalculatePolls);
		//    Assert.Inconclusive("A method that does not return a value cannot be verified.");
		//}

		///// <summary>
		/////A test for RecalculatePolls
		/////</summary>
		//[TestMethod()]
		//[DeploymentItem("Allegiance.CommunitySecuritySystem.Cleanup.dll")]
		//public void RecalculatePollsTest()
		//{
		//    Task_Accessor.RecalculatePolls();
		//    Assert.Inconclusive("A method that does not return a value cannot be verified.");
		//}
	}
}
