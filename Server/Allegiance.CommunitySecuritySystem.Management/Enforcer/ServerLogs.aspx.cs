using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Allegiance.CommunitySecuritySystem.DataAccess;
using System.Data.Linq.SqlClient;

namespace Allegiance.CommunitySecuritySystem.Management.Enforcer
{
	public partial class ServerLogs : UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (Business.Authorization.IsZoneLeadOrAdminOrSuperAdmin(User) == false)
				Response.Redirect("~/Default.aspx");

			Master.PageHeader = "CSS - Game Logs";

			if (this.IsPostBack == false)
				BindData();
		}

		private void BindData()
		{
			if (String.IsNullOrEmpty(txtSearch.Text) == true)
				txtSearch.Text = Request.Params["searchText"];

			string searchText = txtSearch.Text;
			if (searchText.Contains("%") == false)
				searchText = "%" + searchText + "%";

			using (var db = new CSSStatsDataContext())
			{
				var games = db.Games
					.Where(p => searchText.Length == 0 || 
							db.GameChatLogs
								.Where(
											r => r.GameChatTime > DateTime.Now.AddMonths(-3) 
											&& 
												(
													SqlMethods.Like(r.GameChatText, searchText)
													|| SqlMethods.Like(r.GameChatSpeakerName, searchText)
													|| SqlMethods.Like(r.GameChatTargetName, searchText)
												)
										)
								.Select(r => r.GameID)
							.Contains(p.GameIdentID))
					.Join(db.GameServers, p => p.GameServer, r => r.GameServerID, (p, r) => new 
						{ 
							GameID = p.GameID, 
							GameServer = r.GameServerName,
							GameName = p.GameName,
							GameStartTime = p.GameStartTime, //.ToString("mm/dd/yy HH:MM")
							GameIdentID = p.GameIdentID
						})
					.OrderByDescending(p => p.GameStartTime);

				gvGames.DataSource = games;
				gvGames.DataBind();
			}
		}

		protected void txtSearch_TextChanged(object sender, EventArgs e)
		{
			BindData();
		}
	}
}