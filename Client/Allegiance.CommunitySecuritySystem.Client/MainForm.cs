using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Allegiance.CommunitySecuritySystem.Client.Controls;
using Allegiance.CommunitySecuritySystem.Client.Service;
using Allegiance.CommunitySecuritySystem.Client.Utility;
using Allegiance.CommunitySecuritySystem.Client.Integration;
using Allegiance.CommunitySecuritySystem.Client.ClientService;

namespace Allegiance.CommunitySecuritySystem.Client
{
    public partial class MainForm : Form
    {
        #region Fields

		private CheckInStatus _launcherSignInStatus;

        private const int StatusClearInterval = 7000;

		private PollDisplayControl _pollDisplayControl = new PollDisplayControl();
		private MessageSingleControl _messageSingleControl = new MessageSingleControl();
		private CreateCallsignControl _createCallsignControl;
		private UpdateCheckControl _updateCheckControl;
		private PlayControl _playControl = new PlayControl();

		private bool _exiting = false;
		private bool _restart = false;

        #endregion

        #region Properties

        protected static MainForm FormInstance { get; set; }

        protected Timer StatusTimer { get; set; }

        public static bool LoggedIn
        {
            get { return FormInstance._playControl.LoggedIn; }
        }

        public static TabPage MessagesTabPage
        {
            get { return FormInstance._messagesTabPage; }
        }

        public static MessageSingleControl ViewMessageControl
        {
            get { return FormInstance._messageSingleControl; }
        }

		public static bool Restart
		{
			get { return FormInstance._restart; }
		}

        #endregion

        #region Constructors

		public MainForm() : this(CheckInStatus.Ok)
		{

		}

        public MainForm(CheckInStatus launcherSignInStatus)
        {
			_launcherSignInStatus = launcherSignInStatus;

            DebugDetector.AssertCheckRunning();

            InitializeComponent();
            FormInstance = this;

            StatusTimer             = new Timer();
            StatusTimer.Interval    = StatusClearInterval;
            StatusTimer.Tick        += new EventHandler(StatusTimer_Tick);

            //Bind Events
            _messageListControl.OpenClick       += new EventHandler(_messageListControl_OpenClick);
            _callsignControl.CreateClick        += new EventHandler(_callsignControl_CreateClick);
            _callsignControl.CallsignsLoaded    += new TaskEventHandler<List<Callsign>>(_callsignControl_CallsignsLoaded);
			_playControl.ManageCallsignsClick	+= new PlayControl.ManageCallsignsClickHandler(_playControl_ManageCallsignsClick);
			_playControl.AllegianceExited		+= new PlayControl.AllegianceExitedHandler(_playControl_AllegianceExited);
			_playControl.RequestUpdateCheck		+= new PlayControl.RequestUpdateCheckHandler(_playControl_RequestUpdateCheck);
			_playControl.ReloadCallsigns		+= new PlayControl.ReloadCallsignsHandler(_playControl_ReloadCallsigns);

			_messageListControl.MessageReceived += new EventHandler(_messageListControl_MessageReceived);

            //Load Preferences from datastore
            LoadPreferences();

			// Reset the cached callsign list so that it will reload from the server when the app is first started,
			// or if the user re-logs in.
			//DataStore.Callsigns = null;

            //Load form data
            _callsignControl.LoadCallsigns(true);
            _messageListControl.LoadMessages();

            //Check for polls
            _pollDisplayControl.PollComplete += new EventHandler(_pollDisplayControl_PollComplete);
            CheckPolls();
			
            //Auto-Login
            if (DataStore.Preferences.AutoLogin)
                Autologin();
        }

		void _playControl_ReloadCallsigns()
		{
			var signal = new TaskDelegate(delegate(object data)
				{
					_callsignControl.LoadCallsigns(true);
				});

			if(_callsignControl.InvokeRequired == true)
				_callsignControl.Invoke(signal);
			else
				signal(null);
		}

