using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using Allegiance.CommunitySecuritySystem.Management.Properties;
using Allegiance.CommunitySecuritySystem.Management.Business;
using Allegiance.CommunitySecuritySystem.Common.Utility;
using System.Text.RegularExpressions;
using Allegiance.CommunitySecuritySystem.DataAccess;
using System.Net.Mail;

namespace Allegiance.CommunitySecuritySystem.Management
{
	public partial class ForgotPassword : UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Master.PageHeader = "CSS - Forgot Password";

			if (Master.Breadcrumb != null)
				Master.Breadcrumb.Visible = true;

			pErrorMessage.Visible = false;
			divPasswordRecovery.Visible = true;
			lblSuccessMessage.Visible = false;
		}

		protected void PasswordRecovery1_SendingMail(object sender, MailMessageEventArgs e)
		{
			
			e.Cancel = true;
		}

		private string GeneratePasswordResetLink(string toEmailAddress)
		{
			Guid resetGuid = Guid.NewGuid();

			using (CSSDataContext db = new CSSDataContext())
			{
				foreach (var login in db.Logins.Where(p => p.Email == toEmailAddress.Trim()))
				{
					login.PasswordResetGuid = resetGuid;
					login.PasswordResetSendDate = DateTime.Now;
				}

				db.SubmitChanges();
			}

			return Request.Url.Scheme + "://" + Request.Url.Authority + "/" + ResolveClientUrl("~/ResetPassword.aspx?resetGuid=" + resetGuid.ToString());
		}

		protected void btnSubmit_Click(object sender, EventArgs e)
		{

			if (this.IsValid == false)
				return;

			string email;
			string username;
			string successMessage = String.Empty;

			using(CSSDataContext db = new CSSDataContext())
			{
				var login = DataAccess.Login.FindLoginByUsernameOrCallsign(db, txtUsername.Text);

				// this should never happen because the custom validator should trap it.
				if (login == null)
					throw new Exception("Couldn't find login for username: " + txtUsername.Text);

				email = login.Email;
				username = login.Username;
			}

			Regex mailFinder = new Regex(
				  "(?<mail>.*?)(?<rest>@.*?)$",
				RegexOptions.Multiline
				| RegexOptions.ExplicitCapture
				| RegexOptions.CultureInvariant
				| RegexOptions.Compiled
				);

			Match match = mailFinder.Match(email);
			if (match.Success == true)
			{
				string maskedEmail = mailFinder.Replace(email, String.Empty.PadLeft(match.Groups["mail"].Value.Length, '*') + "$2");
				lblSuccessMessage.Text = "A password reset link has been mailed to your registered email address at: " + maskedEmail;
			}
			else
			{
				lblSuccessMessage.Text = "A password reset link has been mailed to your registered email address.";
			}

			string mailBody = String.Format(@"
Hello {0}!

Here is your password reset link. Please click on this link to reset your FreeAllegiance game password.

{1}

Please note, this link will expire in 24 hours. 

Thank you playing, and see you online soon!

- The FreeAllegiance.org team.
",
				username,
				GeneratePasswordResetLink(email)
 );

			MailMessage mailMessage = new MailMessage("allegiance.css.server@gmail.com", email, "Password Reset Link", mailBody);
			MailManager.SendMailMessage(mailMessage);

			divPasswordRecovery.Visible = false;
			lblSuccessMessage.Visible = true;
		}

		protected void cvUsername_ServerValidate(object source, ServerValidateEventArgs args)
		{
			using (CSSDataContext db = new CSSDataContext())
			{
				var login = DataAccess.Login.FindLoginByUsernameOrCallsign(db, txtUsername.Text);

				if (login == null)
				{
					args.IsValid = false;
				}
			}
		}
	}
}
