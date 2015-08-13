using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Allegiance.CommunitySecuritySystem.Management.Stats
{
	public partial class Leaderboard : UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Master.PageHeader = "CSS - Leaderboard";
			//Master.UseFullWidth = true;

			if (this.IsPostBack == false)
				BindData();
		}

		private void BindData()
		{
			using (DataAccess.CSSStatsDataContext statsDB = new Allegiance.CommunitySecuritySystem.DataAccess.CSSStatsDataContext())
			{
				//var sortedLeaderboard = statsDB.StatsLeaderboards
				//    .Where(p => p.DateModified > DateTime.Now.AddDays(Common.Constants.Leaderboard.MaxLastActiveDays))
				//    .OrderByDescending(p => p.Rank)
				//    .ToList()
				//    .Select((p, index) => new 
				//    { 
				//        Order = index + 1, 
				//        Place = index + 1, 
				//        Callsign = p.LoginUsername,
				//        Mu = p.Mu.ToString("F2"),
				//        Sigma = p.Sigma.ToString("F2"),
				//        Rank = p.Rank.ToString("F1"),
				//        Wins = p.Wins,
				//        Losses = p.Losses,
				//        Draws = p.Draws,
				//        Defects = p.Defects,
				//        StackRating = p.StackRating.ToString("F2"),
				//        CommandMu = p.CommandMu.ToString("F2"),
				//        CommandSigma = p.CommandSigma.ToString("F2"),
				//        CommandRank = p.CommandRank.ToString("F1"),
				//        CommandWins = p.CommandWins,
				//        CommandLosses = p.CommandLosses,
				//        CommandDraws = p.CommandDraws,
				//        Kills = p.Kills,
				//        Ejects = p.Ejects,
				//        DroneKills = p.DroneKills,
				//        StationKills = p.StationKills,
				//        StationCaptures = p.StationCaptures,
				//        KillsEjectsRatio = p.Kills > 0 && p.Ejects > 0 ? Math.Round((Double)p.Kills / p.Ejects, 2) : p.Kills > 0 ? 1 : 0,
				//        HoursPlayed = Math.Round(p.HoursPlayed, 2),
				//        KillsPerHour = p.HoursPlayed == 0 ? 0 : Math.Round((Double)p.Kills / p.HoursPlayed, 2)
				//    });

				gvLeaderboard.DataSource = DataAccess.StatsLeaderboard.GetSortedLeaderboard(statsDB);
				gvLeaderboard.DataBind();
			}
		}
	}
}
