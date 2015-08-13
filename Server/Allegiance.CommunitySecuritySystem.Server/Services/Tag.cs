using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Allegiance.CommunitySecuritySystem.Server.Interfaces;
using System.ServiceModel.Activation;
using System.IO;
using System.Xml;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Allegiance.CommunitySecuritySystem.Server.Properties;
using Allegiance.CommunitySecuritySystem.Common.Enumerations;
using Allegiance.CommunitySecuritySystem.DataAccess;
using Allegiance.CommunitySecuritySystem.Server.Utilities;

namespace Allegiance.CommunitySecuritySystem.Server
{
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	public partial class Tag : ITag
	{
		private const int MinimumNumberOfPlayersPerTeam = 2;

        private const int ShipKillPoints = 3;
        private const int DroneKillPoints = 5;
        private const int StationKillPoints = 10;
        private const int StationCapturePoints = 15;

		#region ITag Members

		public int SaveGameData(string gameData, out string message)
		{
			return SaveGameData(gameData, true, out message);
		}

		public int SaveGameData(string gameData, bool isCompressedAndBase64Encoded, out string message)
		{
			try
			{
				Data.GameDataset gameDataset = new Data.GameDataset();
				//gameDataset.EnforceConstraints = false;
				//gameDataset.

				if (isCompressedAndBase64Encoded == true)
				{
					byte[] binaryGameData = Convert.FromBase64String(gameData);

					MemoryStream memoryStream = new MemoryStream(binaryGameData);
					ICSharpCode.SharpZipLib.GZip.GZipInputStream zipStream = new ICSharpCode.SharpZipLib.GZip.GZipInputStream(memoryStream);
					StreamReader streamReader = new StreamReader(zipStream, System.Text.Encoding.Unicode);
					//string gameDataXml = streamReader.ReadToEnd();


					gameDataset.ReadXml(streamReader, System.Data.XmlReadMode.IgnoreSchema);
				}
				else
				{
					//gameDataset.ReadXml(new StringReader(gameData));
					gameDataset.ReadXml(new StringReader(gameData), System.Data.XmlReadMode.IgnoreSchema);
				}

				if (String.IsNullOrEmpty(Settings.Default.TagLastGameDataXmlFileLogLocation) == false)
					File.WriteAllText(Path.Combine(Settings.Default.TagLastGameDataXmlFileLogLocation, Guid.NewGuid().ToString() + ".xml"), gameDataset.GetXml());

				string currentIPAddress;

				if (OperationContext.Current != null)
				{
					//http://nayyeri.net/detect-client-ip-in-wcf-3-5
					OperationContext context = OperationContext.Current;
					MessageProperties messageProperties = context.IncomingMessageProperties;
					RemoteEndpointMessageProperty endpointProperty = (RemoteEndpointMessageProperty)messageProperties[RemoteEndpointMessageProperty.Name];

					currentIPAddress = endpointProperty.Address;
				}
				else
					currentIPAddress = "127.0.0.1"; // Supports unit tests.

				int gameID = 0;

				using (DataAccess.CSSDataContext db = new DataAccess.CSSDataContext())
				{
					using (DataAccess.CSSStatsDataContext statsDB = new DataAccess.CSSStatsDataContext())
					{
						var gameServer = statsDB.GameServers.FirstOrDefault(p => p.GameServerIPs.Where(r => r.IPAddress == currentIPAddress).Count() > 0);

						if (gameServer == null)
							throw new Exception("You may not upload data from this address: " + currentIPAddress);

						try
						{
							foreach (Data.GameDataset.GameRow gameRow in gameDataset.Game)
								gameID = SaveGame(db, statsDB, gameServer, gameRow);
						}
						catch (Exception ex)
						{
							if (String.IsNullOrEmpty(Settings.Default.TagExceptionLogFileName) == false)
								File.AppendAllText(Settings.Default.TagExceptionLogFileName, DateTime.Now.ToString() + ": " + ex.ToString() + "\n\n\n");

							throw;
						}

						statsDB.SubmitChanges();
						db.SubmitChanges();
					}
				}

				// Update AllegSkill rank.
				AllegSkill.Calculator.UpdateAllegSkillForGame(gameID);

                // Update Prestige Rank.
                using (DataAccess.CSSStatsDataContext statsDB = new DataAccess.CSSStatsDataContext())
                {
                    var game = statsDB.Games.FirstOrDefault(p => p.GameIdentID == gameID);
                    if (game == null)
                    {
                        Error.Write(new Exception("Tag.SaveGameData(): Couldn't get game for GameID: " + gameID));
                    }
                    else
                    {
                        PrestigeRankCalculator psc = new PrestigeRankCalculator();
                        psc.Calculate(statsDB, game);
                    }
                }

				message = "Game saved.";
				return gameID;
			}
			catch (Exception ex)
			{
				message = ex.ToString();
				return -1;
			}
		}

		private string TrimString(string target, int length)
		{
			if (String.IsNullOrEmpty(target) == true || target.Length < length)
				return target;

			return target.Substring(0, length);
		}

		private int SaveGame(DataAccess.CSSDataContext db, DataAccess.CSSStatsDataContext statsDB, DataAccess.GameServer gameServer, Data.GameDataset.GameRow gameRow)
		{
			DataAccess.Game game = new Allegiance.CommunitySecuritySystem.DataAccess.Game()
			{
				GameDefections = gameRow.AllowDefections,
				GameDevelopments = gameRow.AllowDevelopments,
				GameShipyards = gameRow.AllowShipyards,
				GameConquest = gameRow.Conquest,
				GameCore = TrimString(gameRow.CoreFile, 50),
				GameDeathMatch = gameRow.DeathMatch,
				GameDeathmatchGoal = gameRow.DeathmatchGoal,
				GameEndTime = gameRow.EndTime,
				GameFriendlyFire = gameRow.FriendlyFire,
				GameName = TrimString(gameRow.GameName, 254),
				GameInvulStations = gameRow.InvulnerableStations,
				GameMap = TrimString(gameRow.MapName, 49),
				GameMaxImbalance = gameRow.MaxImbalance,
				GameResources = gameRow.Resources,
				GameRevealMap = gameRow.RevealMap,
				GameSquadGame = gameRow.SquadGame,
				GameStartingMoney = gameRow.StartingMoney,
				GameStartTime = gameRow.StartTime,
				GameStatsCount = gameRow.StatsCount,
				GameTotalMoney = gameRow.TotalMoney,
				GameID = gameRow.GameID,
				GameServer = gameServer.GameServerID
			};

			statsDB.Games.InsertOnSubmit(game);

			try
			{
				statsDB.SubmitChanges();
			}
			catch (Exception ex)
			{
				string dbLengthErrors = Utilities.LinqErrorDetector.AnalyzeDBChanges(statsDB);
				throw new Exception("CSSStats[games]: DB Error, Linq Length Analysis: " + dbLengthErrors + "\r\n", ex);
			}

			SaveGameEvents(db, statsDB, gameRow, game.GameIdentID);

			SaveTeams(db, statsDB, gameRow, game.GameIdentID);

			SaveChatLog(db, statsDB, gameRow, game.GameIdentID);

			try
			{
				statsDB.SubmitChanges();
			}
			catch (Exception ex)
			{
				string dbLengthErrors = Utilities.LinqErrorDetector.AnalyzeDBChanges(statsDB);
				throw new Exception("CSSStats[game data]: DB Error, Linq Length Analysis: " + dbLengthErrors + "\r\n", ex);
			}

			try
			{
				db.SubmitChanges();
			}
			catch (Exception ex)
			{
				string dbLengthErrors = Utilities.LinqErrorDetector.AnalyzeDBChanges(db);
				throw new Exception("CSS DB Error, Linq Length Analysis: " + dbLengthErrors + "\r\n", ex);
			}

			statsDB.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, game);

			UpdateLeaderboard(game.GameIdentID);

			UpdateFactionStats(game.GameIdentID);

			UpdateMetrics(game.GameIdentID);

			statsDB.SubmitChanges();
			db.SubmitChanges();

			return game.GameIdentID;
		}




