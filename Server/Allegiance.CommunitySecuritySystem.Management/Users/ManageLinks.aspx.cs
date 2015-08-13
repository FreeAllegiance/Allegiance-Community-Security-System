using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Allegiance.CommunitySecuritySystem.Management.Users
{
	public partial class ManageLinks : UI.Page
	{
		private int PrimaryLoginID
		{
			get
			{
				return Int32.Parse(Request.QueryString["LoginID"]);
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			Master.PageHeader = "CSS - Manage Account Linking";

			if (this.IsPostBack == false)
				BindData();
		}

		private void BindData()
		{
			using (DataAccess.CSSDataContext db = new Allegiance.CommunitySecuritySystem.DataAccess.CSSDataContext())
			{
				var primaryLogin = db.Logins.FirstOrDefault(p => p.Id == PrimaryLoginID);

				if (primaryLogin == null)
					lblErrorMessage.Text = "The login for login id: " + PrimaryLoginID + " couldn't be found.";

				lblPrimaryLogin.Text = primaryLogin.Username;
				lblPermanentUnlinkPrimaryLogin.Text = primaryLogin.Username;

				var linkedLogins = db.Logins.Where(p => p.IdentityId == primaryLogin.Identity.Id && p.Id != primaryLogin.Id);

				pLinkedLogins.Visible = linkedLogins.Count() > 0;

				gvLinkedLogins.DataSource = linkedLogins;
				gvLinkedLogins.DataBind();

				var permanentUnlinks = db.Login_UnlinkedLogins.Where(p => p.LoginId1 == PrimaryLoginID).Select(p => new
				{
					UserName = p.Login1.Username,
					Id = p.Login1.Id,
					Email = p.Login1.Email,
					DateCreated = p.Login1.DateCreated
				}).Union(db.Login_UnlinkedLogins.Where(p => p.LoginId2 == PrimaryLoginID).Select(p => new
				{
					UserName = p.Login.Username,
					Id = p.Login.Id,
					Email = p.Login1.Email,
					DateCreated = p.Login1.DateCreated
				}));

				pPermanentUnlinks.Visible = permanentUnlinks.Count() > 0;

				gvPermanentUnlinks.DataSource = permanentUnlinks;
				gvPermanentUnlinks.DataBind();
			}
		}

		private void MergeLogins(int primaryLoginID, int mergeLoginID)
		{
			using (DataAccess.CSSDataContext db = new Allegiance.CommunitySecuritySystem.DataAccess.CSSDataContext())
			{
				var principalLogin = db.Logins.FirstOrDefault(p => p.Id == primaryLoginID);
				var loginToMerge = db.Logins.FirstOrDefault(p => p.Id == mergeLoginID);

				DataAccess.Identity.MergeLogin(db, principalLogin, loginToMerge);
			}
		}

		private void UnlinkLogins(int primaryLoginID, int unlinkLoginID)
		{
			using (DataAccess.CSSDataContext db = new Allegiance.CommunitySecuritySystem.DataAccess.CSSDataContext())
			{
				var principal = db.Logins.FirstOrDefault(p => p.Id == primaryLoginID).Identity;
				var loginToUnlink = db.Logins.FirstOrDefault(p => p.Id == unlinkLoginID);

				DataAccess.Identity.UnlinkLogin(db, principal, loginToUnlink);
			}
		}

		protected void lnkUnlink_Click(object sender, CommandEventArgs e)
		{
			int unlinkLoginID = Int32.Parse(e.CommandArgument.ToString());

			UnlinkLogins(PrimaryLoginID, unlinkLoginID);

			BindData();
		}

		protected void lnkMerge_Click(object sender, CommandEventArgs e)
		{
			int mergeLoginID = Int32.Parse(e.CommandArgument.ToString());

			MergeLogins(PrimaryLoginID, mergeLoginID);

			BindData();
		}



		

		protected void lnkPermanentUnlink_Click(object sender, CommandEventArgs e)
		{
			int unlinkLoginID = Int32.Parse(e.CommandArgument.ToString());

			UnlinkLogins(PrimaryLoginID, unlinkLoginID);

			using (DataAccess.CSSDataContext db = new Allegiance.CommunitySecuritySystem.DataAccess.CSSDataContext())
			{
				DataAccess.Login_UnlinkedLogin unlinkRecord = new Allegiance.CommunitySecuritySystem.DataAccess.Login_UnlinkedLogin()
				{
					LoginId1 = PrimaryLoginID,
					LoginId2 = unlinkLoginID
				};

				db.Login_UnlinkedLogins.InsertOnSubmit(unlinkRecord);

				db.SubmitChanges();
			}

			BindData();
		}

		protected void btnLinkAccount_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/Users/LinkToLogin.aspx?loginID=" + PrimaryLoginID + "&searchText=" + Server.UrlEncode(Request.Params["searchText"]), true);
		}

		protected void lnkRemovePermanentUnlink_Command(object sender, CommandEventArgs e)
		{
			int loginID = Int32.Parse(e.CommandArgument.ToString());

			using (DataAccess.CSSDataContext db = new Allegiance.CommunitySecuritySystem.DataAccess.CSSDataContext())
			{
				var permanentUnlink = db.Login_UnlinkedLogins
					.FirstOrDefault(p => (p.LoginId1 == PrimaryLoginID && p.LoginId2 == loginID) || (p.LoginId1 == loginID && p.LoginId2 == PrimaryLoginID));

				if (permanentUnlink != null)
				{
					db.Login_UnlinkedLogins.DeleteOnSubmit(permanentUnlink);
					db.SubmitChanges();
				}
			}

			BindData();
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/Users/Default.aspx?searchText=" + Server.UrlEncode(Request.Params["searchText"]), true);
		}
	}
}