		void _playControl_RequestUpdateCheck(LobbyType lobbyType, PlayControl.LoginToLobbyCallback loginToLobbyCallback)
		{
			_updateCheckControl = new UpdateCheckControl();
			if (_updateCheckControl.HasPendingUpdates == true)
			{
				SystemWatcher.Close();

				_playControl.Visible = false;

				_updateCheckControl.Parent = _mainTabControl.TabPages[0];
				_updateCheckControl.Dock = DockStyle.Fill;
				_updateCheckControl.AutoupdateComplete += new UpdateCheckControl.AutoupdateCompleteHandler(delegate(bool updateCanceled)
					{
						_updateCheckControl.Visible = false;
						_playControl.Visible = true;

						if (updateCanceled == false)
						{
							SystemWatcher.InitializeWithAutoupdateProtectedFileList();

							loginToLobbyCallback(lobbyType);
						}
					});

				_updateCheckControl.Show();
			}
			else
			{
				SystemWatcher.InitializeWithAutoupdateProtectedFileList();

				loginToLobbyCallback(lobbyType);
			}
		}

        #endregion

        #region Events

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
			if (e.CloseReason == CloseReason.UserClosing && !_exiting && AllegianceLoader.AllegianceProcess != null && AllegianceLoader.AllegianceProcess.IsProcessAvailable() == true)
            {
                e.Cancel = true;
                showToolStripMenuItem_Click(sender, EventArgs.Empty);
            }
			else
			{
				this.StatusTimer.Stop();
			}
		}

