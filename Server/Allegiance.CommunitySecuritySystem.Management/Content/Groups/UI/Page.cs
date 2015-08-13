using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Allegiance.CommunitySecuritySystem.Management.Content.Groups.UI
{
	public class Page : Content.UI.Page
	{
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			Master.PageHeader = "CSS - Manage Squads and Groups";
		}
	}
}