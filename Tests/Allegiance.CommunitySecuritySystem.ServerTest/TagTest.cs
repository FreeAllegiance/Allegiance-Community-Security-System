using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Linq.SqlClient;
using System.Configuration;
using System.IO;
using Allegiance.CommunitySecuritySystem.Server.Data;

namespace Allegiance.CommunitySecuritySystem.ServerTest
{
	/// <summary>
	/// Summary description for TagTest
	/// </summary>
	[TestClass]
	public class TagTest : BaseTest
	{
		public TagTest()
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
		public void TestSaveGameDataWithGameXmlFiles()
		{
			Console.WriteLine("Processing: " + ConfigurationManager.AppSettings["gameTestDataDirectory"]);

			// Increase this to simulate multiple game file loads. Takes a long time for each load.
			for (int i = 0; i < 1; i++)
			{
				Console.WriteLine("Creating TAG object.");

				Allegiance.CommunitySecuritySystem.Server.Tag tag = new Allegiance.CommunitySecuritySystem.Server.Tag();

				using (DataAccess.CSSDataContext db = new Allegiance.CommunitySecuritySystem.DataAccess.CSSDataContext())
				{
					Console.WriteLine("Looking for files.");

					foreach (string file in Directory.GetFiles(ConfigurationManager.AppSettings["gameTestDataDirectory"], ConfigurationManager.AppSettings["gameTestDataFileFilter"]))
					{
						Console.WriteLine("Loading: " + file);

						string gameData = File.ReadAllText(file);
						GameDataset gameDataset = new GameDataset();
						gameDataset.ReadXml(new StringReader(gameData), System.Data.XmlReadMode.IgnoreSchema);

						foreach (GameDataset.TeamRow team in gameDataset.Team)
						{
							var alias = DataAccess.Alias.GetAliasByCallsign(db, team.Commander);

							if (alias == null)
								CreateUser(DataAccess.Alias.GetCallsignFromStringWithTokensAndTags(db, team.Commander), "test12", "nick@chi-town.com", 10);
						}

						foreach (GameDataset.TeamMemberRow teamMember in gameDataset.TeamMember)
						{
							var alias = DataAccess.Alias.GetAliasByCallsign(db, teamMember.Callsign);

							if (alias == null)
								CreateUser(DataAccess.Alias.GetCallsignFromStringWithTokensAndTags(db, teamMember.Callsign), "test12", "nick@chi-town.com", 20);
						}


						string message;
						int result = tag.SaveGameData(gameData, false, out message);

						Assert.IsTrue(result > 0, message);
					}
				}
			}
		}


		[TestMethod]
		public void TestSaveGameDataWithStaticData()
		{
			string compressedXml = GetCompressedGameDataXml();

			Allegiance.CommunitySecuritySystem.Server.Tag tag = new Allegiance.CommunitySecuritySystem.Server.Tag();
			
			string message;
			int result = tag.SaveGameData(compressedXml, out message);

			Assert.IsTrue(result > 0, message);
		}

		public string GetCompressedGameDataXml()
		{
			int testPilot1ID;
			int testPilot2ID;
			using(DataAccess.CSSDataContext db = new Allegiance.CommunitySecuritySystem.DataAccess.CSSDataContext())
			{
				var testPilot1Alias = DataAccess.Alias.GetAliasByCallsign(db, "BackTrak");
				testPilot1ID = testPilot1Alias.Id;

				var testPilot2Alias = DataAccess.Alias.GetAliasByCallsign(db, "TheBored");
				testPilot2ID = testPilot2Alias.Id;
			}

			int gameID = 1234567;

			Allegiance.CommunitySecuritySystem.Server.Data.GameDataset gameData = new Allegiance.CommunitySecuritySystem.Server.Data.GameDataset();

			AddGame(gameData, gameID);

			AddRandomGameEvents(gameData, gameID, 10);

			AddTeams(gameData, gameID);

			AddRandomPlayersToTeam(gameData, 1, 5);

			AddRandomPlayersToTeam(gameData, 2, 5);

			AddCommanderToTeam(gameData, "^Orion@PK", 1);
			AddCommanderToTeam(gameData, "BackTrak", 1);


			AddChatLogMessages(gameData, gameID, 10);

			string xml = gameData.GetXml();

			string compressedXml = Compress(xml);

			return compressedXml;
		}

