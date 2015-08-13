using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Allegiance.CommunitySecuritySystem.Management.UI.MasterPages
{
	public partial class Default : System.Web.UI.MasterPage
	{
		public string PageHeader { get; set; }
		public bool UseFullWidth { get; set; }

		public SiteMapPath Breadcrumb
		{
			get { return smpBreadcrumb; }
		}

	
		protected void Page_Load(object sender, EventArgs e)
		{
			if (UseFullWidth == true)
			{
				tableMainForm.Attributes["class"] = "fullWidthPage mainForm";
				//tdMainNav.Attributes["class"] = "mainNav fullWidth";
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
			if (String.IsNullOrEmpty(this.Page.Title) == true && String.IsNullOrEmpty(this.PageHeader) == false)
				this.Page.Title = this.PageHeader;

			base.OnPreRender(e);
		}
	}
}
