using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Allegiance.CommunitySecuritySystem.Management.Stats
{
	public partial class LeaderboardData : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Response.ContentType = "text/javascript";
		}

		protected override void Render(HtmlTextWriter writer)
		{
			using (DataAccess.CSSStatsDataContext db = new Allegiance.CommunitySecuritySystem.DataAccess.CSSStatsDataContext())
			{
				var sortedLeaderboard = db.StatsLeaderboards.Where(p => p.DateModified > DateTime.Now.AddDays(-1 * Common.Constants.Leaderboard.MaxLastActiveDays)).OrderBy(p => p.Rank).ToList().Select((p, index) => new
				{
					Order = index, 
					Place = index, 
					Callsign = p.LoginUsername }
					);

				writer.Write("{ total: \"1\", page: \"0\", records: \"" + sortedLeaderboard.Count() + "\", rows : [");

				bool firstRow = true;
				foreach (var row in sortedLeaderboard)
				{
					if(firstRow == true)
						firstRow = false;
					else
						writer.Write(",");

					writer.Write("{id:\"" + row.Order + "\", cell:[\"" + row.Order + "\",\"" + row.Place + "\",\"" + row.Callsign + "\"]}");
				}

				writer.Write("]}");
			}
		}
	}
}
