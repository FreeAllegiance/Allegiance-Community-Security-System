using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Allegiance.CommunitySecuritySystem.Server.Contracts;

namespace Allegiance.CommunitySecuritySystem.Management.Enforcer
{
	public partial class Default : UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			ucBan.ActivePlayersOnly = true;
		}
	}
}