		private void AddCommanderToTeam(Allegiance.CommunitySecuritySystem.Server.Data.GameDataset gameData, string commanderCallsign, int teamID)
		{
			Random random = new Random();

			Server.Data.GameDataset.TeamMemberRow teamMemberRow = gameData.TeamMember.NewTeamMemberRow();
			teamMemberRow.Callsign = commanderCallsign;
			teamMemberRow.Duration = random.Next(10, 30);
			teamMemberRow.JoinTime = DateTime.Now.AddMinutes(-30);
			teamMemberRow.LeaveTime = teamMemberRow.JoinTime.AddMinutes(teamMemberRow.Duration);
			teamMemberRow.TeamID = teamID;

			gameData.TeamMember.AddTeamMemberRow(teamMemberRow);
		}

		private void AddChatLogMessages(Allegiance.CommunitySecuritySystem.Server.Data.GameDataset gameData, int gameID, int chatLogCount)
		{
			Random random = new Random();

			using (DataAccess.CSSDataContext db = new Allegiance.CommunitySecuritySystem.DataAccess.CSSDataContext())
			{
				DataAccess.Alias[] aliases = db.Alias.ToArray();

				for (int i = 0; i < chatLogCount; i++)
				{
					Server.Data.GameDataset.ChatLogRow chatLogRow = gameData.ChatLog.NewChatLogRow();

					chatLogRow.ChatTime = DateTime.Now.AddMinutes(-1 * random.Next(0, 30));
					chatLogRow.GameID = gameID;
					chatLogRow.SpeakerName = aliases[random.Next(0, 10)].Callsign;
					chatLogRow.TargetName = aliases[random.Next(0, 10)].Callsign;
					chatLogRow.Text = "This is a chat message: " + i;

					gameData.ChatLog.AddChatLogRow(chatLogRow);
				}
			}
		}

		private void AddRandomPlayersToTeam(Allegiance.CommunitySecuritySystem.Server.Data.GameDataset gameData, int teamID, int playerCount)
		{
			Random random = new Random();

			using (DataAccess.CSSDataContext db = new Allegiance.CommunitySecuritySystem.DataAccess.CSSDataContext())
			{
				DataAccess.Alias[] aliases = db.Alias.GroupBy(p => p.LoginId, r => r).Select(p => p.First()).Where(p => p.Callsign.StartsWith("Test")).ToArray();

				for (int i = 0; i < playerCount; i++)
				{
					Server.Data.GameDataset.TeamMemberRow teamMemberRow = gameData.TeamMember.NewTeamMemberRow();
					teamMemberRow.Callsign = aliases[i + (playerCount * teamID)].Callsign;
					teamMemberRow.Duration = random.Next(10, 30);
					teamMemberRow.JoinTime = DateTime.Now.AddMinutes(-30);
					teamMemberRow.LeaveTime = teamMemberRow.JoinTime.AddMinutes(teamMemberRow.Duration);
					teamMemberRow.TeamID = teamID;

					gameData.TeamMember.AddTeamMemberRow(teamMemberRow);
				}
			}
		}

		private void AddTeams(Allegiance.CommunitySecuritySystem.Server.Data.GameDataset gameData, int gameID)
		{
			Allegiance.CommunitySecuritySystem.Server.Data.GameDataset.TeamRow teamRow = gameData.Team.NewTeamRow();

			teamRow.Commander = "^Orion@PK";
			teamRow.Faction = "Rixian Unity";
			teamRow.GameID = gameID;
			teamRow.ResearchedExpansion = true;
			teamRow.ResearchedShipyard = false;
			teamRow.ResearchedStarbase = true;
			teamRow.ResearchedSupremacy = false;
			teamRow.ResearchedTactical = true;
			teamRow.TeamID = 1;
			teamRow.TeamName = "Team 1";
			teamRow.TeamNumber = 1;
			teamRow.Won = true;

			gameData.Team.AddTeamRow(teamRow);

			

			teamRow = gameData.Team.NewTeamRow();

			teamRow.Commander = "BackTrak";
			teamRow.Faction = "Gigacorp";
			teamRow.GameID = gameID;
			teamRow.ResearchedExpansion = false;
			teamRow.ResearchedShipyard = true;
			teamRow.ResearchedStarbase = false;
			teamRow.ResearchedSupremacy = true;
			teamRow.ResearchedTactical = false;
			teamRow.TeamID = 2;
			teamRow.TeamName = "Team 2";
			teamRow.TeamNumber = 2;
			teamRow.Won = false;

			gameData.Team.AddTeamRow(teamRow);
		}

