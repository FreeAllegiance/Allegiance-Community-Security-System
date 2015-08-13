using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace Allegiance.CommunitySecuritySystem.Management
{
	public partial class Default : UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Master.PageHeader = "CSS - Main Menu";

			if (Membership.GetUser() == null)
			{
				FormsAuthentication.RedirectToLoginPage();
				Response.End();
			}
		}
	}
}
