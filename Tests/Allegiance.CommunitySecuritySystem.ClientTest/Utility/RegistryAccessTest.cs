using Allegiance.CommunitySecuritySystem.Client.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security.AccessControl;
using Microsoft.Win32;

namespace Allegiance.CommunitySecuritySystem.ClientTest.Utility
{
    /// <summary>
    ///This is a test class for RegistrySecurityTest and is intended
    ///to contain all RegistrySecurityTest Unit Tests
    ///</summary>
	[TestClass()]
	public class RegistryAccessTest
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
		public void SetRegistryPermissionsTest()
		{
			if (Registry.LocalMachine.OpenSubKey("Software\\TestKey") != null)
				Registry.LocalMachine.DeleteSubKeyTree("Software\\TestKey");

			try
			{
				RegistryKey registryKey = Registry.LocalMachine.CreateSubKey("Software\\TestKey");
				RegistryKey subRegistryKey = registryKey.CreateSubKey("TestSubKey");
				subRegistryKey.SetValue("TestValue", "Pork Muffins!");

				Assert.IsFalse(RegistryAccess.DoesUserHaveAccess(registryKey, "BUILTIN\\Users", RegistryRights.FullControl));
				Assert.IsFalse(RegistryAccess.DoesUserHaveAccess(subRegistryKey, "BUILTIN\\Users", RegistryRights.FullControl));

				RegistryAccess.SetUserAccessByUserName(registryKey, "BUILTIN\\Users", RegistryRights.FullControl);

				Assert.IsTrue(RegistryAccess.DoesUserHaveAccess(registryKey, "BUILTIN\\Users", RegistryRights.FullControl));
				Assert.IsTrue(RegistryAccess.DoesUserHaveAccess(subRegistryKey, "BUILTIN\\Users", RegistryRights.FullControl));
			}
			finally
			{
				if (Registry.LocalMachine.OpenSubKey("Software\\TestKey") != null)
					Registry.LocalMachine.DeleteSubKeyTree("Software\\TestKey");
			}
		}

		/// <summary>
		///A test for DoesUserHaveAccess
		///</summary>
		[TestMethod()]
		public void SetRegistryPermissionsBySIDTest()
		{
			string builtInUsersSID = "S-1-5-32-545";

			if (Registry.LocalMachine.OpenSubKey("Software\\TestKey") != null)
				Registry.LocalMachine.DeleteSubKeyTree("Software\\TestKey");

			try
			{
				RegistryKey registryKey = Registry.LocalMachine.CreateSubKey("Software\\TestKey");
				RegistryKey subRegistryKey = registryKey.CreateSubKey("TestSubKey");
				subRegistryKey.SetValue("TestValue", "Pork Muffins!");

				Assert.IsFalse(RegistryAccess.DoesUserHaveAccess(registryKey, builtInUsersSID, RegistryRights.FullControl));
				Assert.IsFalse(RegistryAccess.DoesUserHaveAccess(subRegistryKey, builtInUsersSID, RegistryRights.FullControl));

				RegistryAccess.SetUserAccessBySID(registryKey, builtInUsersSID, RegistryRights.FullControl);

				Assert.IsTrue(RegistryAccess.DoesUserHaveAccess(registryKey, builtInUsersSID, RegistryRights.FullControl));
				Assert.IsTrue(RegistryAccess.DoesUserHaveAccess(subRegistryKey, builtInUsersSID, RegistryRights.FullControl));
			}
			finally
			{
				if (Registry.LocalMachine.OpenSubKey("Software\\TestKey") != null)
					Registry.LocalMachine.DeleteSubKeyTree("Software\\TestKey");
			}
		}
	}
}