		private void AddGame(Allegiance.CommunitySecuritySystem.Server.Data.GameDataset gameData, int gameID)
		{
			Allegiance.CommunitySecuritySystem.Server.Data.GameDataset.GameRow gameRow = gameData.Game.NewGameRow();
			gameRow.AllowDefections = true;
			gameRow.AllowDevelopments = true;
			gameRow.AllowShipyards = true;
			gameRow.Conquest = true;
			gameRow.CoreFile = "CC 1.0.8 - Test";
			gameRow.DeathMatch = false;
			gameRow.DeathmatchGoal = 0;
			gameRow.EndTime = DateTime.Now;
			gameRow.FriendlyFire = false;
			gameRow.GameID = gameID;
			gameRow.GameName = "Test Game Name";
			gameRow.InvulnerableStations = false;
			gameRow.MapName = "HiHigher";
			gameRow.MaxImbalance = 0;
			gameRow.Resources = 2;
			gameRow.RevealMap = false;
			gameRow.SquadGame = false;
			gameRow.StartingMoney = 1.25F;
			gameRow.StartTime = DateTime.Now.AddHours(-1);
			gameRow.StatsCount = true;
			gameRow.TotalMoney = 1.0F;

			gameData.Game.AddGameRow(gameRow);
		}

		private void AddRandomGameEvents(Allegiance.CommunitySecuritySystem.Server.Data.GameDataset gameData, int gameID, int eventCount)
		{
			Random random = new Random();

			using(DataAccess.CSSDataContext db = new Allegiance.CommunitySecuritySystem.DataAccess.CSSDataContext())
			{
				//Alias.GroupBy(p => p.LoginId, r => r.Id).Select(p => p.First())

				DataAccess.Alias[] aliases = db.Alias.GroupBy(p => p.LoginId, r => r).Select(p => p.First()).Where(p => p.Callsign.StartsWith("Test")).ToArray();

				//DataAccess.Alias [] aliases = db.Alias.ToArray();

				for(int i = 0; i < eventCount; i++)
				{
					int performerID = random.Next(0, aliases.Length);
					int targetID = random.Next(0, aliases.Length);

					while(targetID == performerID)
						targetID = random.Next(0, aliases.Length);

					int indirectID = random.Next(0, aliases.Length);

					while (indirectID == performerID || indirectID == targetID)
						indirectID = random.Next(0, aliases.Length);

					Allegiance.CommunitySecuritySystem.Server.Data.GameDataset.GameEventRow gameEventRow = gameData.GameEvent.NewGameEventRow();
					gameEventRow.EventID = (int)Common.Enumerations.AllegianceEventIDs.ShipKilled; ;
					gameEventRow.EventTime = DateTime.Now.AddMinutes(-1 * random.Next(0, 10));
					gameEventRow.GameID = gameID;
					gameEventRow.IndirectID = indirectID;
					gameEventRow.IndirectName = aliases[indirectID].Callsign;
					gameEventRow.PerformerID = performerID;
					gameEventRow.PerformerName = aliases[performerID].Callsign;
					gameEventRow.TargetID = targetID;
					gameEventRow.TargetName = aliases[targetID].Callsign;

					if (((int) Math.Floor(eventCount / 2M)) == i)
					{
						gameEventRow.TargetID = 11;
						gameEventRow.TargetName = ".Miner 230";
					}

					if (((int)Math.Floor(eventCount / 3M)) == i)
					{
						gameEventRow.TargetID = 12;
						gameEventRow.TargetName = "Outpost";
						gameEventRow.EventID = (int)Common.Enumerations.AllegianceEventIDs.StationDestroyed;
					}

					if (((int)Math.Floor(eventCount / 4M)) == i)
					{
						gameEventRow.TargetID = 13;
						gameEventRow.TargetName = "Supremecy Center";
						gameEventRow.EventID = (int)Common.Enumerations.AllegianceEventIDs.StationCaptured;
					}

					gameData.GameEvent.AddGameEventRow(gameEventRow);
				}
			}
		}


		public string Compress(string inputString)
		{
			// Create the zip stream
			System.IO.MemoryStream memstream = new System.IO.MemoryStream();
			ICSharpCode.SharpZipLib.GZip.GZipOutputStream zipstream = new ICSharpCode.SharpZipLib.GZip.GZipOutputStream(memstream);

			// Get the bytes of the input string
			Byte[] buffer = System.Text.Encoding.Unicode.GetBytes(inputString);

			// Write the bytes to the zipstream
			zipstream.Write(buffer, 0, buffer.Length);

			// Clean up
			zipstream.Close();
			memstream.Close();

			// Get the Base64 representation of the compressed string
			buffer = memstream.ToArray();
			string Result = Convert.ToBase64String(buffer);

			return Result;

		}
	}
}
