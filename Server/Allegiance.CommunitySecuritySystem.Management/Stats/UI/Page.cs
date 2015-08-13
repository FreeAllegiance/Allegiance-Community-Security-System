using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Allegiance.CommunitySecuritySystem.Management.Stats.UI
{
	public class Page : Management.UI.Page
	{
		protected override void OnLoad(EventArgs e)
		{
			Master.PageHeader = "CSS - Stats";

			base.OnLoad(e);
		}

	}
}
