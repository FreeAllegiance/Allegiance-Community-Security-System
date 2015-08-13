using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;
using Allegiance.CommunitySecuritySystem.Client.Utility;
using Allegiance.CommunitySecuritySystem.Client.ClientService;
using Allegiance.CommunitySecuritySystem.Client.Service;
using Allegiance.CommunitySecuritySystem.Client.Integration;


namespace Allegiance.CommunitySecuritySystem.Client.Controls
{
	public partial class NewAccount : LoginControlBase
	{
		private Guid _captchaToken;
		private System.Timers.Timer _accountCreationTimer;
		private bool _enableValidators = false;

		public delegate void CancelAccountSignupHandler();
		public event CancelAccountSignupHandler CancelAccountSignup;

		public delegate void AccountCreatedSuccessHandler(string username, string password);
		public event AccountCreatedSuccessHandler AccountCreatedSuccess;

		private CallsignChecker _callsignChecker;

		public NewAccount()
		{
			InitializeComponent();

			_callsignChecker = new CallsignChecker(_usernameTextBox, _errorProvider, _createAccountButton, new CallsignChecker.SetCheckMessageDelegate(SetCheckMessage));
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (keyData == Keys.Enter)
			{
				if (this._createAccountButton.Enabled == true)
					this._createAccountButton_Click(this, EventArgs.Empty);

				return true;
			}

			return base.ProcessDialogKey(keyData);
		}


		private void _createAccountButton_Click(object sender, EventArgs e)
		{
			// Don't start showing the validators until the user tries to create an account the first time.
			_enableValidators = true;

			if (this.ValidateChildren() == false)
				return;

			if(this.ValidateLegacyCallsign() == false)
				return;

			this.ParentForm.Enabled = false;
			this.ParentForm.Cursor = Cursors.WaitCursor;

			_accountCreationTimer = new System.Timers.Timer(30000);
			_accountCreationTimer.Elapsed += new System.Timers.ElapsedEventHandler(AccountCreationTimeoutElapsed);
			_accountCreationTimer.Start();

			Service.Authentication.CreateNewAccount(_usernameTextBox.Text, _passwordTextBox.Text, _emailTextBox.Text, _captchaToken.ToString(), _verificationCodeTextBox.Text, new ClientService.CreateLoginCompletedEventHandler(OnCreateLoginCompleted));
		}

		private bool ValidateLegacyCallsign()
		{
			var legacyCallsignCheck = Service.Callsign.ValidateLegacyCallsignUsage(_usernameTextBox.Text, _passwordTextBox.Text);

			switch (legacyCallsignCheck)
			{
				case CheckAliasResult.InvalidLegacyPassword:
					MessageBox.Show("You are trying to register a callsign that has been previously registered on ASGS (the old authentication system). To claim this callsign, you must use your orignal password. If you don't remeber this password, please contact the adminstration for assistance, or use a new callsign.");
					break;

				case CheckAliasResult.Available:
					return true;

				default:
					MessageBox.Show("There was an unknown error checking if your callsign was previously registered on ASGS. Please use your original password, or try a different callsign.");
					break;
			}

			return false;
		}



