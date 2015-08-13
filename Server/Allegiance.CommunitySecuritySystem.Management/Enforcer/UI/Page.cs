using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Allegiance.CommunitySecuritySystem.Management.Enforcer.UI
{
	public class Page : Management.UI.Page
	{
		protected override void OnLoad(EventArgs e)
		{
			Master.PageHeader = "CSS - Enforcer";

			if (Business.Authorization.IsModeratorOrZoneLeadOrAdminOrSuperAdmin(User) == false)
				Response.Redirect("~/Default.aspx");

			if (Master.Breadcrumb != null)
				Master.Breadcrumb.Visible = true;

			base.OnLoad(e);
		}
	}
}