		private void SaveChatLog(Allegiance.CommunitySecuritySystem.DataAccess.CSSDataContext db, DataAccess.CSSStatsDataContext statsDB, Allegiance.CommunitySecuritySystem.Server.Data.GameDataset.GameRow gameRow, int gameIdentID)
		{
			foreach (Data.GameDataset.ChatLogRow chatLogRow in gameRow.GetChatLogRows())
			{
				DataAccess.GameChatLog chatLog = new Allegiance.CommunitySecuritySystem.DataAccess.GameChatLog()
				{
					GameChatSpeakerName = TrimString(DataAccess.Alias.GetCallsignFromStringWithTokensAndTags(db, chatLogRow.SpeakerName), 49),
					GameChatTargetName = TrimString(DataAccess.Alias.GetCallsignFromStringWithTokensAndTags(db, chatLogRow.TargetName), 49),
					GameChatText = TrimString(chatLogRow.Text, 2063),
					GameChatTime = chatLogRow.ChatTime,
					GameID = gameIdentID
				};

				statsDB.GameChatLogs.InsertOnSubmit(chatLog);
			}
		}

		private void SaveTeams(Allegiance.CommunitySecuritySystem.DataAccess.CSSDataContext db, DataAccess.CSSStatsDataContext statsDB, Allegiance.CommunitySecuritySystem.Server.Data.GameDataset.GameRow gameRow, int gameIdentID)
		{
			foreach (Data.GameDataset.TeamRow teamRow in gameRow.GetTeamRows())
			{
				var commanderAlias = DataAccess.Alias.GetAliasByCallsign(db, teamRow.Commander);
				int commanderLoginID = 0;

				if (commanderAlias != null)
					commanderLoginID = commanderAlias.Login.Id;

				DataAccess.GameTeam team = new Allegiance.CommunitySecuritySystem.DataAccess.GameTeam()
				{
					GameID = gameIdentID,
					GameTeamCommander = TrimString(teamRow.Commander, 49),
					GameTeamCommanderLoginID = commanderLoginID,
					GameTeamExpansion = teamRow.ResearchedExpansion,
					GameTeamFaction = teamRow.Faction,
					GameTeamID = teamRow.TeamID,
					GameTeamName = TrimString(teamRow.TeamName, 49),
					GameTeamNumber = teamRow.TeamNumber,
					GameTeamShipyard = teamRow.ResearchedShipyard,
					GameTeamStarbase = teamRow.ResearchedStarbase,
					GameTeamSupremacy = teamRow.ResearchedSupremacy,
					GameTeamTactical = teamRow.ResearchedTactical,
					GameTeamWinner = teamRow.Won
				};

				statsDB.GameTeams.InsertOnSubmit(team);
				statsDB.SubmitChanges();

				foreach (Data.GameDataset.TeamMemberRow teamMemberRow in teamRow.GetTeamMemberRows())
				{
					var teamMemberAlias = DataAccess.Alias.GetAliasByCallsign(db, teamMemberRow.Callsign);
					int loginID = 0;
					if (teamMemberAlias != null)
						loginID = teamMemberAlias.Login.Id;

					DataAccess.GameTeamMember teamMember = new Allegiance.CommunitySecuritySystem.DataAccess.GameTeamMember()
					{
						GameTeamID = team.GameTeamIdentID,
						GameTeamMemberCallsign = TrimString(DataAccess.Alias.GetCallsignFromStringWithTokensAndTags(db, teamMemberRow.Callsign), 49),
						GameTeamMemberDuration = teamMemberRow.Duration,
						GameTeamMemberLoginID = loginID
					};

					statsDB.GameTeamMembers.InsertOnSubmit(teamMember);
				}

				statsDB.SubmitChanges();
			}
		}