		void OnCreateLoginCompleted(object sender, ClientService.CreateLoginCompletedEventArgs args)
		{
			_accountCreationTimer.Stop();

			this.ParentForm.Enabled = true;
			this.ParentForm.Cursor = Cursors.Default;

			switch (args.Result.MembershipCreateStatus)
			{
				case ClientService.MembershipCreateStatus.InvalidAnswer:
					MessageBox.Show("Please check the verification code you entered and try again.");
					_verificationCodeTextBox.Focus();
					_verificationCodeTextBox.SelectAll();
					break;

				case ClientService.MembershipCreateStatus.DuplicateEmail:
					MessageBox.Show("This email address is already in use. Please check the email address and try again.");
					_emailTextBox.Focus();
					_emailTextBox.SelectAll();
					break;

				case ClientService.MembershipCreateStatus.DuplicateUserName:
					MessageBox.Show("This user name is already in use. Please check the user name and try again.");
					_usernameTextBox.Focus();
					_usernameTextBox.SelectAll();
					break;

				case ClientService.MembershipCreateStatus.InvalidEmail:
					MessageBox.Show("Your email is invalid. Please check the email address and try again.");
					_emailTextBox.Focus();
					_emailTextBox.SelectAll();
					break;

				case ClientService.MembershipCreateStatus.InvalidPassword:
					MessageBox.Show("Your password is invalid. Please check the password and try again.");
					_verifyPasswordTextBox.Text = String.Empty;
					_passwordTextBox.Focus();
					_passwordTextBox.SelectAll();
					break;

				case ClientService.MembershipCreateStatus.InvalidUserName:
					MessageBox.Show("This user name is invalid. Please check the user name and try again.");
					_usernameTextBox.Focus();
					_usernameTextBox.SelectAll();
					break;

				case ClientService.MembershipCreateStatus.ProviderError:
					MessageBox.Show("There was a provider error creating your account. Please try again.");
					break;

				case ClientService.MembershipCreateStatus.Success:
					MessageBox.Show("Your account has been created.");
					if (AccountCreatedSuccess != null)
						AccountCreatedSuccess(_usernameTextBox.Text, _passwordTextBox.Text);
					break;

				case ClientService.MembershipCreateStatus.UserRejected:
					MessageBox.Show("The user name was rejected. Please check the user name and try again.");
					_usernameTextBox.Focus();
					_usernameTextBox.SelectAll();
					break;

				default:
					MessageBox.Show("Your account was not created.");
					break;
			}

			if(args.Result.MembershipCreateStatus != ClientService.MembershipCreateStatus.Success)
				LoadCaptchaImage();
		}

		void AccountCreationTimeoutElapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			_accountCreationTimer.Stop();
			this.ParentForm.Enabled = true;
			this.ParentForm.Cursor = Cursors.Default;

			MessageBox.Show("There was an error creating your login account. The server did not respond within the required time. Please try again later.");

			if (CancelAccountSignup != null)
				CancelAccountSignup();
		}

		//private bool ValidateUsername(string userName)
		//{
		//    const int _minAliasLength = 3 - 1;
		//    const int _maxAliasLength = 17 - 1;

		//    return Regex.IsMatch(userName,
		//            string.Concat(@"[a-zA-Z0-9_]\w{", _minAliasLength, ",", _maxAliasLength, "}"),
		//            RegexOptions.Compiled | RegexOptions.IgnoreCase);
		//}

