using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Linq.SqlClient;

namespace Allegiance.CommunitySecuritySystem.Management.Users
{
	public partial class Default : UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.IsPostBack == false)
			{
				if (String.IsNullOrEmpty(Request.Params["searchText"]) == false)
				{
					txtSearch.Text = Request.Params["searchText"];
					BindData();
				}
			}
		}

		private void BindData()
		{
			string searchText = txtSearch.Text;
			if (searchText.Contains("%") == false)
				searchText += "%";

			using (var db = new DataAccess.CSSDataContext())
			{
				var matchingUsers = db.Logins.Where(
					p => p.Aliases.Count(q => SqlMethods.Like(q.Callsign, searchText)) > 0
					|| SqlMethods.Like(p.Email, searchText)
					|| SqlMethods.Like(p.Username, searchText)
					).OrderBy(p => p.Username).Take(100).Select( p => new
					{
						DateCreated = p.DateCreated,
						Email = p.Email,
						Id = p.Id,
						LastLogin = p.Identity.DateLastLogin,
						Username = p.Username,
						LinkManagementLabel = p.Identity.Logins.Count() > 1 ? "Unlink" : "Link"
					});

				//List<Data.EditableUser> editableUsers = new List<Allegiance.CommunitySecuritySystem.Management.Users.Data.EditableUser>();
				//foreach (var matchingUser in matchingUsers)
				//{
				//    editableUsers.Add(new Allegiance.CommunitySecuritySystem.Management.Users.Data.EditableUser()
				//    {
				//        DateCreated = matchingUser.DateCreated,
				//        Email = matchingUser.Email,
				//        Id = matchingUser.Id,
				//        LastLogin = matchingUser.Identity.DateLastLogin,
				//        Username = matchingUser.Username,
				//        LinkManagementLabel = matchingUser.Identity.Links.Count() > 0 ? "Unlink" : "Link"
				//    });
				//}

				if (matchingUsers.Count() > 0)
				{
					gvUsers.Visible = true;
					gvUsers.DataSource = matchingUsers;
					gvUsers.DataBind();
				}
				else
				{
					gvUsers.Visible = false;
				}
			}
		}

		//private string FormatUnlinkAccountHtml(Allegiance.CommunitySecuritySystem.DataAccess.Login login)
		//{
		//    if (login.Identity.Links.Count > 0)
		//        return String.Format("<a href=\"ManageLinks.aspx?loginID={0}&searchText={1}\">Unlink</a>", login.Id, txtSearch.Text);
		//    else
		//        return string.Format("");
		//}

		protected void txtSearch_TextChanged(object sender, EventArgs e)
		{
			BindData();
		}

		protected void btnSearch_Click(object sender, EventArgs e)
		{
			BindData();
		}
	}
}
