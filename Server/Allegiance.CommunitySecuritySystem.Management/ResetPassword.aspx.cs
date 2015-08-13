using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Allegiance.CommunitySecuritySystem.DataAccess;
using System.Diagnostics;
using Allegiance.CommunitySecuritySystem.DataAccess.MembershipProviders;

namespace Allegiance.CommunitySecuritySystem.Management
{
	public partial class ResetPassword : UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Master.PageHeader = "CSS - Forgot Password";

			if (Master.Breadcrumb != null)
				Master.Breadcrumb.Visible = true;

			string resetGuid = Request.Params["resetGuid"];

			bool resetGuidPresent = false;

			if(String.IsNullOrEmpty(resetGuid) == false)
			{
				try
				{
					Guid passwordResetGuid = new Guid(resetGuid);
					using (CSSDataContext db = new CSSDataContext())
					{
						var login = db.Logins.FirstOrDefault(p => p.PasswordResetGuid == passwordResetGuid && p.PasswordResetSendDate > DateTime.Now.AddDays(-1));
						if (login != null)
							resetGuidPresent = true;
					}
				} 
				catch (Exception ex)
				{
					Debug.WriteLine("Invalid GUID: " + resetGuid + " - " + ex.ToString());
				}
			}

			if (resetGuidPresent == true)
			{
				divResetPassword.Visible = true;
				divResetLinkInvalid.Visible = false;
			}
			else
			{
				divResetPassword.Visible = false;
				divResetLinkInvalid.Visible = true;
			}

			divResetSuccess.Visible = false;
		}

		protected void btnSubmit_Click(object sender, EventArgs e)
		{
			using(CSSDataContext db = new CSSDataContext())
			{
				// If the page loaded, then don't worry about the date at this point, they'll get a free ride if 
				// they camped on the page for a while. The date is just to keep the enemy from getting a hold
				// of an old email and coming back in with it. 
				var logins = db.Logins.Where(p => p.PasswordResetGuid == new Guid(Request.Params["resetGuid"]));

				foreach(var login in logins)
				{
					CssMembershipProvider cssMembershipProvider = new CssMembershipProvider();

					string tempPassword = cssMembershipProvider.ResetPassword(login.Username, null);

					cssMembershipProvider.ChangePassword(login.Username, tempPassword, txtPassword.Text);
				}

				db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, logins);

				foreach (var login in logins)
				{
					// Keep anyone from reusing an old password reset mail.
					login.PasswordResetGuid = Guid.Empty;
				}

				db.SubmitChanges();
			}

			divResetPassword.Visible = false;
			divResetSuccess.Visible = true;
		}

		protected void cvValidatePasswords_ServerValidate(object source, ServerValidateEventArgs args)
		{
			args.IsValid = txtPassword.Text.Trim().Equals(txtVerifyPassword.Text.Trim());
		}
	}
}