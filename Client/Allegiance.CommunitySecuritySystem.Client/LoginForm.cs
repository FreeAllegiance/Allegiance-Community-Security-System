using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Allegiance.CommunitySecuritySystem.Client.Service;
using Allegiance.CommunitySecuritySystem.Client.Utility;
using Allegiance.CommunitySecuritySystem.Client.ClientService;

namespace Allegiance.CommunitySecuritySystem.Client
{
    public partial class LoginForm : LoginBaseForm
    {
        #region Fields

		public CheckInStatus LauncherSignInStatus;

        const int StatusClearInterval = 7000;

		private Controls.Login _loginControl;
		private Controls.NewAccount _newAccountControl;
		private Controls.UpdateCheckControl _updateCheckControl;
        
        #endregion

        #region Properties

        private static LoginForm FormInstance { get; set; }

        protected Timer StatusTimer { get; set; }

        #endregion

        #region Constructors

        public LoginForm()
        {
            DebugDetector.AssertCheckRunning();

            InitializeComponent();

            StatusTimer             = new Timer();
            StatusTimer.Interval    = StatusClearInterval;
            StatusTimer.Tick        += new EventHandler(StatusTimer_Tick);

            FormInstance = this;
        }

        #endregion

        #region Events


        void StatusTimer_Tick(object sender, EventArgs e)
        {
            var pb = FormInstance._loginStatusStrip.Items[1] as ToolStripProgressBar;
            var percentage = pb.Value;

            if (percentage == 0)
                SetStatusBar(string.Empty);
        }

        #endregion

        #region Methods

        public static void SetStatusBar(string text)
        {
            SetStatusBar(text, 0);
        }

        public static void SetStatusBar(string text, int percentage)
        {
            var signal = new TaskDelegate(delegate(object parameter)
            {
                FormInstance._loginStatusStrip.Items[0].Text = parameter as string;

                var pb = FormInstance._loginStatusStrip.Items[1] as ToolStripProgressBar;
                pb.Value = percentage;

                FormInstance.StatusTimer.Stop();
                FormInstance.StatusTimer.Start();
            });

            if (FormInstance.InvokeRequired)
                FormInstance.Invoke(signal, text);
            else
                signal(text);
        }

        #endregion

		private void LoginForm_Load(object sender, EventArgs e)
		{
			AttachLoginControl();
		}

		private delegate void AttachUpdateCheckControlDelegate();
		private void AttachUpdateCheckControl()
		{
			if (this.InvokeRequired == true)
			{
				this.Invoke(new AttachUpdateCheckControlDelegate(AttachUpdateCheckControl));
			}
			else
			{
				_accountControlsPanel.Controls.Clear();

				_updateCheckControl = new Controls.UpdateCheckControl();

				_updateCheckControl.AutoupdateComplete += new Client.Controls.UpdateCheckControl.AutoupdateCompleteHandler(_updateCheckControl_AutoupdateComplete);
				_updateCheckControl.Dock = DockStyle.Fill;

				_accountControlsPanel.Controls.Add(_updateCheckControl);
			}
		}

		void _updateCheckControl_AutoupdateComplete(bool updateWasAborted)
		{
			if (updateWasAborted == true)
				Application.Exit();
			else
				AttachLoginControl();
		}

		private delegate void AttachLoginControlDelegate();
		private void AttachLoginControl()
		{
			if (this.InvokeRequired == true)
			{
				this.Invoke(new AttachLoginControlDelegate(AttachNewAccountControl));
			}
			else
			{
				_accountControlsPanel.Controls.Clear();

				_loginControl = new Controls.Login();
				_loginControl.StatusChanged += new Client.Controls.LoginControlBase.StatusChangedHandler(_loginControl_StatusChanged);
				_loginControl.LauncherSignInComplete += new Client.Controls.Login.LauncherSignInCompleteHandler(_loginControl_LoginComplete);
				_loginControl.ShowNewAccountSignUp += new Client.Controls.Login.ShowNewAccountSignUpHandler(_loginControl_ShowNewAccountSignUp);
				_loginControl.ShowOfflineLaunch += new Client.Controls.Login.ShowOfflineLaunchHandler(_loginControl_ShowOfflineLaunch);
				_loginControl.ShowVirtualMachineInfo += new Client.Controls.Login.ShowVirtualMachineInfoHandler(_loginControl_ShowVirtualMachineInfo);
				_loginControl.Dock = DockStyle.Fill;

				_accountControlsPanel.Controls.Add(_loginControl);
			}
		}

		void _loginControl_ShowVirtualMachineInfo(bool closeParentWindow)
		{
			this.ShowVirtualMachineInfo(closeParentWindow);
		}

		void _loginControl_ShowOfflineLaunch(string accountAttemptingToLogin, string oldestLinkedAccount, bool closeParentWindow)
		{
			this.ShowOfflineLaunchOption(accountAttemptingToLogin, oldestLinkedAccount, closeParentWindow);
		}

		private delegate void AttachNewAccountControlDelegate();
		private void AttachNewAccountControl()
		{
			if (this.InvokeRequired == true)
			{
				this.Invoke(new AttachNewAccountControlDelegate(AttachNewAccountControl));
			}
			else
			{
				_accountControlsPanel.Controls.Clear();

				_newAccountControl = new Client.Controls.NewAccount();
				_newAccountControl.CancelAccountSignup += new Client.Controls.NewAccount.CancelAccountSignupHandler(_newAccountControl_CancelAccountSignup);
				_newAccountControl.AccountCreatedSuccess += new Client.Controls.NewAccount.AccountCreatedSuccessHandler(_newAccountControl_AccountCreatedSuccess);
				_newAccountControl.Dock = DockStyle.Fill;

				_accountControlsPanel.Controls.Add(_newAccountControl);
			}
			//_newAccountControl.Visible = false;
		}

		void _newAccountControl_AccountCreatedSuccess(string username, string password)
		{
			AttachLoginControl();

			_loginControl.Username = username;
			_loginControl.Password = password;
		}

		void _newAccountControl_CancelAccountSignup()
		{
			AttachLoginControl();
		}

		void _loginControl_ShowNewAccountSignUp()
		{
			AttachNewAccountControl();
		}

		void _loginControl_LoginComplete(CheckInStatus checkInStatus)
		{
			StatusTimer.Stop();

			this.LauncherSignInStatus = checkInStatus;
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		void _loginControl_StatusChanged(string statusMessage, int percentage)
		{
			SetStatusBar(statusMessage, percentage);
		}
    }
}