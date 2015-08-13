using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Allegiance.CommunitySecuritySystem.Management.Enforcer
{
	public partial class Search : UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			BindData();
			
		}

		private void BindData()
		{
			if (String.IsNullOrEmpty(Request.Params["searchText"]) == false && String.IsNullOrEmpty(txtSearch.Text) == true)
				txtSearch.Text = Request.Params["searchText"];

			if (String.IsNullOrEmpty(txtSearch.Text) == true)
			{
				ucBan.Visible = false;
				return;
			}

			ucBan.Visible = true;
			ucBan.SearchText = txtSearch.Text;
			ucBan.ActivePlayersOnly = false;
			ucBan.BindData();

		}

		protected void txtSearch_TextChanged(object sender, EventArgs e)
		{

		}
	}
}