		private void SaveGameEvents(Allegiance.CommunitySecuritySystem.DataAccess.CSSDataContext db, DataAccess.CSSStatsDataContext statsDB, Allegiance.CommunitySecuritySystem.Server.Data.GameDataset.GameRow gameRow, int gameIdentID)
		{
			foreach (Data.GameDataset.GameEventRow gameEventRow in gameRow.GetGameEventRows())
			{
				int performerLoginID = 0;
				int targetLoginID = 0;

				try
				{
					var performerAlias = DataAccess.Alias.GetAliasByCallsign(db, gameEventRow.PerformerName);
					
					if (performerAlias != null)
						performerLoginID = performerAlias.Login.Id;

					var targetAlias = DataAccess.Alias.GetAliasByCallsign(db, gameEventRow.TargetName);
					
					if (targetAlias != null)
						targetLoginID = targetAlias.Login.Id;

					DataAccess.GameEvent gameEvent = new DataAccess.GameEvent()
					{
						GameEventTime = gameEventRow.EventTime,
						GameID = gameIdentID,
						GameEventIndirectID = gameEventRow.IndirectID,
						GameEventIndirectName = TrimString(DataAccess.Alias.GetCallsignFromStringWithTokensAndTags(db, gameEventRow.IndirectName), 254),
						GameEventPerformerID = gameEventRow.PerformerID,
						GameEventPerformerName = TrimString(DataAccess.Alias.GetCallsignFromStringWithTokensAndTags(db, gameEventRow.PerformerName), 254),
						GameEventPerformerLoginID = performerLoginID,
						GameEventTargetID = gameEventRow.TargetID,
						GameEventTargetName = TrimString(DataAccess.Alias.GetCallsignFromStringWithTokensAndTags(db, gameEventRow.TargetName), 254),
						GameEventTargetLoginID = targetLoginID,
						EventID = gameEventRow.EventID
					};

					statsDB.GameEvents.InsertOnSubmit(gameEvent);

					statsDB.SubmitChanges();
				}
				catch(Exception ex)
				{
					
					string errorDetails = String.Format(@"SaveGameEvents(): Insert error! 
		gameIdentID = {0},
		gameEventRow.EventID = {1},
		gameEventRow.EventTime = {2},
		gameEventRow.GameID = {3},
		gameEventRow.IndirectID = {4},
		gameEventRow.IndirectName = {5},
		gameEventRow.PerformerID = {6},
		gameEventRow.PerformerName = {7},
		gameEventRow.TargetID = {8},
		gameEventRow.TargetName = {9},
		performerLoginID = {10},
		targetLoginID = {11},
		DataAccess.Alias.GetCallsignFromStringWithTokensAndTags(db, gameEventRow.IndirectName) = {12},
		DataAccess.Alias.GetCallsignFromStringWithTokensAndTags(db, gameEventRow.PerformerName) = {13},
		DataAccess.Alias.GetCallsignFromStringWithTokensAndTags(db, gameEventRow.TargetName) = {14}
		===================================================
{15}
",
		gameIdentID,
		gameEventRow.EventID,
		gameEventRow.EventTime,
		gameEventRow.GameID,
		gameEventRow.IndirectID,
		gameEventRow.IndirectName,
		gameEventRow.PerformerID,
		gameEventRow.PerformerName,
		gameEventRow.TargetID,
		gameEventRow.TargetName,
		performerLoginID,
		targetLoginID,
		DataAccess.Alias.GetCallsignFromStringWithTokensAndTags(db, gameEventRow.IndirectName),
		DataAccess.Alias.GetCallsignFromStringWithTokensAndTags(db, gameEventRow.PerformerName),
		DataAccess.Alias.GetCallsignFromStringWithTokensAndTags(db, gameEventRow.TargetName),
		ex.ToString());

					if (String.IsNullOrEmpty(Settings.Default.TagExceptionLogFileName) == false)
						File.AppendAllText(Settings.Default.TagExceptionLogFileName, DateTime.Now.ToString() + ": " + errorDetails + "\n\n\n");


				}
			}

			
		}

