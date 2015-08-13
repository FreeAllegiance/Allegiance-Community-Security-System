using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;

namespace Allegiance.CommunitySecuritySystem.Management.UI
{
	public class Page : System.Web.UI.Page
	{
		public new MasterPages.Default Master
		{
			get
			{
				if (this.Page.Master is MasterPages.Default)
					return (MasterPages.Default)this.Page.Master;

				throw new Exception("The inheriting page is not a content web page that points to MasterPages.Default");
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			HtmlLink link = new HtmlLink();
			link.Attributes.Add("rel", "stylesheet");
			link.Attributes.Add("type", "text/css");

			if (Request.Browser.IsBrowser("IE") == true)
			{
				link.Attributes.Add("href", ResolveUrl("~/Css/ie_only.css"));
			}
			else
			{
				link.Attributes.Add("href", ResolveUrl("~/Css/non_ie.css"));
			}

			Header.Controls.Add(link);

			base.OnLoad(e);
		}
	}
}
