using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Allegiance.CommunitySecuritySystem.Common.Utility;

namespace Allegiance.CommunitySecuritySystem.CommonTest.Utility
{
	[TestClass]
	public class BadWordsTest
	{
		[TestMethod]
		public void TestBadWordDetection()
		{
			string teststring1 = "f u c k";
			string teststring2 = "quick f u c k fox";
			string teststring3 = "quick brown f u c k";
			string teststring4 = "fuckasshole";
			string teststring5 = "swank";
			string teststring6 = "wank";

			Assert.IsTrue(BadWords.ContainsBadWord(teststring1));
			Assert.IsTrue(BadWords.ContainsBadWord(teststring2));
			Assert.IsTrue(BadWords.ContainsBadWord(teststring3));
			Assert.IsTrue(BadWords.ContainsBadWord(teststring4));
			Assert.IsTrue(BadWords.ContainsBadWord(teststring5));
			Assert.IsTrue(BadWords.ContainsBadWord(teststring6));
		}
	}
}