		private bool IsGameEligibleForLogging(DataAccess.Game game)
		{
			// Not logging single games.
			if (game.GameTeams.Count() < 2)
				return false;

			// Games must have at least MinimumNumberOfPlayersPerTeam players on each side that have played for more than 5 minutes.
			bool hadEnoughEligiblePlayers = true;
			foreach (DataAccess.GameTeam team in game.GameTeams)
			{
				if (team.GameTeamMembers.Where(p => p.GameTeamMemberDuration / 60 >= 5).Count() < MinimumNumberOfPlayersPerTeam)
				{
					hadEnoughEligiblePlayers = false;
					break;
				}
			}

			if (hadEnoughEligiblePlayers == false)
				return false;

			return true;
		}

		private bool IsDrawGame(DataAccess.Game game)
		{
			bool isDrawGame = true;
			foreach (DataAccess.GameTeam team in game.GameTeams)
			{
				if (team.GameTeamWinner == true)
				{
					isDrawGame = false;
					break;
				}
			}

			return isDrawGame;
		}

		private void UpdateLeaderboard(int gameIdentID)
		{
			using (DataAccess.CSSDataContext db = new Allegiance.CommunitySecuritySystem.DataAccess.CSSDataContext())
			{
				using (DataAccess.CSSStatsDataContext statsDB = new Allegiance.CommunitySecuritySystem.DataAccess.CSSStatsDataContext())
				{
					var game = statsDB.Games.FirstOrDefault(p => p.GameIdentID == gameIdentID);

					if (game == null)
						throw new Exception("Couldn't get game for ID: " + gameIdentID);

					if (IsGameEligibleForLogging(game) == false)
						return;

					bool isDrawGame = IsDrawGame(game);

					foreach (DataAccess.GameTeam team in game.GameTeams)
					{
						var commanderAlias = DataAccess.Alias.GetAliasByCallsign(db, team.GameTeamCommander);

						foreach (DataAccess.GameTeamMember teamMember in team.GameTeamMembers)
						{
							int shipKills = 0;
							int stationKills = 0;
							int stationCaptures = 0;
							int droneKills = 0;
							int ejects = 0;
							int defection = 0;
                            int score = 0;

							var alias = DataAccess.Alias.GetAliasByCallsign(db, teamMember.GameTeamMemberCallsign);

							if (alias == null)
								continue;

							// Check current team member for defections.
							foreach (DataAccess.GameTeam otherTeam in game.GameTeams)
							{
								if (otherTeam.GameTeamID == team.GameTeamID)
									continue;

								foreach (DataAccess.GameTeamMember otherTeamMember in otherTeam.GameTeamMembers)
								{
									var otherLogin = DataAccess.Alias.GetAliasByCallsign(db, otherTeamMember.GameTeamMemberCallsign);

									if (otherLogin != null && otherLogin.LoginId == alias.LoginId)
									{
										defection = 1;
										break;
									}
								}

								if (defection != 0)
									break;
							}

							var primaryMemberEvents = game.GameEvents.Where
							(
								p => p.GameEventPerformerID != 1
								&&
								(
									p.GameEventPerformerLoginID == alias.Login.Id
									&& (
											p.EventID == (int)AllegianceEventIDs.StationDestroyed
											|| p.EventID == (int)AllegianceEventIDs.StationCaptured
											|| p.EventID == (int)AllegianceEventIDs.ShipKilled
										)
								)
								||
								(
									p.GameEventTargetLoginID == alias.Login.Id
									&& p.EventID == (int)AllegianceEventIDs.ShipKilled
								)
							);

							foreach (var primaryMemberEvent in primaryMemberEvents)
							{
								switch (primaryMemberEvent.EventID)
								{
									case (int)Common.Enumerations.AllegianceEventIDs.ShipKilled:
                                        if (primaryMemberEvent.GameEventPerformerName.StartsWith(".") == true)
                                        {
                                            droneKills++;
                                            score += DroneKillPoints;
                                        }
                                        else
                                        {
                                            if (primaryMemberEvent.GameEventPerformerLoginID == teamMember.GameTeamMemberLoginID)
                                            {
                                                ejects++;
                                            }
                                            else
                                            {
                                                shipKills++;
                                                score += ShipKillPoints;
                                            }
                                        }
										break;

                                    case (int)Common.Enumerations.AllegianceEventIDs.StationCaptured:
                                        {
                                            stationCaptures++;
                                            score += StationCapturePoints;
                                        }
                                        break;

                                    case (int)Common.Enumerations.AllegianceEventIDs.StationDestroyed:
                                        {
                                            stationKills++;
                                            score += StationKillPoints;
                                        }
                                        break;
								}
							}

							if (alias != null)
							{
								var leaderboard = statsDB.StatsLeaderboards.FirstOrDefault(p => p.LoginID == alias.Login.Id);

								if (leaderboard == null)
								{
									leaderboard = new Allegiance.CommunitySecuritySystem.DataAccess.StatsLeaderboard()
									{
										CommandDraws = 0, // Value filled in by ASGSServiceUpdateASRankings stored proc during AllegSkill calculation.
										CommandLosses = 0, // Value filled in by ASGSServiceUpdateASRankings stored proc during AllegSkill calculation.
										CommandMu = 25D,
										CommandRank = 0, // Value filled in by ASGSServiceUpdateASRankings stored proc during AllegSkill calculation.
										CommandSigma = 25D / 3D,
										CommandWins = 0, // Value filled in by ASGSServiceUpdateASRankings stored proc during AllegSkill calculation.
										Defects = 0, // Value filled in by ASGSServiceUpdateASRankings stored proc during AllegSkill calculation.
										Draws = 0, // Value filled in by ASGSServiceUpdateASRankings stored proc during AllegSkill calculation.
										DroneKills = 0, // Value filled in by ASGSServiceUpdateASRankings stored proc during AllegSkill calculation.
										Ejects = 0, // Value filled in by ASGSServiceUpdateASRankings stored proc during AllegSkill calculation.
										HoursPlayed = 0, // Value filled in by ASGSServiceUpdateASRankings stored proc during AllegSkill calculation.
										Kills = 0, // Value filled in by ASGSServiceUpdateASRankings stored proc during AllegSkill calculation.
										LoginUsername = alias.Login.Username,
										LoginID = alias.Login.Id,
										Losses = 0, // Value filled in by ASGSServiceUpdateASRankings stored proc during AllegSkill calculation.
										Mu = 25D,
										Rank = 0, // Value filled in by ASGSServiceUpdateASRankings stored proc during AllegSkill calculation.
										Sigma = 25D / 3D,
										StackRating = 0, // Value filled in by ASGSServiceUpdateASRankings stored proc during AllegSkill calculation.
										StationCaptures = 0, // Value filled in by ASGSServiceUpdateASRankings stored proc during AllegSkill calculation.
										StationKills = 0, // Value filled in by ASGSServiceUpdateASRankings stored proc during AllegSkill calculation.
										Wins = 0, // Value filled in by ASGSServiceUpdateASRankings stored proc during AllegSkill calculation.
										DateModified = DateTime.Now



										//CommandDraws = commanderAlias.LoginId == alias.LoginId && isDrawGame == true && defection == 0 ? 1 : 0,
										//CommandLosses = commanderAlias.LoginId == alias.LoginId && team.GameTeamWinner == false && isDrawGame == false && defection == 0 ? 1 : 0,
										//CommandMu = 25D, 
										//CommandRank = 0, 
										//CommandSigma = 25D / 3D, 
										//CommandWins = commanderAlias.LoginId == alias.LoginId && team.GameTeamWinner == true && defection == 0 ? 1 : 0,
										//Defects = defection,
										//Draws = isDrawGame == true && defection == 0 ? 1 : 0,
										//DroneKills = droneKills,
										//Ejects = ejects,
										//HoursPlayed = teamMember.GameTeamMemberDuration / 60.0 / 60.0,
										//Kills = shipKills,
										//LoginUsername = alias.Login.Username,
										//LoginID = alias.Login.Id,
										//Losses = team.GameTeamWinner == false && isDrawGame == false && defection == 0 ? 1 : 0,
										//Mu = 25D, 
										//Rank = 0, 
										//Sigma = 25D / 3D, 
										//StackRating = 0, 
										//StationCaptures = stationCaptures,
										//StationKills = stationKills,
										//Wins = team.GameTeamWinner == true && defection == 0 ? 1 : 0,
										//DateModified = DateTime.Now
									};

									statsDB.StatsLeaderboards.InsertOnSubmit(leaderboard);
								}
								else
								{
									// These values are all set by ASGSServiceUpdateASRankings stored proc during AllegSkill calculation.

									//leaderboard.CommandDraws += commanderAlias.LoginId == alias.LoginId && isDrawGame == true && defection == 0 ? 1 : 0;
									//leaderboard.CommandLosses += commanderAlias.LoginId == alias.LoginId && team.GameTeamWinner == false && isDrawGame == false && defection == 0 ? 1 : 0;
									//leaderboard.CommandWins += commanderAlias.LoginId == alias.LoginId && team.GameTeamWinner == true && defection == 0 ? 1 : 0;
									//leaderboard.Defects += defection;
									//leaderboard.Draws += isDrawGame == true && defection == 0 ? 1 : 0;
									//leaderboard.DroneKills += droneKills;
									//leaderboard.Ejects += ejects;
									//leaderboard.HoursPlayed += teamMember.GameTeamMemberDuration / 60.0 / 60.0;
									//leaderboard.Kills += shipKills;
									//leaderboard.Losses += team.GameTeamWinner == false && isDrawGame == false && defection == 0 ? 1 : 0;
									//leaderboard.StationCaptures += stationCaptures;
									//leaderboard.StationKills += stationKills;
									//leaderboard.Wins += team.GameTeamWinner == true && defection == 0 ? 1 : 0;
									//leaderboard.DateModified = DateTime.Now;
								}

                                teamMember.Score = score;

								statsDB.SubmitChanges();
								db.SubmitChanges();
							}
						}
					}

				}
			}
		}