		private bool ValidateEmailAddress(string email)
		{
			// email matching Regex from http://geekswithblogs.net/VROD/archive/2007/03/16/109007.aspx
			return Regex.IsMatch(email, @"^(([^<>()[\]\\.,;:\s@\""]+"
                        + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                        + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                        + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                        + @"[a-zA-Z]{2,}))$");
		}

		private void _cancelButton_Click(object sender, EventArgs e)
		{
			if (CancelAccountSignup != null)
				CancelAccountSignup();
		}

		private void OnValidating(object sender, CancelEventArgs e)
		{
			string statusMessage = null;

			if (_enableValidators == false)
				return;

			if (_usernameTextBox.Text.Trim().Length == 0)
			{
				_errorProvider.SetError(_usernameTextBox, "Please enter a user name.");
				statusMessage = "Username is required.";

				if (sender == _usernameTextBox)
					e.Cancel = true;
			}
			else
			{
				_errorProvider.SetError(_usernameTextBox, "");
			}

			if (_passwordTextBox.Text.Trim().Length == 0)
			{
				_errorProvider.SetError(_passwordTextBox, "Please enter a password.");
				statusMessage = "Password is required.";

				if (sender == _passwordTextBox)
					e.Cancel = true;
			}
			else
			{
				_errorProvider.SetError(_passwordTextBox, "");
			}

			if (_verifyPasswordTextBox.Text.Trim().Length == 0)
			{
				_errorProvider.SetError(_verifyPasswordTextBox, "Please enter a verification password.");
				statusMessage = "Verification Password is required.";

				if (sender == _verifyPasswordTextBox)
					e.Cancel = true;
			}
			else
			{
				_errorProvider.SetError(_verifyPasswordTextBox, "");
			}

			if (_verificationCodeTextBox.Text.Trim().Length == 0)
			{
				_errorProvider.SetError(_verificationCodeTextBox, "Please enter the verification code displayed below.");
				statusMessage = "Please enter the verification code displayed in the picture.";

				if (sender == _verificationCodeTextBox)
					e.Cancel = true;
			}
			else
			{
				_errorProvider.SetError(_verificationCodeTextBox, "");
			}

			string errorMessage;
			if (Validation.ValidateAlias(_usernameTextBox.Text, out errorMessage) == false)
			{
				_errorProvider.SetError(_usernameTextBox, errorMessage);
				statusMessage = "Username does not meet requirements."; //way more description needed

				if (sender == _usernameTextBox)
					e.Cancel = true;
			}
			else
			{
				_errorProvider.SetError(_usernameTextBox, "");
			}

			if (_passwordTextBox.Text != _verifyPasswordTextBox.Text)
			{
				_errorProvider.SetError(_verifyPasswordTextBox, "Please reenter your password. Both passwords must match.");
			    statusMessage = "Please reenter your password. Both passwords must match."; //way more description needed

				if (sender == _verifyPasswordTextBox)
					e.Cancel = true;
			}
			else
			{
				_errorProvider.SetError(_verifyPasswordTextBox, "");
			}

			if (ValidateEmailAddress(_emailTextBox.Text) == false)
			{
				_errorProvider.SetError(_emailTextBox, "Please enter a valid email address.");
				statusMessage = "Please enter a valid email address."; //way more description needed

				if (sender == _emailTextBox)
					e.Cancel = true;
			}
			else
			{
				_errorProvider.SetError(_emailTextBox, "");
			}

			if (statusMessage != null)
				SetStatusBar(statusMessage, 0);

		}

		private void NewAccount_Load(object sender, EventArgs e)
		{
			LoadCaptchaImage();
		}

		private void LoadCaptchaImage()
		{
			_animatedThrobber.Visible = true;
			_verificationCodePictureBox.Visible = false;
			_verificationCodeTextBox.Enabled = false;
			_verificationCodeTextBox.Text = String.Empty;

			Service.Authentication.GetCaptchaAsync(200, 50, new ClientService.GetCaptchaCompletedEventHandler(delegate(object captchaSender, ClientService.GetCaptchaCompletedEventArgs args)
			{
				SetCaptchaImage(args.Result);
			})); 
		}

		private delegate void SetCaptchaImageDelegate(ClientService.CaptchaResult captchaResult);
		private void SetCaptchaImage(ClientService.CaptchaResult captchaResult)
		{
			if (this.InvokeRequired == true)
			{
				this.Invoke(new SetCaptchaImageDelegate(SetCaptchaImage), captchaResult);
			}
			else
			{
				_animatedThrobber.Visible = false;
				_verificationCodeTextBox.Enabled = true;
				_captchaToken = new Guid(captchaResult.CaptchaToken);
				_verificationCodePictureBox.Image = Image.FromStream(new MemoryStream(captchaResult.CaptchaImage));
				_verificationCodePictureBox.Visible = true;
			}
		}

		private void _usernameTextBox_TextChanged(object sender, EventArgs e)
		{
			_callsignChecker.RequestCallsignCheck();
		}

		private void SetCheckMessage(string message)
		{
			LoginForm.SetStatusBar(message);
		}

		private void _forgotPasswordButton_Click(object sender, EventArgs e)
		{
			Process.Start(AllegianceRegistry.GetManagementWebPath("ForgotPassword.aspx"));
		}
	}
}
