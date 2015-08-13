using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Allegiance.CommunitySecuritySystem.Client.ClientService;
using System.IO;
using Allegiance.CommunitySecuritySystem.Client.Utility;
using System.Security.Cryptography;

namespace Allegiance.CommunitySecuritySystem.ClientTest.Service
{
	/// <summary>
	/// Summary description for AutoUpdate
	/// </summary>
	[TestClass]
	public class AutoUpdate
	{
		public AutoUpdate()
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
		public void GetPendingUpdateQueues()
		{
			LobbyResult lobbyResult = new LobbyResult()
			{
				Host = "www.test.com",
				LobbyId = 1,
				LobbyIdSpecified = true,
				Name = "TestUpdate"
			};

			

			string testPath = Path.Combine(Allegiance.CommunitySecuritySystem.Client.Integration.AllegianceRegistry.LobbyPath, lobbyResult.Name);

			if(Directory.Exists(testPath) == false)
				Directory.CreateDirectory(testPath);

			File.WriteAllText(Path.Combine(testPath, "test1.txt"), "this is a test file. It is for testing.");

			var encryption = new Encryption<SHA1>();
			var checksum = encryption.Calculate(Path.Combine(testPath, "test1.txt"));

			AutoUpdateResult autoUpdateResult = new AutoUpdateResult()
			{
				AutoUpdateBaseAddress = "http://www.pork.com",
				Files = new FindAutoUpdateFilesResult[]
				{
					new FindAutoUpdateFilesResult()
					{
						AutoUpdateFileId = 1,
						AutoUpdateFileIdSpecified = true,
						 CurrentVersion = "1.0.0.0",
						 DateCreated = DateTime.Parse("4/8/2014"),
						 DateCreatedSpecified = true,
						 DateModified = DateTime.Parse("4/8/2014"),
						 DateModifiedSpecified = true,
						 Filename = "test1-notfound.txt",
						 IsProtected = false,
						 IsProtectedSpecified = true,
						 LobbyId = 1, 
						 LobbyIdSpecified = true,
						 ValidChecksum = checksum
					}
				}
			};

			if (File.Exists("autoupdate.ds") == true)
				File.Delete("autoupdate.ds");

			List<FindAutoUpdateFilesResult> result = Client.Service.AutoUpdate.ProcessPendingUpdates(lobbyResult, autoUpdateResult);

			Assert.AreEqual(1, result.Count, "File was not found, one update should have applied.");

			autoUpdateResult.Files[0].Filename = "test1.txt";

			result = Client.Service.AutoUpdate.ProcessPendingUpdates(lobbyResult, autoUpdateResult);

			Assert.AreEqual(0, result.Count, "No updates should have been done, checksums match.");

			autoUpdateResult.Files[0].ValidChecksum = "INVALID_CHECKSUM";

			result = Client.Service.AutoUpdate.ProcessPendingUpdates(lobbyResult, autoUpdateResult);

			Assert.AreEqual(1, result.Count, "One update should have been done, checksum miss-match with no prior update record.");

			var autoUpdate = DataStore.Open("autoupdate.ds", "Ga46^#a042");

			var lastResults = new Dictionary<string, FindAutoUpdateFilesResult>();
			//foreach (var res in result)
			//	lastResults.Add(res.Filename, res);

			string dataKey = "Files_" + lobbyResult.LobbyId;
			autoUpdate[dataKey] = lastResults;
			
			autoUpdate.Save();

			result = Client.Service.AutoUpdate.ProcessPendingUpdates(lobbyResult, autoUpdateResult);

			Assert.AreEqual(1, result.Count, "One update should have been done, checksum miss-match prior update dictionary exists, but no record for file.");

			autoUpdateResult.Files[0].ValidChecksum = checksum;

			foreach (var res in result)
				lastResults.Add(res.Filename, res);

			autoUpdate[dataKey] = lastResults;

			autoUpdate.Save();

			result = Client.Service.AutoUpdate.ProcessPendingUpdates(lobbyResult, autoUpdateResult);

			Assert.AreEqual(0, result.Count, "No updates should have been done, versions match.");

			autoUpdateResult.Files[0].CurrentVersion = "1.0.0.1";

			result = Client.Service.AutoUpdate.ProcessPendingUpdates(lobbyResult, autoUpdateResult);

			Assert.AreEqual(1, result.Count, "File version miss-match with previous version of file, update applied.");

			autoUpdateResult.Files[0].CurrentVersion = "1.0.0.0";
			autoUpdateResult.Files[0].IsProtected = false;
			autoUpdateResult.Files[0].ValidChecksum = "INVALID_CHECKSUM";

			result = Client.Service.AutoUpdate.ProcessPendingUpdates(lobbyResult, autoUpdateResult);

			Assert.AreEqual(1, result.Count, "updates should have been done, versions are same, checksum is different, file is not protected and the user has not modified the file.");

			autoUpdateResult.Files[0].DateModified = DateTime.Parse("1/1/2015");

			result = Client.Service.AutoUpdate.ProcessPendingUpdates(lobbyResult, autoUpdateResult);

			Assert.AreEqual(0, result.Count, "updates should not have been done, versions are same, checksum is different, file is not protected and the user has modified the file since the last update.");

			//autoUpdateResult.Files[0].DateModified = DateTime.Parse("4/8/2014");

			autoUpdateResult.Files[0].CurrentVersion = "1.0.0.0";
			autoUpdateResult.Files[0].IsProtected = true;
			autoUpdateResult.Files[0].ValidChecksum = "INVALID_CHECKSUM";

			result = Client.Service.AutoUpdate.ProcessPendingUpdates(lobbyResult, autoUpdateResult);

			Assert.AreEqual(1, result.Count, "updates should have been done, versions are same, checksum is different, file is protected and the user has modified the file.");

		}
	}
}
