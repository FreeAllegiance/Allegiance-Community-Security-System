using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Allegiance.CommunitySecuritySystem.DataAccess;

namespace Allegiance.CommunitySecuritySystem.Server
{
	[DataContract]
	public class CommitPlayerDataResponse
	{
		[DataMember]
		public bool Succeeded { get; set; }
		[DataMember]
		public string ErrorMessage { get; set; }

		public CommitPlayerDataResponse() { }

		public CommitPlayerDataResponse(CommitPlayerDataRequest request)
		{
			ErrorMessage = String.Empty;
			Succeeded = true;

			using (CSSStatsDataContext statsDB = new CSSStatsDataContext())
			{
				using(CSSDataContext db = new CSSDataContext())
				{
					foreach (var scoreQueue in statsDB.ScoreQueues.Where(p => p.GameGuid == request.GameGuid))
					{
						var login = db.Logins.FirstOrDefault(p => p.Id == scoreQueue.LoginId);
						if(login == null)
						{
							Succeeded = false;
							ErrorMessage += "Couldn't find login for login id: " + scoreQueue.LoginId;
							continue;
						}

						string callsign = login.Username;

						var primaryAlias = login.Aliases.FirstOrDefault(p => p.IsDefault == true);
						if(primaryAlias == null)
							primaryAlias = login.Aliases.FirstOrDefault();

						if(primaryAlias != null)
							callsign = primaryAlias.Callsign;

						StatsLeaderboard leaderBoard = statsDB.StatsLeaderboards.FirstOrDefault(p => p.LoginID == scoreQueue.LoginId);

						if (leaderBoard == null)
						{
							leaderBoard = new StatsLeaderboard()
							{
								CommandDraws = 0,
								CommandLosses = 0,
								CommandMu = 0, 
								CommandRank = 0,
								CommandSigma = 0,
								CommandWins = 0,
								DateModified = DateTime.Now,
								Defects = 0,
								Draws = 0,
								DroneKills = 0,
								Ejects = 0,
								HoursPlayed = 0,
								Kills = 0,
								LoginID = scoreQueue.LoginId,
								LoginUsername = login.Username,
								Losses = 0,
								Mu = 0,
								PRank = 0,
								Rank = 0,
								Sigma = 0,
								StationCaptures = 0,
								StationKills = 0,
								Wins = 0,
								Xp = 0
							};
							
							statsDB.StatsLeaderboards.InsertOnSubmit(leaderBoard);
						}

						// Add in the new values.
						leaderBoard.CommandDraws += (scoreQueue.CommandCredit == true && scoreQueue.CommandWin == false && scoreQueue.CommandLose == false) ? 1 : 0;
						leaderBoard.CommandLosses += (scoreQueue.CommandCredit == true && scoreQueue.CommandWin == false && scoreQueue.CommandLose == true) ? 1 : 0;
						
						//leaderBoard.CommandMu = 0; 
						//leaderBoard.CommandRank = 0;
						//leaderBoard.CommandSigma = 0;
						//leaderBoard.Mu = 0;
						//leaderBoard.Sigma = 0;

						leaderBoard.CommandWins += (scoreQueue.CommandCredit == true && scoreQueue.CommandWin == true && scoreQueue.CommandLose == false) ? 1 : 0;
						leaderBoard.DateModified = DateTime.Now;
						//leaderBoard.Defects = 0;
						leaderBoard.Draws += (scoreQueue.Win == false && scoreQueue.Lose == false) ? 1 : 0;
						leaderBoard.DroneKills += (int) Math.Round(scoreQueue.BuilderKills + scoreQueue.CarrierKills + scoreQueue.LayerKills + scoreQueue.MinerKills);
						leaderBoard.Ejects += scoreQueue.Deaths;
						leaderBoard.HoursPlayed += scoreQueue.TimePlayed.GetValueOrDefault(0) / 3600D;
						leaderBoard.Kills += (int) Math.Round(scoreQueue.PlayerKills);
						leaderBoard.Losses += (scoreQueue.Win == false && scoreQueue.Lose == true) ? 1 : 0;
						leaderBoard.StationCaptures += scoreQueue.PilotBaseCaptures + (int) Math.Round(scoreQueue.BaseCaptures);
						leaderBoard.StationKills += scoreQueue.PilotBaseKills + (int) Math.Round(scoreQueue.BaseKills);
						leaderBoard.Wins += (scoreQueue.Win == true && scoreQueue.Lose == false) ? 1 : 0;
						leaderBoard.Xp += (int) Math.Round(scoreQueue.Score);
						leaderBoard.PRank = 0;
						leaderBoard.Rank = GetLevel(statsDB, leaderBoard.Xp);

						statsDB.ScoreQueues.DeleteOnSubmit(scoreQueue);

						statsDB.SubmitChanges();
					}
				}
			}

			this.Succeeded = true;
		}

		private int GetLevel(CSSStatsDataContext statsDB, int playerXp)
		{
			var level = statsDB.Levels.FirstOrDefault(p => p.MinXP <= playerXp && p.MaxXP >= playerXp);
			if (level == null)
				return 0;

			return level.Level1;
		}
	}
}
