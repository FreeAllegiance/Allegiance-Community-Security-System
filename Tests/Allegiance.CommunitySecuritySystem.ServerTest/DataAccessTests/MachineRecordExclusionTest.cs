using Allegiance.CommunitySecuritySystem.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Allegiance.CommunitySecuritySystem.ServerTest.DataAccessTests
{
    
    
    /// <summary>
    ///This is a test class for MachineRecordExclusionTest and is intended
    ///to contain all MachineRecordExclusionTest Unit Tests
    ///</summary>
	[TestClass()]
	public class MachineRecordExclusionTest : BaseTest
	{


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
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
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
		///A test for IsMachineRecordExcluded
		///</summary>
		[TestMethod()]
		public void IsMachineRecordExcludedTest()
		{
			using (var db = new CSSDataContext())
			{
				MachineRecord machineRecord1 = new MachineRecord()
				{
					Identifier = "Volume0",
					RecordTypeId = 2,
					Login = db.Logins.FirstOrDefault()
				};

				MachineRecord machineRecord2 = new MachineRecord()
				{
					Identifier = "Extra_Volume1",
					RecordTypeId = 2,
					Login = db.Logins.FirstOrDefault()
				};

				MachineRecord machineRecord3 = new MachineRecord()
				{
					Identifier = "Volume2_Extra",
					RecordTypeId = 2,
					Login = db.Logins.FirstOrDefault()
				};

				MachineRecord machineRecord4 = new MachineRecord()
				{
					Identifier = "Extra_Volume3_Extra",
					RecordTypeId = 2,
					Login = db.Logins.FirstOrDefault()
				};

				MachineRecord machineRecord5 = new MachineRecord()
				{
					Identifier = "NoMatch",
					RecordTypeId = 2,
					Login = db.Logins.FirstOrDefault()
				};

				MachineRecord machineRecord6 = new MachineRecord()
				{
					Identifier = "Volume1_NoMatch",
					RecordTypeId = 2,
					Login = db.Logins.FirstOrDefault()
				};

				MachineRecord machineRecord7 = new MachineRecord()
				{
					Identifier = "NoMatch_Volume2",
					RecordTypeId = 2,
					Login = db.Logins.FirstOrDefault()
				};

				MachineRecord machineRecord8 = new MachineRecord()
				{
					Identifier = "Volume0",
					RecordTypeId = 1,
					Login = db.Logins.FirstOrDefault()
				};

				Assert.IsTrue(MachineRecordExclusion.IsMachineRecordExcluded(machineRecord1));
				Assert.IsTrue(MachineRecordExclusion.IsMachineRecordExcluded(machineRecord2));
				Assert.IsTrue(MachineRecordExclusion.IsMachineRecordExcluded(machineRecord3));
				Assert.IsTrue(MachineRecordExclusion.IsMachineRecordExcluded(machineRecord4));
				Assert.IsFalse(MachineRecordExclusion.IsMachineRecordExcluded(machineRecord5));
				Assert.IsFalse(MachineRecordExclusion.IsMachineRecordExcluded(machineRecord6));
				Assert.IsFalse(MachineRecordExclusion.IsMachineRecordExcluded(machineRecord7));
				Assert.IsFalse(MachineRecordExclusion.IsMachineRecordExcluded(machineRecord8));
			}
		}
	}
}
