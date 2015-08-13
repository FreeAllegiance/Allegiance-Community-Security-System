using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Allegiance.CommunitySecuritySystem.DataAccess;

namespace Allegiance.CommunitySecuritySystem.ServerTest.DataAccessTests
{
	/// <summary>
	/// Summary description for AliasTest
	/// </summary>
	[TestClass]
	public class AliasTest : BaseTest
	{
		public AliasTest()
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
		public void GetCallsignFromStringWithTokensAndTags()
		{
			string noTags = "BackTrak";
			string withTag = "BackTrak@BS";
			string withToken = "?BackTrak";
			string withTagAndToken = "*BackTrak@BS";

			using (DataAccess.CSSDataContext db = new Allegiance.CommunitySecuritySystem.DataAccess.CSSDataContext())
			{
				Assert.AreEqual(noTags, DataAccess.Alias.GetCallsignFromStringWithTokensAndTags(db, noTags));
				Assert.AreEqual(noTags, DataAccess.Alias.GetCallsignFromStringWithTokensAndTags(db, withTag));
				Assert.AreEqual(noTags, DataAccess.Alias.GetCallsignFromStringWithTokensAndTags(db, withToken));
				Assert.AreEqual(noTags, DataAccess.Alias.GetCallsignFromStringWithTokensAndTags(db, withTagAndToken));
			}
		}

		[TestMethod]
		public void AliasLimitAndValidation()
		{
			string accountName = "AliasLength";
			string tooSmallAlias = "qa";
			string smallAlias = "qa3";
			string largeAlias = "qa123456789012345";
			string tooLargeAlias = "qa1234567890123456";
			string aliasStartsWithBadWord = "fucktrak";
			string aliasEndsWithBadWord = "backfuck";
			string aliasContainsBadWord = "backfucktrak";
			string aliasExistsOnASGS = "acsstest1";
			string aliasOnASGSPassword = "acsstest12";
			string invalidASGSPassword = "invalid";

			using(CSSDataContext db = new CSSDataContext())
			{
				db.Logins.DeleteAllOnSubmit(db.Logins.Where(p => p.Username == accountName));
				db.SubmitChanges();

				Login login = CreateUser(accountName, accountName, "nick@chi-town.com", 10);

				Alias alias;

				Assert.AreEqual(DataAccess.Enumerations.CheckAliasResult.Unavailable, Alias.ValidateUsage(db, login, false, null, ref tooSmallAlias, out alias));
				Assert.AreEqual(DataAccess.Enumerations.CheckAliasResult.Available, Alias.ValidateUsage(db, login, false, null, ref smallAlias, out alias));
				Assert.AreEqual(DataAccess.Enumerations.CheckAliasResult.Available, Alias.ValidateUsage(db, login, false, null, ref largeAlias, out alias));
				Assert.AreEqual(DataAccess.Enumerations.CheckAliasResult.Unavailable, Alias.ValidateUsage(db, login, false, null, ref tooLargeAlias, out alias));

				Assert.AreEqual(DataAccess.Enumerations.CheckAliasResult.ContainedBadWord, Alias.ValidateUsage(db, login, false, null, ref aliasStartsWithBadWord, out alias));
				Assert.AreEqual(DataAccess.Enumerations.CheckAliasResult.ContainedBadWord, Alias.ValidateUsage(db, login, false, null, ref aliasEndsWithBadWord, out alias));
				Assert.AreEqual(DataAccess.Enumerations.CheckAliasResult.ContainedBadWord, Alias.ValidateUsage(db, login, false, null, ref aliasContainsBadWord, out alias));

				Assert.AreEqual(DataAccess.Enumerations.CheckAliasResult.LegacyExists, Alias.ValidateUsage(db, login, false, null, ref aliasExistsOnASGS, out alias));
				Assert.AreEqual(DataAccess.Enumerations.CheckAliasResult.InvalidLegacyPassword, Alias.ValidateUsage(db, login, false, invalidASGSPassword, ref aliasExistsOnASGS, out alias));
				Assert.AreEqual(DataAccess.Enumerations.CheckAliasResult.Available, Alias.ValidateUsage(db, login, false, aliasOnASGSPassword, ref aliasExistsOnASGS, out alias));


			}
		}
	}
}
