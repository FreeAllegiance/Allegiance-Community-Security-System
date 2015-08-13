using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Allegiance.CommunitySecuritySystem.Client.Service;
using System.Diagnostics;
using Allegiance.CommunitySecuritySystem.Client.ClientService;
using Allegiance.CommunitySecuritySystem.Client.Integration;
using System.IO;

namespace Allegiance.CommunitySecuritySystem.Client.Controls
{
	public partial class Login : LoginControlBase
	{
		public delegate void LauncherSignInCompleteHandler(CheckInStatus loginStatus);
		public event LauncherSignInCompleteHandler LauncherSignInComplete;

		public delegate void ShowNewAccountSignUpHandler();
		public event ShowNewAccountSignUpHandler ShowNewAccountSignUp;

		public delegate void ShowOfflineLaunchHandler(string accountAttemptingToLogin, string oldestLinkedAccount, bool closeParentWindow);
		public event ShowOfflineLaunchHandler ShowOfflineLaunch;

		public delegate void ShowVirtualMachineInfoHandler(bool closeParentWindow);
		public event ShowVirtualMachineInfoHandler ShowVirtualMachineInfo;

		private bool _enableValidators = false;

		public string Username 
		{
			get { return _usernameTextBox.Text; }
			set { _usernameTextBox.Text = value; }
		}

		public string Password
		{
			get { return _passwordTextBox.Text; }
			set { _passwordTextBox.Text = value; }
		}


		public Login()
		{
			InitializeComponent();
		}

		private void _loginButton_Click(object sender, EventArgs e)
		{
			_enableValidators = true;

			if (this.ValidateChildren() == false)
				return;


			//Disable the form
			SetLoggingIn(true);

			//Create a thread-safe delegate
			var launcherSignInComplete = new ServiceHandler.LauncherSignInCompleteDelegate(delegate(LauncherSignInResult launcherSignInResult)
			{
				//Unlock the form
				SetLoggingIn(false);

				if (launcherSignInResult.StatusSpecified == false)
				{
					SetStatusBar("Failed to contact server, please try again later.", 0);
				}
				else
				{
					switch (launcherSignInResult.Status)
					{
						case CheckInStatus.Ok:
							if (LauncherSignInComplete != null)
								LauncherSignInComplete(launcherSignInResult.Status);
							break;

						case CheckInStatus.AccountLinked:
							if (ShowOfflineLaunch != null)
								ShowOfflineLaunch(_usernameTextBox.Text, launcherSignInResult.LinkedAccount, false);
							break;

						case CheckInStatus.VirtualMachineBlocked:
							if (ShowVirtualMachineInfo != null)
								ShowVirtualMachineInfo(false);
							break;

						case CheckInStatus.InvalidCredentials:
							SetStatusBar("Username or password was incorrect.", 0);
							break;

						case CheckInStatus.InvalidHash:
							SetStatusBar("Machine information package was incorrect.", 0);
							break;

						case CheckInStatus.Timeout:
							SetStatusBar("Server response timeout, please try again.", 0);
							break;

						case CheckInStatus.PermissionDenied:
							SetStatusBar("Permission was denied, please try again.", 0);
							break;

						case CheckInStatus.AccountLocked:
							SetStatusBar("Your account was locked out. Please use the forums for more help.", 0);
							break;

						default:
							throw new NotSupportedException(launcherSignInResult.Status.ToString());
					}
				}
			});

			//Check if the input credentials are valid
			ServiceHandler.LauncherSignIn(_usernameTextBox.Text, _passwordTextBox.Text, delegate(LauncherSignInResult launcherSignInResult)
			{
				//Check if it is safe to call the delegate from this thread
				if (this.InvokeRequired)
					this.Invoke(launcherSignInComplete, launcherSignInResult);
				else
					launcherSignInComplete(launcherSignInResult);
			});
		}


		private void SetLoggingIn(bool loggingIn)
		{
			_loginButton.Text = loggingIn ? "Logging In..." : "Login";

			if (loggingIn)
				SetStatusBar("Logging In...", 100);
			else
				SetStatusBar(string.Empty, 0);

			_loginButton.Enabled = !loggingIn;
			_registerButton.Enabled = !loggingIn;
			_playOfflineButton.Enabled = !loggingIn;
			_usernameTextBox.ReadOnly = loggingIn;
			_passwordTextBox.ReadOnly = loggingIn;
			btnForgotPassword.Enabled = !loggingIn;
		}




		private void _registerButton_Click(object sender, EventArgs e)
		{
			if (ShowNewAccountSignUp != null)
				ShowNewAccountSignUp();
		}

		private void OnValidating(object sender, CancelEventArgs e)
		{
			if (_enableValidators == false)
				return;

			string errorMessage = null;

			//if (this.Visible == false)
			//	return;

			if (_usernameTextBox.Text.Trim().Length == 0)
			{
				_errorProvider.SetError(_usernameTextBox, "Please enter a user name.");
				errorMessage = "Please enter a user name.";

				if(_usernameTextBox == sender)
					e.Cancel = true;
			}
			else
			{
				_errorProvider.SetError(_usernameTextBox, "");
			}

			if (_passwordTextBox.Text.Trim().Length == 0)
			{
				_errorProvider.SetError(_passwordTextBox, "Please enter a password.");
				errorMessage = "Password is required.";

				if (_passwordTextBox == sender)
					e.Cancel = true;
			}
			else
			{
				_errorProvider.SetError(_passwordTextBox, "");
			}

			if(errorMessage != null)
				SetStatusBar(errorMessage);
		}

		private void Login_Load(object sender, EventArgs e)
		{
			this.ParentForm.AcceptButton = this._loginButton;
		}

		private void btnForgotPassword_Click(object sender, EventArgs e)
		{
			Process.Start(AllegianceRegistry.GetManagementWebPath("ForgotPassword.aspx"));
		}

		private void _playOfflineButton_Click(object sender, EventArgs e)
		{
			_playOfflineButton.Enabled = false;

			AllegianceLoader.StartAllegiance(String.Empty, LobbyType.Production, String.Empty, new Utility.TaskDelegate(delegate(Object context)
				{
					AllegianceLoader.AllegianceExit += new EventHandler(AllegianceLoader_AllegianceExit);
				}));
		}

		void AllegianceLoader_AllegianceExit(object sender, EventArgs e)
		{
			if (this.InvokeRequired == true)
				this.Invoke(new EventHandler(delegate(object insideSender, EventArgs insideE)
				{
					_playOfflineButton.Enabled = true;
				}), sender, e);
			
		}

	}
}
