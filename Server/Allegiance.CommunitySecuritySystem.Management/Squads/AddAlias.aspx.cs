using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Linq.SqlClient;

namespace Allegiance.CommunitySecuritySystem.Management.Squads
{
	public partial class AddAlias : UI.Page
	{
		protected int GroupID
		{
			get
			{
				if (string.IsNullOrEmpty(Request.Params["groupID"]) == true)
					throw new Exception("groupID not specified.");

				return Int32.Parse(Request.Params["groupID"]);
			}
		}

		protected string Group;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.IsPostBack == false)
			{
				if (String.IsNullOrEmpty(Request.Params["searchText"]) == false)
				{
					txtSearch.Text = Request.Params["searchText"];
				}
			}

			BindData();
		}

		private void BindData()
		{
			string searchText = txtSearch.Text;
			if (searchText.Contains("%") == false)
				searchText += "%";

			using (var db = new DataAccess.CSSDataContext())
			{
				var group = db.Groups.FirstOrDefault(p => p.Id == GroupID);

				lblSquadName.Text = group.Name;
				Group = group.Name;

				//var matchingUsers = group.Group_Alias_GroupRoles.Where(p => SqlMethods.Like(p.Alias.Callsign, searchText)).OrderBy(p => p.Alias.Callsign).Take(100).Select(p => p.Alias);


				if (searchText.Length > 1)
				{
					var matchingUsers = db.Alias.Where(p => SqlMethods.Like(p.Callsign, searchText)
						&& db.Group_Alias_GroupRoles.Where(q => q.GroupId == GroupID && q.AliasId == p.Id).Count() == 0).OrderBy(p => p.Callsign).Take(100);

					gvUsers.DataSource = matchingUsers.ToList();
					gvUsers.DataBind();
				}
			}
		}

		protected void txtSearch_TextChanged(object sender, EventArgs e)
		{
			BindData();
		}

		protected void btnSearch_Click(object sender, EventArgs e)
		{
			BindData();
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			Response.Redirect("Default.aspx?groupID=" + GroupID);
		}
	}
}
