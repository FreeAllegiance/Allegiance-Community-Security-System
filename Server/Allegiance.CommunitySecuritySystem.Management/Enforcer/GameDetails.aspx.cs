using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Allegiance.CommunitySecuritySystem.DataAccess;

namespace Allegiance.CommunitySecuritySystem.Management.Enforcer
{
	public partial class GameDetails : UI.Page
	{
		private int? _gameIdentID = null;
		private int GameIdentID
		{
			get
			{
				if(_gameIdentID == null)
				{
					_gameIdentID = Int32.Parse(Request.Params["gameIdentID"]);
				}

				return _gameIdentID.GetValueOrDefault(0);
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Business.Authorization.IsZoneLeadOrAdminOrSuperAdmin(User) == false)
				Response.Redirect("~/Default.aspx");

			Master.PageHeader = "CSS - Game Details";

			if (this.IsPostBack == false)
				BindData();
		}

		private void BindData()
		{
			using (var db = new CSSStatsDataContext())
			{
				var game = db.Games.Where(p => p.GameIdentID == GameIdentID);
				gvGame.DataSource = game;
				gvGame.DataBind();

				var teams = db.GameTeams.Where(p => p.GameID == GameIdentID);
				rptTeams.DataSource = teams;
				rptTeams.DataBind();

				var chatLog = db.GameChatLogs
					.Where(p => p.GameID == GameIdentID)
					.OrderBy(p => p.GameChatTime)
					.Select(p => new
					{
						Time = p.GameChatTime,
						Speaker = p.GameChatSpeakerName,
						Target = p.GameChatTargetName,
						Text = p.GameChatText
					});

				gvChatLog.DataSource = chatLog;
				gvChatLog.DataBind();
			}
		}

		protected void rptTeams_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			DataAccess.GameTeam gameTeam = (DataAccess.GameTeam)e.Item.DataItem;

			var gvTeam = (GridView) e.Item.FindControl("gvTeam");

			using (var db = new CSSStatsDataContext())
			{
				var teamMembers = db.GameTeamMembers
					.Where(p => p.GameTeamID == gameTeam.GameTeamIdentID)
					.Select(p => new
					{
						Callsign = p.GameTeamMemberCallsign,
						Time = string.Format("{0:hh\\:mm\\:ss}", new TimeSpan(0, 0, p.GameTeamMemberDuration))
					});

				gvTeam.DataSource = teamMembers;
				gvTeam.DataBind();
			}
			
		}
	}
}