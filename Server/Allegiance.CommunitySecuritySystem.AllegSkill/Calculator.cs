using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegiance.CommunitySecuritySystem.AllegSkill
{
	public class Calculator
	{
		public static void UpdateAllegSkillForGame(int gameID)
		{
			//using(DataAccess.CSSDataContext db = new DataAccess.CSSDataContext())
			//{
				using (DataAccess.CSSStatsDataContext statsDB = new DataAccess.CSSStatsDataContext())
				{
					statsDB.ASGSServiceUpdateASRankings(gameID, 0);

				//	var allegSkillGamePlayerStats = statsDB.AS_GamePlayerAs.Where(p => p.GameID == gameID);

				//    var game = statsDB.Games.FirstOrDefault(p => p.GameID == gameID);

				//    // AllegSkill only supports two teams currently.
				//    if (game.GameTeams.Count() != 2)
				//        return;

				//    foreach (DataAccess.GameTeam team in game.GameTeams)
				//    {
				//        foreach (DataAccess.GameTeamMember teamMember in team.GameTeamMembers)
				//        {

				//            DataAccess.Alias alias = DataAccess.Alias.GetAliasByCallsign(db, teamMember.GameTeamMemeberCallsign);
				//            if (alias != null)
				//            {
				//                float mu;
				//                float sigma;
				//                float commandMu;
				//                float commandSigma;
				//                float stackRating;
				//                int rank;
				//                int commandRank;

				//                // TODO: Baker: perform AllegSkill calculations for each team member.
				//                // ---- Just some dummy code for now. -----

				//                Random random = new Random();

				//                mu = random.Next(20, 30);
				//                sigma = random.Next(-5, 5);
				//                commandMu = random.Next(20, 30);
				//                commandSigma = random.Next(-5, 5);
				//                stackRating = random.Next(-10, 10);
				//                rank = random.Next(0, 20);
				//                commandRank = random.Next(0, 20);

				//                // ---- End dummy code ----

				//                // Save the finished calculaitons.
				//                using (DataAccess.CSSStatsDataContext statsDB = new Allegiance.CommunitySecuritySystem.DataAccess.CSSStatsDataContext())
				//                {
				//                    DataAccess.StatsLeaderboard leaderboard = statsDB.StatsLeaderboards.FirstOrDefault(p => p.LoginUsername == alias.Login.Username);
				//                    if (leaderboard != null)
				//                    {
				//                        leaderboard.CommandMu = commandMu;
				//                        leaderboard.CommandSigma = commandSigma;
				//                        leaderboard.Mu = mu;
				//                        leaderboard.Sigma = sigma;
				//                        leaderboard.StackRating = stackRating;
				//                        leaderboard.Rank = rank;
				//                        leaderboard.CommandRank = commandRank;
				//                    }

				//                    statsDB.SubmitChanges();
				//                }
				//            }
				//        }
				//    }

				//    db.SubmitChanges();
				//}
			}
		}
	}
}
