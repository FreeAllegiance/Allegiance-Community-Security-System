using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Allegiance.CommunitySecuritySystem.Management.Stats
{
	public partial class SquadRoster : UI.Page
	{
		protected List<Data.SquadData> SquadList = new List<Data.SquadData>();


		protected void Page_Load(object sender, EventArgs e)
		{
			Master.PageHeader = "CSS - Squad Rosters";
			//Master.UseFullWidth = true;

			if (this.IsPostBack == false)
				BindData();
		}

		private void BindData()
		{
			using (DataAccess.CSSDataContext db = new Allegiance.CommunitySecuritySystem.DataAccess.CSSDataContext())
			{
				var allSquads = db.Groups.Where(p => p.IsSquad == true).OrderBy(p => p.Name);

				List<Data.SquadData> squadDatas = new List<Allegiance.CommunitySecuritySystem.Management.Stats.Data.SquadData>();
				foreach (var squad in allSquads)
				{
					Data.SquadData squadData = new Allegiance.CommunitySecuritySystem.Management.Stats.Data.SquadData();
					squadData.SquadName = squad.Name;

					foreach (var gagr in squad.Group_Alias_GroupRoles)
					{
						squadData.Members.Add(new Data.MemberData()
						{
							Token = gagr.GroupRole.Token.ToString(),
							Callsign = gagr.Alias.Callsign, 
							IsActive = gagr.Alias.Login.Identity.DateLastLogin > DateTime.Now.AddDays(-30)
						});
					}

					//squadData.Members.OrderBy(p => p.IsActive).ThenBy(p => p.Token);

					if(squadData.Members.Count > 0)
						squadDatas.Add(squadData);
				}

				SquadList = squadDatas;

				rptHeaders.DataSource = squadDatas.ToArray();
				rptHeaders.DataBind();

				rptSquads.DataSource = squadDatas.ToArray();
				rptSquads.DataBind();

				rptFooter.DataSource = squadDatas.ToArray();
				rptFooter.DataBind();
			}
		}

		protected void rptSquads_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				Data.SquadData squadData = (Data.SquadData) e.Item.DataItem;
				Repeater rptMembers = (Repeater) e.Item.FindControl("rptMembers");

				rptMembers.DataSource = squadData.Members.OrderByDescending(p => p.IsActive).ThenBy(p => p.TokenValue).ThenBy(p => p.Callsign);
				rptMembers.DataBind();
			}
		}
	}
}
