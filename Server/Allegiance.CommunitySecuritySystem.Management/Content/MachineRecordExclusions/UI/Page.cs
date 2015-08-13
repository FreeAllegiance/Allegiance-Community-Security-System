using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Allegiance.CommunitySecuritySystem.Management.Content.MachineRecordExclusions.UI
{
	public class Page : Content.UI.Page
	{
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			Master.PageHeader = "CSS - Manage Machine Record Exclusions";

			//if (Master.Breadcrumb != null)
			//	Master.Breadcrumb.Visible = true;

			
		}
	}
}