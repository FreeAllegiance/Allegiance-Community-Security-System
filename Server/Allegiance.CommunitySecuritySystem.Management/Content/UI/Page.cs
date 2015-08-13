using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Allegiance.CommunitySecuritySystem.Management.Content.UI
{
	public class Page : Management.UI.Page
	{
		protected override void OnLoad(EventArgs e)
		{
			Master.PageHeader = "CSS - Content Management";

			if (Business.Authorization.IsAdminOrSuperAdmin(User) == false)
				Response.Redirect("~/Default.aspx");

			base.OnLoad(e);
		}
	}
}
