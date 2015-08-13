using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Allegiance.CommunitySecuritySystem.DataAccess;
using Allegiance.CommunitySecuritySystem.Management.Properties;

namespace Allegiance.CommunitySecuritySystem.Management
{
	public partial class CreateUser : UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Master.PageHeader = "CSS - New User";

			if (Master.Breadcrumb != null)
				Master.Breadcrumb.Visible = true;

			
		}

		protected void cvCaptcha_ServerValidate(object source, ServerValidateEventArgs args)
		{
			bool isValid = false;

			if (ViewState["captchaToken"] != null)
			{
				Guid captchaToken = (Guid)ViewState["captchaToken"];
				using (CSSDataContext db = new CSSDataContext())
				{
					var captcha = db.Captchas.FirstOrDefault(p => p.Id == captchaToken);

					//args.Value
					//CustomValidator cvCaptcha = (CustomValidator)source;
					//TextBox txtValidationCode = (TextBox) cvCaptcha.ControlToValidate;
					if (captcha != null && args.Value.Equals(captcha.Answer, StringComparison.InvariantCultureIgnoreCase) == true)
					{
						isValid = true;
					}
				}
			}

			if (isValid == false)
				ViewState["captchaToken"] = null;

			args.IsValid = isValid;
		}

		protected void imgValidationImage_Load(object sender, EventArgs e)
		{
			
		}

		protected void imgValidationImage_PreRender(object sender, EventArgs e)
		{
			Guid captchaToken;

			if (ViewState["captchaToken"] == null)
			{
				Allegiance.CommunitySecuritySystem.Server.ClientService cs = new Server.ClientService();
				Allegiance.CommunitySecuritySystem.Server.Contracts.CaptchaResult captcha = cs.GetCaptcha(200, 100);

				ViewState["captchaToken"] = captcha.CaptchaToken;
				captchaToken = captcha.CaptchaToken;
			}
			else
			{
				captchaToken = (Guid)ViewState["captchaToken"];
			}

			Image imgValidationImage = (Image)sender;
			imgValidationImage.ImageUrl = Page.ResolveUrl("~/CaptchaImageGenerator.aspx?width=200&height=50&token=" + Server.UrlEncode(captchaToken.ToString()));

		}

		protected void userNameCustomValidator_ServerValidate(object source, ServerValidateEventArgs args)
		{
			TextBox userNameTextBox = (TextBox)CreateUserWizard1.WizardSteps[0].Controls[0].FindControl("UserName");

			string userName = userNameTextBox.Text;

			if (userName.Length < Alias.MinAliasLength || userName.Length > Alias.MaxAliasLength)
				args.IsValid = false;
		}

		protected void cvLegacyAliasCheck_ServerValidate(object source, ServerValidateEventArgs args)
		{
			if (Allegiance.CommunitySecuritySystem.DataAccess.Properties.Settings.Default.UseAsgsForLegacyCallsignCheck == true)
			{
				TextBox userNameTextBox = (TextBox)CreateUserWizard1.WizardSteps[0].Controls[0].FindControl("UserName");
				TextBox passwordTextBox = (TextBox)CreateUserWizard1.WizardSteps[0].Controls[0].FindControl("Password");

				using (CSSDataContext db = new CSSDataContext())
				{
					string callsign = Alias.GetCallsignFromStringWithTokensAndTags(db, userNameTextBox.Text);
					var checkAliasResult = Alias.ValidateLegacyCallsignUsage(callsign, passwordTextBox.Text);

					if (checkAliasResult == DataAccess.Enumerations.CheckAliasResult.InvalidLegacyPassword)
						args.IsValid = false;
				}
			}
		}
	}
}
