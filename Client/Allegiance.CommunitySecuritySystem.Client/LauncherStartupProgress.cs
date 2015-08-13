using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Allegiance.CommunitySecuritySystem.Client.Service;
using Allegiance.CommunitySecuritySystem.Client.ClientService;
using System.Threading;

namespace Allegiance.CommunitySecuritySystem.Client
{
	public partial class LauncherStartupProgress : LoginBaseForm
	{
		private List<string> _statusMessages = new List<string>( new string []
		{
			"Requesting machine verifier.",
			"Creating machine signature.",
			"Encrypting machine signature.",
			"Sending machine signature for verification.",
			"Receiving verification.",
			"Resolving verification details.",
			"Complete. Starting launcher."
		});

		private int _progressIndex;
		private System.Timers.Timer _progressTimer = new System.Timers.Timer(700);

		public LauncherStartupProgress()
		{
			InitializeComponent();
		}

		private void _progressTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (_progressIndex >= _statusMessages.Count)
				_progressTimer.Stop();
			else
				SetProgress(_statusMessages[_progressIndex++]);
		}

		private delegate void SetProgressLabelDelegate(string progressLabel);
		private void SetProgress(string progressLabel)
		{
			if (this.InvokeRequired == true)
			{
				this.Invoke(new SetProgressLabelDelegate(SetProgress), progressLabel);
				return;
			}

			_progressLabel.Text = progressLabel;

			_startupProgressBar.Value = _progressIndex;
		}

		void LauncherStartupProgress_Load(object sender, EventArgs e)
		{
			_startupProgressBar.Minimum = 0;
			_startupProgressBar.Maximum = _statusMessages.Count - 1;

			_progressIndex = 0;

			_progressTimer.Elapsed += new System.Timers.ElapsedEventHandler(_progressTimer_Elapsed);
			_progressTimer.Start();

			SetProgress("Initializing.");

			ServiceHandler.LauncherSignIn(new ServiceHandler.LauncherSignInCompleteDelegate(LauncherSignInComplete));
		}

		private void LauncherSignInComplete(LauncherSignInResult launcherSignInResult)
		{
			if (string.IsNullOrEmpty(launcherSignInResult.LinkedAccount) == false)
			{
				//OfflineLaunch.ShowOfflineLaunchForLinkedAccount(this, AuthenticatedData.PersistedUser, launcherSignInResult.LinkedAccount);
				ShowOfflineLaunchOption(AuthenticatedData.PersistedUser, launcherSignInResult.LinkedAccount, true);
				return;
			}

			switch (launcherSignInResult.Status)
			{
				case CheckInStatus.AccountLinked:
					throw new Exception("Status: AccountLinked - should never get here.");

				case CheckInStatus.InvalidCredentials:
					ShowMessageBox("The stored account credentials are no longer valid, please log in again.");
					CompleteFormProgressAndClose(System.Windows.Forms.DialogResult.None);
					break;

				case CheckInStatus.InvalidHash:
					ShowMessageBox("The machine hash was not valid, please log in again.");
					CompleteFormProgressAndClose(System.Windows.Forms.DialogResult.None);
					break;

				case CheckInStatus.Ok:
					CompleteFormProgressAndClose(System.Windows.Forms.DialogResult.OK);
					break;

				case CheckInStatus.Timeout:
					ShowMessageBox("A timeout occurred trying to validate your stored credentials. Please log in again.");
					CompleteFormProgressAndClose(System.Windows.Forms.DialogResult.None);
					break;

				case CheckInStatus.VirtualMachineBlocked:
					ShowVirtualMachineInfo(true);
					break;

				case CheckInStatus.AccountLocked:
					ShowMessageBox("Your account has been locked, please go to the forums for help.");
					CompleteFormProgressAndClose(System.Windows.Forms.DialogResult.None);
					break;

				case CheckInStatus.PermissionDenied:
					ShowMessageBox("Permission denied, please see the forums for help.");
					CompleteFormProgressAndClose(System.Windows.Forms.DialogResult.None);
					break;

				default:
					throw new NotSupportedException(launcherSignInResult.Status.ToString());
			}
		}

		private delegate void CompleteFormProgressAndCloseDelegate(DialogResult dialogResult);
		private void CompleteFormProgressAndClose(DialogResult dialogResult)
		{
			if (this.InvokeRequired == true)
			{
				this.Invoke(new CompleteFormProgressAndCloseDelegate(CompleteFormProgressAndClose), dialogResult);
				return;
			}

			_progressTimer.Stop();

			for (; _progressIndex < _statusMessages.Count; _progressIndex++)
			{
				SetProgress(_statusMessages[_progressIndex]);
				Thread.Sleep(50);
			}

			this.DialogResult = dialogResult;
			this.Close();
		}

		//private delegate void CloseFormDelegate();
		//private void CloseForm()
		//{
		//    if (this.InvokeRequired == true)
		//    {
		//        this.Invoke(new CloseFormDelegate(CloseForm));
		//        return;
		//    }

		//    this.Close();
		//}





		private delegate void ShowMessageBoxDelegate(string message);
		private void ShowMessageBox(string message)
		{
			if (this.InvokeRequired == true)
			{
				this.Invoke(new ShowMessageBoxDelegate(ShowMessageBox), message);
				return;
			}

			MessageBox.Show(message, "Login Results", MessageBoxButtons.OK, MessageBoxIcon.Error);
			//this.DialogResult = System.Windows.Forms.DialogResult.None;
			//this.CloseForm();
		}

		//private delegate void ShowOfflineLaunchOptionDelegate(string messagePrompt, string helpLinkUrl, string helpLinkLabel);
		//private void ShowOfflineLaunchOption(string messagePrompt, string helpLinkUrl, string helpLinkLabel)
		//{
		//    if (this.InvokeRequired == true)
		//    {
		//        this.Invoke(new ShowOfflineLaunchOptionDelegate(ShowOfflineLaunchOption), messagePrompt, helpLinkUrl, helpLinkLabel);
		//        return;
		//    }

		//    var offlineLaunch = new OfflineLaunch(messagePrompt, helpLinkUrl, helpLinkLabel);
		//    offlineLaunch.TopMost = true;
		//    offlineLaunch.ShowDialog();

		//    this.DialogResult = System.Windows.Forms.DialogResult.None;
		//    this.CloseForm();
		//}
	}
}
