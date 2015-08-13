using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Allegiance.CommunitySecuritySystem.Management.Stats
{
	public partial class BanList : UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Master.PageHeader = "CSS - Ban List";

			if (this.IsPostBack == false)
				BindData();
		}

		private void BindData()
		{
			using (DataAccess.CSSDataContext db = new Allegiance.CommunitySecuritySystem.DataAccess.CSSDataContext())
			{
				string banListType = Request.Params["type"] ?? String.Empty;

				if (banListType.Equals("mostRecent", StringComparison.CurrentCultureIgnoreCase) == true)
				{
					gvBanList.DataSource = db.Bans
						.OrderByDescending(p => p.DateCreated)
						.Take(20)
						.Select(p => new Data.BanData()
					{
						Username = String.IsNullOrEmpty(p.Alias.Callsign) ? p.Login.Username : p.Alias.Callsign,
						BannedBy = p.BanningLogin.Username,
						Reason = p.Reason == null ? (p.BanTypeId != null ? p.BanType.Description : "") : p.Reason,
						DateCreated = p.DateCreated.ToString(),
						Duration = FormatTimespan(p.DateCreated, p.DateExpires),
						TimeLeft = FormatTimeLeft(p.DateCreated, p.DateExpires, p.InEffect)
					});
				}
				else
				{
					gvBanList.DataSource = db.Bans
						.Where(p => p.DateExpires > DateTime.Now && p.InEffect == true)
						.OrderByDescending(p => DateTime.Now - p.DateExpires)
						.Take(20)
						.Select(p => new Data.BanData()
					{
						Username = String.IsNullOrEmpty(p.Alias.Callsign) ? p.Login.Username : p.Alias.Callsign,
						BannedBy = p.BanningLogin.Username,
						Reason = p.Reason == null ? (p.BanTypeId != null ? p.BanType.Description : "") : p.Reason,
						DateCreated = p.DateCreated.ToString(),
						Duration = FormatTimespan(p.DateCreated, p.DateExpires),
						TimeLeft = FormatTimeLeft(p.DateCreated, p.DateExpires, p.InEffect)
					});
				}

				gvBanList.DataBind();

				if (gvBanList.Rows.Count == 0)
					pNoBannedUsers.Visible = true;
			}
		}

		private string FormatTimeLeft(DateTime startTime, DateTime? endTime, bool inEffect)
		{
			if (endTime < DateTime.Now || inEffect == false)
				return "Expired";

			return FormatTimespan(DateTime.Now, endTime);
		}

		private string FormatTimespan(DateTime startTime, DateTime? endTime)
		{
			if (endTime != null)
			{
				TimeSpan time = endTime.Value - startTime;

				if (time.TotalDays >= 365)
				{
					int years = (int)Math.Floor(time.TotalDays / 365);
					int days = (int)time.TotalDays - (years * 365);

					return String.Format("{0}y {1}d {2}h {3}m", years, days, time.Hours, time.Minutes);
				}
				else
				{
					return String.Format("{0}d {1}h {2}m", (int) Math.Floor(time.TotalDays), time.Hours, time.Minutes);
				}
			}
			else
			{
				return "Infinite";
			}
		}
	}
}
