using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Allegiance.CommunitySecuritySystem.Management.Stats
{
	public partial class Faction : UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Master.PageHeader = "CSS - Faction Statistics";

			if (this.IsPostBack == false)
				BindData();
		}

		private void BindData()
		{
			using (var db = new DataAccess.CSSStatsDataContext())
			{
				var allFactionNames = db.StatsFactions
					.Select(p => new { FactionName = p.WinFactionName } )
					.Union
					(
						db.StatsFactions.Select(p => new { FactionName = p.LossFactionName} )
					).Distinct().OrderBy(p => p.FactionName);

				List<Data.FactionData> allFactionData = new List<Allegiance.CommunitySecuritySystem.Management.Stats.Data.FactionData>();

				foreach (var winFactionName in allFactionNames)
				{
					Data.FactionData factionData = new Allegiance.CommunitySecuritySystem.Management.Stats.Data.FactionData();
					factionData.Name = winFactionName.FactionName;
					factionData.Stats = String.Empty;

					int index = 0;
					foreach (var lossFactionName in allFactionNames)
					{
						if (winFactionName.FactionName.Equals(lossFactionName.FactionName, StringComparison.InvariantCultureIgnoreCase) == true)
						{
							factionData.Stats += "<td>--</td>";
						}
						else
						{
							var winTotal = db.StatsFactions
								.Where(p => p.WinFactionName == winFactionName.FactionName && p.LossFactionName == lossFactionName.FactionName)
								.GroupBy(p => new { p.WinFactionName, p.LossFactionName })
								.Select(p => p.Sum(r => r.GamesPlayed) ).FirstOrDefault();

							var lossTotal = db.StatsFactions
								.Where(p => p.WinFactionName == lossFactionName.FactionName && p.LossFactionName == winFactionName.FactionName)
								.GroupBy(p => new { p.WinFactionName, p.LossFactionName })
								.Select(p => p.Sum(r => r.GamesPlayed)).FirstOrDefault();

							if(winTotal + lossTotal == 0)
								factionData.Stats += "<td>--</td>";
							else
								factionData.Stats += String.Format("<td>{0:p}</td>", (decimal) winTotal / (lossTotal + winTotal));
						}

						index++;
					}

					allFactionData.Add(factionData);
				}

				rptFactionHeaders.DataSource = allFactionData;
				rptFactionHeaders.DataBind();

				rptFactionRow.DataSource = allFactionData;
				rptFactionRow.DataBind();
			}
		}
	}
}
