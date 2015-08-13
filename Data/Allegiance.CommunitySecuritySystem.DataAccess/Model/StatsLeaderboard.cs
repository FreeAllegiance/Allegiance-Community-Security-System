using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegiance.CommunitySecuritySystem.DataAccess
{
	public partial class StatsLeaderboard
	{
		public int Order { get; set; }
		public int Place { get; set; }
		public string Callsign
		{
			get
			{
				return LoginUsername;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public double KillsEjectsRatio 
		{
			get
			{
				return Kills > 0 && Ejects > 0 ? Math.Round((Double)Kills / Ejects, 2) : Kills > 0 ? 1 : 0;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public double KillsPerHour
		{
			get
			{
				return HoursPlayed == 0 ? 0 : Math.Round((Double)Kills / HoursPlayed, 2);
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public string MuString
		{
			get
			{
				return Mu.ToString("F2");
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public string SigmaString
		{
			get
			{
				return Sigma.ToString("F2");
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public string RankString
		{
			get
			{
				return Rank.ToString("F1");
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public string StackRatingString
		{
			get
			{
				return StackRating.ToString("F2");
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public string CommandMuString
		{
			get
			{
				return CommandMu.ToString("F2");
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public string CommandSigmaString
		{
			get
			{
				return CommandSigma.ToString("F2");
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public string CommandRankString
		{
			get
			{
				return CommandRank.ToString("F1");
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public string HoursPlayedString
		{
			get
			{
				return Math.Round(HoursPlayed, 2).ToString();
			}
			set
			{
				throw new NotImplementedException();
			}
		}


		public static List<StatsLeaderboard> GetSortedLeaderboard(CSSStatsDataContext statsDB)
		{
			List<StatsLeaderboard> sortedLeaderboard = statsDB.StatsLeaderboards
					.Where(p => p.DateModified > DateTime.Now.AddDays(-1 * Common.Constants.Leaderboard.MaxLastActiveDays))
					.OrderByDescending(p => p.Rank)
					.ToList();

			int counter = 1;
			foreach (var entry in sortedLeaderboard)
			{
				entry.Order = counter;
				entry.Place = counter;
				counter++;
			}

					//Select((p, index) => new StatsLeaderboard()
					//{
					//    Order = index + 1,
					//    Place = index + 1,
					//    Callsign = p.LoginUsername,
					//    Mu = p.Mu.ToString("F2"),
					//    Sigma = p.Sigma.ToString("F2"),
					//    Rank = p.Rank.ToString("F1"),
					//    Wins = p.Wins,
					//    Losses = p.Losses,
					//    Draws = p.Draws,
					//    Defects = p.Defects,
					//    StackRating = p.StackRating.ToString("F2"),
					//    CommandMu = p.CommandMu.ToString("F2"),
					//    CommandSigma = p.CommandSigma.ToString("F2"),
					//    CommandRank = p.CommandRank.ToString("F1"),
					//    CommandWins = p.CommandWins,
					//    CommandLosses = p.CommandLosses,
					//    CommandDraws = p.CommandDraws,
					//    Kills = p.Kills,
					//    Ejects = p.Ejects,
					//    DroneKills = p.DroneKills,
					//    StationKills = p.StationKills,
					//    StationCaptures = p.StationCaptures,
					//    HoursPlayed = Math.Round(p.HoursPlayed, 2)
					//}).ToList();

	
			return sortedLeaderboard;
		}
	}
}