		private void UpdateFactionStats(int gameIdentID)
		{
			using (DataAccess.CSSStatsDataContext db = new Allegiance.CommunitySecuritySystem.DataAccess.CSSStatsDataContext())
			{
				var game = db.Games.FirstOrDefault(p => p.GameIdentID == gameIdentID);

				if (game == null)
					throw new Exception("Couldn't get game for ID: " + gameIdentID);

				if (IsGameEligibleForLogging(game) == false)
					return;

				if (IsDrawGame(game) == true)
					return;

				if(game.GameTeams.Count != 2)
					return;

				DataAccess.GameTeam winningTeam;
				DataAccess.GameTeam losingTeam;

				if(game.GameTeams[0].GameTeamWinner == true)
				{
					winningTeam = game.GameTeams[0];
					losingTeam = game.GameTeams[1];
				}
				else
				{
					winningTeam = game.GameTeams[1];
					losingTeam = game.GameTeams[0];
				}

				DataAccess.StatsFaction statsFaction = db.StatsFactions.FirstOrDefault(p =>
						p.WinFactionName == winningTeam.GameTeamFaction
					&&	p.WinExpansion == winningTeam.GameTeamExpansion
					&&	p.WinShipyard == winningTeam.GameTeamShipyard
					&&	p.WinStarbase == winningTeam.GameTeamStarbase
					&&	p.WinSupremacy == winningTeam.GameTeamSupremacy
					&&	p.WinTactical == winningTeam.GameTeamTactical
					&&	p.LossFactionName == losingTeam.GameTeamFaction
					&&	p.LossExpansion == losingTeam.GameTeamExpansion
					&&	p.LossShipyard == losingTeam.GameTeamShipyard
					&&	p.LossStarbase == losingTeam.GameTeamStarbase
					&&	p.LossSupremacy == losingTeam.GameTeamSupremacy
					&&	p.LossTactical == losingTeam.GameTeamTactical
					);

				if(statsFaction == null)
				{
					statsFaction = new Allegiance.CommunitySecuritySystem.DataAccess.StatsFaction()
					{
						GamesPlayed = 1,
						HoursPlayed = game.GameEndTime.Subtract(game.GameStartTime).TotalMinutes / 60,
						LossExpansion = losingTeam.GameTeamExpansion,
						LossFactionName = losingTeam.GameTeamFaction,
						LossShipyard = losingTeam.GameTeamShipyard,
						LossStarbase = losingTeam.GameTeamStarbase,
						LossSupremacy = losingTeam.GameTeamSupremacy,
						LossTactical = losingTeam.GameTeamTactical,
						WinExpansion = winningTeam.GameTeamExpansion,
						WinFactionName = winningTeam.GameTeamFaction,
						WinShipyard = winningTeam.GameTeamShipyard,
						WinStarbase = winningTeam.GameTeamStarbase,
						WinSupremacy = winningTeam.GameTeamSupremacy,
						WinTactical = winningTeam.GameTeamTactical,
						DateModified = DateTime.Now
					};

					db.StatsFactions.InsertOnSubmit(statsFaction);
				}
				else
				{
					statsFaction.GamesPlayed++;
					statsFaction.HoursPlayed += game.GameEndTime.Subtract(game.GameStartTime).TotalMinutes / 60;
					statsFaction.DateModified = DateTime.Now;
				}

				db.SubmitChanges();
			}
		}


