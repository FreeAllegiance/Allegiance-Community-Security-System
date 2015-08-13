using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Linq.SqlClient;

namespace Allegiance.CommunitySecuritySystem.Management.Users
{
	public partial class LinkToLogin : UI.Page
	{
		private int PrimaryLoginID
		{
			get
			{
				return Int32.Parse(Request.QueryString["loginID"]);
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			Master.PageHeader = "CSS - Link To Another Login";
		}

		private void BindData()
		{
			string searchText = txtSearch.Text;
			if (searchText.Contains("%") == false)
				searchText += "%";

			using (var db = new DataAccess.CSSDataContext())
			{
				var primaryLogin = db.Logins.FirstOrDefault(p => p.Id == PrimaryLoginID);

				var unlinkedLoginIds = db.Login_UnlinkedLogins
					.Where(p => p.LoginId1 == PrimaryLoginID)
					.Select(p => p.LoginId2)
					.Union(db.Login_UnlinkedLogins
						.Where(p => p.LoginId2 == PrimaryLoginID)
						.Select(p => p.LoginId1)
						);


				var matchingUsers = db.Logins.Where(
					p => (p.Aliases.Count(q => SqlMethods.Like(q.Callsign, searchText)) > 0
							|| SqlMethods.Like(p.Email, searchText)
							|| SqlMethods.Like(p.Username, searchText))
						&& p.IdentityId != primaryLogin.IdentityId
						&& unlinkedLoginIds.Contains(p.Id) == false
					).OrderBy(p => p.Username).Take(100).Select(p => new
					{
						DateCreated = p.DateCreated,
						Email = p.Email,
						Id = p.Id,
						LastLogin = p.Identity.DateLastLogin,
						Username = p.Username
					});

				if (matchingUsers.Count() > 0)
				{
					gvLogins.Visible = true;
					gvLogins.DataSource = matchingUsers;
					gvLogins.DataBind();
				}
				else
				{
					gvLogins.Visible = false;
				}
			}
		}

		protected void lnkSelect_Command(object sender, CommandEventArgs e)
		{
			int targetLoginID = Int32.Parse(e.CommandArgument.ToString());

			using (DataAccess.CSSDataContext db = new Allegiance.CommunitySecuritySystem.DataAccess.CSSDataContext())
			{
				var primaryIdentity = db.Logins.FirstOrDefault(p => p.Id == PrimaryLoginID).Identity;
				var secondaryIdentity = db.Logins.FirstOrDefault(p => p.Id == targetLoginID).Identity;

				DataAccess.Identity.MergeIdentities(db, new DataAccess.Identity[] { primaryIdentity, secondaryIdentity });

				db.SubmitChanges();
			}

			Response.Redirect("~/Users/ManageLinks.aspx?loginID=" + PrimaryLoginID + "&searchText=" + Server.UrlEncode(Request.Params["searchText"]), true);
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
			Response.Redirect("~/Users/ManageLinks.aspx?loginID=" + PrimaryLoginID + "&searchText=" + Server.UrlEncode(Request.Params["searchText"]), true);
		}
	}
}