        private void _notificationIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
			FormInstance.showToolStripMenuItem_Click(FormInstance, EventArgs.Empty);
        }

        #region Message Tab Events

        private void _messageListControl_OpenClick(object sender, EventArgs e)
        {
            _messageListControl.Visible         = false;
            _messageSingleControl.Parent        = _mainTabControl.TabPages[1];
            _messageSingleControl.CloseClick    += new EventHandler(_messageSingleControl_CloseClick);
            _messageSingleControl.Show();
        }

		void _messageListControl_MessageReceived(object sender, EventArgs e)
		{
			_mainTabControl.SelectedTab = _messagesTabPage;
		}


        private void _messageSingleControl_CloseClick(object sender, EventArgs e)
        {
            _messageListControl.Visible = true;
        }

        void StatusTimer_Tick(object sender, EventArgs e)
        {
            var pb          = FormInstance._mainStatusStrip.Items[1] as ToolStripProgressBar;
            var percentage  = pb.Value;

            if(percentage == 0)
                SetStatusBar(string.Empty);
        }

        #endregion

		void _playControl_ManageCallsignsClick()
		{
			_mainTabControl.SelectedTab = _callsignsTabPage;
		}

		void _playControl_AllegianceExited()
		{
			SetStatusBar("Allegiance Exited.");


			var signal = new TaskDelegate(delegate(object parameter)
			{
				if (this.Visible == false)
					Close();
			});

			if (FormInstance.InvokeRequired)
                FormInstance.Invoke(signal, this);
            else
                signal(this);
			
		}

        #region Callsign Tab Events

        private void _callsignControl_CreateClick(object sender, EventArgs e)
        {
            _callsignControl.Visible                = false;
            _createCallsignControl                  = new CreateCallsignControl();
            _createCallsignControl.Parent           = _mainTabControl.TabPages[2];
            _createCallsignControl.CloseClick       += new EventHandler(_createCallsignForm_CloseClick);
            _createCallsignControl.CallsignCreated  += new TaskEventHandler<string>(_createCallsignControl_CallsignCreated);
            _createCallsignControl.Show();
        }

        void _createCallsignControl_CallsignCreated(object sender, string callsign)
        {
            //Add the new username to the datastore
            var callsigns = DataStore.Callsigns;
            callsigns.Add(new Callsign()
            {
                Active  = true,
                Default = false,
                Name    = callsign
            });

            //Save changes to datastore
            DataStore.Callsigns = callsigns;
            DataStore.Instance.Save();

            //Re-load the callsign list from the server so that the callsigns remaining counter gets updated correctly.
            //_callsignControl.LoadCallsigns(callsigns);
			_callsignControl.LoadCallsigns(true);

            //Hide the create callsign form
            _createCallsignForm_CloseClick(sender, EventArgs.Empty);
        }

        private void _createCallsignForm_CloseClick(object sender, EventArgs e)
        {
            _callsignControl.Visible = true;
            _createCallsignControl.Dispose();
        }

        void _callsignControl_CallsignsLoaded(object sender, List<Callsign> args)
        {
            _playControl.FillCallsignDropdown(args);
        }

        #endregion

        #region Menu Item Events

        //Links
        private void _squadRostersToolStripMenuItem_Click(object sender, EventArgs e)
        {
			Process.Start(AllegianceRegistry.GetManagementWebPath("Stats/SquadRoster.aspx"));
        }
        
        private void _leaderboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
			Process.Start(AllegianceRegistry.GetManagementWebPath("Stats/Leaderboard.aspx"));
        }

        private void _banlistToolStripMenuItem_Click(object sender, EventArgs e)
        {
			Process.Start(AllegianceRegistry.GetManagementWebPath("Stats/BanList.aspx?type=mostRecent"));
        }

        private void _troubleshootingToolStripMenuItem_Click(object sender, EventArgs e)
        {
			Process.Start("http://www.freeallegiance.org/FAW/index.php/Troubleshooting");
        }
        private void _newToAllegToolStripMenuItem_Click(object sender, EventArgs e)
        {
			Process.Start("http://www.freeallegiance.org/FAW/index.php/Main_Page");
        }
        private void _newToCSSToolStripMenuItem_Click(object sender, EventArgs e)
        {
			Process.Start("http://www.freeallegiance.org/FAW/index.php/CSS");
        }

        //Preferences
        private void _launchWindowedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _launchWindowedToolStripMenuItem.Checked    = !_launchWindowedToolStripMenuItem.Checked;
            DataStore.Preferences.LaunchWindowed        = _launchWindowedToolStripMenuItem.Checked;
            DataStore.Instance.Save();
        }

        private void _logChatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _logChatToolStripMenuItem.Checked   = !_logChatToolStripMenuItem.Checked;
            DataStore.Preferences.LogChat       = _logChatToolStripMenuItem.Checked;
            DataStore.Instance.Save();
        }

        private void _autoLoginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _autoLoginToolStripMenuItem.Checked = !_autoLoginToolStripMenuItem.Checked;
            DataStore.Preferences.AutoLogin     = _autoLoginToolStripMenuItem.Checked;
            DataStore.Instance.Save();
        }

        private void _safeModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _safeModeToolStripMenuItem.Checked  = !_safeModeToolStripMenuItem.Checked;
            DataStore.Preferences.SafeMode      = _safeModeToolStripMenuItem.Checked;
            DataStore.Instance.Save();
        }

        private void _debugLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _debugLogToolStripMenuItem.Checked = !_debugLogToolStripMenuItem.Checked;
            DataStore.Preferences.DebugLog = _debugLogToolStripMenuItem.Checked;
            DataStore.Instance.Save();
        }

        private void _noMoviesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _noMoviesToolStripMenuItem.Checked = !_noMoviesToolStripMenuItem.Checked;
            DataStore.Preferences.NoMovies = _noMoviesToolStripMenuItem.Checked;
            DataStore.Instance.Save();
        }

        //Other
        private void _systemInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var diagForm = new DiagnosticsForm();
            diagForm.Show();
        }

        private void _logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MainForm.LoggedIn)
                FormInstance._playControl.Logout();

            DataStore.Username = null;
            DataStore.Password = null;
            DataStore.Instance.Save();

			//_exiting = true;
            //Application.Restart();

			this._exiting = true;
			this._restart = true;
			this.DialogResult = DialogResult.OK;
			this.Close();
        }

        private void _exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
			_exiting = true;
            this.Close();
        }

        //Statusbar
        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                this.Show();
                showToolStripMenuItem.Text = "Hide";
				this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.Hide();
                showToolStripMenuItem.Text = "Show";
            }
        }

		private void _aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AboutBoxForm aboutBoxForm = new AboutBoxForm();
			aboutBoxForm.ShowDialog();
		}

		private void _cssDiagnosticsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CssDiagnosticsForm cssDiagnosticForm = new CssDiagnosticsForm();
			cssDiagnosticForm.ShowDialog();
		}

        #endregion

        #region Poll Events

        void _pollDisplayControl_PollComplete(object sender, EventArgs e)
        {
            this.PresentPoll(Allegiance.CommunitySecuritySystem.Client.Service.Poll.CurrentPolls);
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Retrieves preferences stored in the configuration
        /// </summary>
        private void LoadPreferences()
        {
            _launchWindowedToolStripMenuItem.Checked    = DataStore.Preferences.LaunchWindowed;
            _logChatToolStripMenuItem.Checked           = DataStore.Preferences.LogChat;
            _autoLoginToolStripMenuItem.Checked         = DataStore.Preferences.AutoLogin;
            _safeModeToolStripMenuItem.Checked          = DataStore.Preferences.SafeMode;
            _debugLogToolStripMenuItem.Checked          = DataStore.Preferences.DebugLog;
            _noMoviesToolStripMenuItem.Checked          = DataStore.Preferences.NoMovies;
        }

        public static void SetStatusBar(string text)
        {
            SetStatusBar(text, 0);
        }

        public static void SetStatusBar(string text, int percentage)
        {
			if (FormInstance == null)
				return; 

            var signal = new TaskDelegate(delegate(object parameter)
            {
                var msg = parameter as string;

				// BT - This was throwing an exception during launcher shutdown.
				// It's still a race, but maybe this is good enough.
				if (FormInstance._mainStatusStrip.Items.Count > 0)
				{
					FormInstance._mainStatusStrip.Items[0].Text = msg;

					var pb = FormInstance._mainStatusStrip.Items[1] as ToolStripProgressBar;
					pb.Value = percentage;
					pb.Visible = percentage > 0;
				}

				FormInstance.StatusTimer.Stop();
                FormInstance.StatusTimer.Start();

                if(!FormInstance.Visible)
                    FormInstance.DisplayNotificationMessage(msg);
            });

            if (FormInstance.InvokeRequired)
                FormInstance.Invoke(signal, text);
            else
                signal(text);
        }

        private void Autologin()
        {
            _playControl.LoginToLobby(LobbyType.Production);
        }

        private void CheckPolls()
        {
            var signal = new TaskDelegate(delegate(object data)
            {
                var list = data as List<ClientService.Poll>;
                if (list.Count > 0)
                {
                    PresentPoll(list);
                    SetStatusBar("Poll(s) Received");
                }
            });

            Allegiance.CommunitySecuritySystem.Client.Service.Poll.RetrievePolls(delegate(object data)
            {
                if (InvokeRequired)
                    Invoke(signal, data);
                else
                    signal(data);
            });
        }

        private void PresentPoll(List<ClientService.Poll> pollList)
        {
            //Remove poll from list and present it.
            if (pollList.Count > 0)
            {
                var poll = pollList[0];
                pollList.RemoveAt(0);

                _playControl.Visible = false;
                _pollDisplayControl.FillForm(poll);
                _pollDisplayControl.Parent = _mainTabControl.TabPages[0];
				_pollDisplayControl.Dock = DockStyle.Fill;
                _pollDisplayControl.Show();
            }
            else
            {
                _playControl.Visible        = true;
                _pollDisplayControl.Visible = false;
            }
        }

        private void DisplayNotificationMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                _notificationIcon.BalloonTipText = message;
                _notificationIcon.ShowBalloonTip(StatusClearInterval);
            }
        }

        internal static void HideForm()
        {
            FormInstance.showToolStripMenuItem_Click(FormInstance, EventArgs.Empty);
        }

        #endregion

		private void MainForm_Resize(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Minimized)
			{
				HideForm();
			}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			// Under VS2010, adding this control thru the designer will crash VS2010.
			_playTabPage.Controls.Clear();

			_playTabPage.Controls.Add(_playControl);
			_playControl.Dock = System.Windows.Forms.DockStyle.Fill;
		}


    }
}