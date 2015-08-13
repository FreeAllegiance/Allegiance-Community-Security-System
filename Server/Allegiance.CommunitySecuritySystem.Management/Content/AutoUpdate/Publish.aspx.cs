using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Allegiance.CommunitySecuritySystem.Management.Business;

namespace Allegiance.CommunitySecuritySystem.Management.Content.AutoUpdate
{
	public partial class Publish : UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.IsPostBack == false)
				BindData();
		}

		private void BindData()
		{
			gvPublications.DataSource = AutoUpdateManager.GetPublications();
			gvPublications.DataBind();
		}

		protected void btnNewPublication_Click(object sender, EventArgs e)
		{

		}
	}
}
