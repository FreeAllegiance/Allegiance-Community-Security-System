using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Allegiance.CommunitySecuritySystem.DataAccess;

namespace Allegiance.CommunitySecuritySystem.Management.Content.Polls
{
	public partial class Default : UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			BindData();
		}

		private void BindData()
		{
			using (CSSDataContext db = new CSSDataContext())
			{
				gvPolls.DataSource = db.Polls.OrderByDescending(p => p.DateExpires);
				gvPolls.DataBind();
			}
		}

		protected void btnAddPoll_Click(object sender, EventArgs e)
		{
			Response.Redirect("EditPoll.aspx");
		}
	}
}