		private void UpdateMetrics(int gameIdentID)
		{
			using (DataAccess.CSSStatsDataContext statsDB = new Allegiance.CommunitySecuritySystem.DataAccess.CSSStatsDataContext())
			{
				var game = statsDB.Games.FirstOrDefault(p => p.GameIdentID == gameIdentID);

				if (game == null)
					throw new Exception("Couldn't get game for ID: " + gameIdentID);

				if (IsGameEligibleForLogging(game) == false)
					return;

				DataAccess.StatsMetric statsMetric = statsDB.StatsMetrics.FirstOrDefault();

				var averages = statsDB.StatsLeaderboards
					.Where(p => p.DateModified > DateTime.Now.AddDays(-1 * Common.Constants.Leaderboard.MaxLastActiveDays))
					.GroupBy(p => p)
					.Select(p => new 
					{ 
						CommandRank = p.Average(x => x.CommandRank), 
						Rank = p.Average(x => x.Rank) 
					})
					.FirstOrDefault();

				double averageCommandRank = 0;
				double averageRank = 0;

				if (averages != null)
				{
					averageCommandRank = averages.CommandRank;
					averageRank = averages.Rank;
				}

				if (statsMetric == null)
				{
					statsMetric = new Allegiance.CommunitySecuritySystem.DataAccess.StatsMetric()
					{
						AverageCommandRank = averageCommandRank,
						AveragePlayerRank = averageRank,
						DateModified = DateTime.Now,
						LastGameProcessed = gameIdentID,
						TotalGamesLogged = 1
					};

					statsDB.StatsMetrics.InsertOnSubmit(statsMetric);
				}
				else
				{
					statsMetric.AverageCommandRank = averageCommandRank;
					statsMetric.AveragePlayerRank = averageRank;
					statsMetric.DateModified = DateTime.Now;
					statsMetric.LastGameProcessed = gameIdentID;
					statsMetric.TotalGamesLogged++;
				}

				statsDB.SubmitChanges();

			}
		}



		#endregion
	}
}
