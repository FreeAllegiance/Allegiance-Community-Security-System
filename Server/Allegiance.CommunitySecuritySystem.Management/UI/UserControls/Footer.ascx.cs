using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Allegiance.CommunitySecuritySystem.Management.UI.UserControls
{
	public partial class Footer : System.Web.UI.UserControl
	{
		protected string WebsiteVersion;

		protected void Page_Load(object sender, EventArgs e)
		{
			WebsiteVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
		}
	}
}