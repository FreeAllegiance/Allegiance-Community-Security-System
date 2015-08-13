using Allegiance.CommunitySecuritySystem.Client.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security.AccessControl;
using System.IO;

namespace Allegiance.CommunitySecuritySystem.ClientTest.Utility
{
    
    
    /// <summary>
    ///This is a test class for FileSystemAccessTest and is intended
    ///to contain all FileSystemAccessTest Unit Tests
    ///</summary>
	[TestClass()]
	public class FileSystemAccessTest
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
		///A test for DoesUserHaveAccess
		///</summary>
		[TestMethod()]
		public void SetDirectoryPermissionsTest()
		{
			string testDir = @"c:\TestDirectory";
			string testSubDir = @"c:\TestDirectory\SubDir";
			
			if (Directory.Exists(testDir) == true)
				Directory.Delete(testDir, true);

			try
			{
				Directory.CreateDirectory(testSubDir);

				Assert.IsFalse(FileSystemAccess.DoesUserHaveAccessToDirectory(testDir, "BUILTIN\\Users", FileSystemRights.FullControl));
				Assert.IsFalse(FileSystemAccess.DoesUserHaveAccessToDirectory(testSubDir, "BUILTIN\\Users", FileSystemRights.FullControl));

				FileSystemAccess.SetDirectoryAccessByUserName(testDir, "BUILTIN\\Users", FileSystemRights.FullControl);

				Assert.IsTrue(FileSystemAccess.DoesUserHaveAccessToDirectory(testDir, "BUILTIN\\Users", FileSystemRights.FullControl));
				Assert.IsTrue(FileSystemAccess.DoesUserHaveAccessToDirectory(testSubDir, "BUILTIN\\Users", FileSystemRights.FullControl));
			}
			finally
			{
				//if (Directory.Exists(testDir) == true)
				//	Directory.Delete(testDir, true);
			}
		}

		[TestMethod()]
		public void SetDirectoryPermissionsBySIDTest()
		{
			string builtInUsersSID = "S-1-5-32-545";
			string testDir = @"c:\TestDirectory";
			string testSubDir = @"c:\TestDirectory\SubDir";

			if (Directory.Exists(testDir) == true)
				Directory.Delete(testDir, true);

			try
			{
				Directory.CreateDirectory(testSubDir);

				Assert.IsFalse(FileSystemAccess.DoesUserHaveAccessToDirectory(testDir, builtInUsersSID, FileSystemRights.FullControl));
				Assert.IsFalse(FileSystemAccess.DoesUserHaveAccessToDirectory(testSubDir, builtInUsersSID, FileSystemRights.FullControl));

				FileSystemAccess.SetDirectoryAccessBySID(testDir, builtInUsersSID, FileSystemRights.FullControl);

				Assert.IsTrue(FileSystemAccess.DoesUserHaveAccessToDirectory(testDir, builtInUsersSID, FileSystemRights.FullControl));
				Assert.IsTrue(FileSystemAccess.DoesUserHaveAccessToDirectory(testSubDir, builtInUsersSID, FileSystemRights.FullControl));
			}
			finally
			{
				//if (Directory.Exists(testDir) == true)
				//	Directory.Delete(testDir, true);
			}
		}

		
	}
}
