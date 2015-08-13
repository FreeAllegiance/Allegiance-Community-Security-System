using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Allegiance.CommunitySecuritySystem.Client.Integration;
using Allegiance.CommunitySecuritySystem.Client.Service;
using Allegiance.CommunitySecuritySystem.Client.Utility;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Allegiance.CommunitySecuritySystem.Client.ClientService;

namespace Allegiance.CommunitySecuritySystem.Client
{
    public partial class PlayControl : UserControl
    {
        #region Fields


		const int MaximumMissedCheckInsBeforeLogout = 10;
        const int CheckInInterval   = 60000;

        private bool _loggedIn = false;
		private int _missedCheckInCounter = 0;

		public delegate void ManageCallsignsClickHandler();
		public event ManageCallsignsClickHandler ManageCallsignsClick;

		public delegate void AllegianceExitedHandler();
		public event AllegianceExitedHandler AllegianceExited;

		public delegate void RequestUpdateCheckHandler(LobbyType lobbyType, LoginToLobbyCallback loginToLobbyCallback );
		public event RequestUpdateCheckHandler RequestUpdateCheck;

		public delegate void LoginToLobbyCallback(LobbyType lobbyType);

		public delegate void ReloadCallsignsHandler();
		public event ReloadCallsignsHandler ReloadCallsigns;

        #endregion

        #region Properties

        public Timer CheckInTimer { get; set; }

        public bool LoggedIn
        {
            get { return _loggedIn; }
        }

		private LobbyResult[] _availableLobbies = null;
		public LobbyResult[] AvailableLobbies
		{
			get
			{
				if (_availableLobbies == null)
					_availableLobbies = ServiceHandler.Service.CheckAvailableLobbies();

				return _availableLobbies;
			}
		}

        #endregion

        #region Constructors

        public PlayControl()
        {
            InitializeComponent();

			if (IsLobbyAvailable(LobbyType.Production) == false)
				_playOnlineButton.Enabled = false;

			if (IsLobbyAvailable(LobbyType.Beta) == false)
				_playBetaButton.Enabled = false;
        }

        #endregion

        #region Events

        private void _playOnlineButton_Click(object sender, System.EventArgs e)
        {
			UpdateArtPathAndCheckForUpdatesAndLoginToLobby(LobbyType.Production);
        }

        private void _playBetaButton_Click(object sender, EventArgs e)
        {
			UpdateArtPathAndCheckForUpdatesAndLoginToLobby(LobbyType.Beta);
        }

        void CheckInTimer_Tick(object sender, System.EventArgs e)
        {
            DebugDetector.AssertCheckRunning();

            MainForm.SetStatusBar("Checking in...");

            var message = string.Empty;
            var ticket  = string.Empty;
            var data    = SessionNegotiator.GenerateCheckInData();
            var result  = SessionNegotiator.CheckIn(data, ref message, ref ticket);

			if (result != CheckInStatus.Ok)
			{
				if (result == CheckInStatus.Timeout)
					_missedCheckInCounter++;

				if (_missedCheckInCounter >= MaximumMissedCheckInsBeforeLogout || result != CheckInStatus.Timeout)
				{
					//Close Allegiance
					AllegianceLoader.ExitAllegiance();

					//Unlock form
					this.Enabled = true;

					MainForm.SetStatusBar(message);
				}
			}
			else
			{
				MainForm.SetStatusBar(string.Format("Checked in at {0:h:mm tt}.", DateTime.Now));
				_missedCheckInCounter = 0;
			}

            GC.Collect(); //Now is a decent time to force garbage collection
        }

        #endregion

        #region Methods

		private bool IsLobbyAvailable(LobbyType lobbyType)
		{
			foreach (LobbyResult lobby in AvailableLobbies)
			{
				if (lobby.Name.Equals(lobbyType.ToString(), StringComparison.InvariantCultureIgnoreCase) == true)
					return true;
			}

			return false;
		}

		internal void UpdateArtPathAndCheckForUpdatesAndLoginToLobby(LobbyType lobbyType)
		{
			this.Enabled = false;

			switch (lobbyType)
			{
				case LobbyType.Production:
					AllegianceRegistry.ArtPath = AllegianceRegistry.ProductionArtPath;
					AllegianceRegistry.CfgFile = AllegianceRegistry.ProductionCfgFile;
					break;

				case LobbyType.Beta:
					AllegianceRegistry.ArtPath = AllegianceRegistry.BetaArtPath;
					AllegianceRegistry.CfgFile = AllegianceRegistry.BetaCfgFile;
					break;

				default:
					throw new NotSupportedException(lobbyType.ToString());
			}

			// Don't request updates for offline play.
			if (RequestUpdateCheck != null && lobbyType != LobbyType.None)
				RequestUpdateCheck(lobbyType, LoginToLobby);
			else
				LoginToLobby(lobbyType);
		}

        internal void LoginToLobby(LobbyType lobbyType)
        {
            //Verify the form is filled out
            if (string.IsNullOrEmpty(_loginComboBox.Text))
                return;

			// TODO: Figure out where the alleg exe is and send it to the launcher.

            //Create a new session
            var signal = new TaskDelegate(delegate(object input)
            {
                var parameters  = input as object[];
                var status   = (CheckInStatus)parameters[0];
                var message     = parameters[1] as string;
                var alias       = parameters[2] as string;
                var ticket      = parameters[3] as string;

				int rank = 0;

				if (parameters[4] != null)
					rank = (int)parameters[4];

				Regex aliasFinder = new Regex(
					  "(?<callsign>.*?)(\\(\\d+\\))?$",
					RegexOptions.ExplicitCapture
					| RegexOptions.CultureInvariant
					| RegexOptions.Compiled
					);

				var match = aliasFinder.Match(alias);
				if (match.Success == true)
					alias = match.Groups["callsign"].Value;

				if (status == CheckInStatus.AccountLinked)
				{
					if (ReloadCallsigns != null)
						ReloadCallsigns();
				}

				if (status == CheckInStatus.Ok || status == CheckInStatus.AccountLinked)
                {
                    //Initialize check-in interval
                    CheckInTimer            = new Timer();
                    CheckInTimer.Interval   = CheckInInterval;
                    CheckInTimer.Tick       += new EventHandler(CheckInTimer_Tick);
                    CheckInTimer.Start();

                    //Launch Allegiance
                    MainForm.SetStatusBar("Launching Allegiance...");

                    //Store last-used alias
                    if (!string.Equals(DataStore.LastAlias, alias))
                    {
                        DataStore.LastAlias = alias;
                        DataStore.Instance.Save();
                    }

					if (rank <= 5)
						alias += "(" + rank + ")";


                    AllegianceLoader.StartAllegiance(ticket, lobbyType, alias, delegate(object param)
                    {
                        var result = (bool)param;

                        if (!result)
                        {
                            Logout();

                            MainForm.SetStatusBar("Failed to launch Allegiance.");
                        }
                        else
                        {
                            //AllegianceLoader.AllegianceProcess.OnExiting
                              //  += new EventHandler(AllegianceProcess_OnExiting);

							AllegianceLoader.AllegianceExit
								+= new EventHandler(AllegianceProcess_OnExiting);
                        }
                    });

                    SetLoggedIn(true);

                    if (DataStore.Preferences.AutoLogin)
                        MainForm.HideForm();
                }
                else
                {
                    MainForm.SetStatusBar(message);

					if (status == CheckInStatus.VirtualMachineBlocked)
					{
						VirtualMachineInfo virtualMachineInfo = new VirtualMachineInfo();
						virtualMachineInfo.ShowDialog();
					}

					this.Enabled = true;

                    SystemWatcher.Close();
                }
            });

            SessionNegotiator.Login(_loginComboBox.Text, lobbyType.ToString(), delegate(object input)
            {
                if (this.InvokeRequired)
                    this.Invoke(signal, input);
                else
                    signal(input);
            });
        }

        void AllegianceProcess_OnExiting(object sender, EventArgs e)
        {
			Log.Write("PlayControl::AllegianceProcess_OnExiting - Logout.");

            //Logout
            Logout(false);

			if (AllegianceExited != null)
			{
				AllegianceExited();

				Log.Write("PlayControl::AllegianceProcess_OnExiting - AllegianceExited called.");
			}
        }

        internal void FillCallsignDropdown(List<Callsign> callsigns)
        {
            _loginComboBox.Items.Clear();

            var storedDefault = DataStore.LastAlias;

            foreach (var callsign in callsigns)
            {
                int index = _loginComboBox.Items.Add(callsign);

                if (callsign.Default && string.IsNullOrEmpty(storedDefault))
                    storedDefault = callsign.Name;
            }

            if (!string.IsNullOrEmpty(storedDefault))
                _loginComboBox.Text = storedDefault;
        }

        public void Logout()
        {
            Logout(true);
        }

        private void Logout(bool closeAllegiance)
        {
            MainForm.SetStatusBar("Logged out");

            //Stop timer
            CheckInTimer.Stop();

            //Close Allegiance
            if(closeAllegiance)
                AllegianceLoader.ExitAllegiance();

            //Tell the server you are logging out
            SessionNegotiator.Logout(false);

            SetLoggedIn(false);
        }

		private delegate void SetLoggedInDelegate(bool loggedIn);
        private void SetLoggedIn(bool loggedIn)
        {
			if (this.InvokeRequired == true)
			{
				this.Invoke(new SetLoggedInDelegate(delegate(bool innerLoggedIn)
				{
					this.SetLoggedIn(innerLoggedIn);
				}), loggedIn);

				return;
			}

            if(!loggedIn)
                this.Enabled = true;

			_playmodeGroupbox.Visible = loggedIn == false;
			_callsignGroupbox.Visible = loggedIn == false;
			_allegianceRunningGroupBox.Visible = loggedIn == true;

			this.Enabled = true;

            _loggedIn = loggedIn;
        }

        #endregion

		private void _loginComboBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			ComboBox loginComboBox = (ComboBox)sender;

			string errorMessage;
			Validation.ValidateAlias(loginComboBox.Text, out errorMessage);

			playControlErrorProvider.SetError(loginComboBox, errorMessage);

			if (String.IsNullOrEmpty(errorMessage) == false)
			{
				MainForm.SetStatusBar(errorMessage);
				e.Cancel = true;
			}
		}

		private void manageCallsignsButton_Click(object sender, EventArgs e)
		{
			if (ManageCallsignsClick != null)
				ManageCallsignsClick();
		}

		private void learnMoreLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("http://www.freeallegiance.org/FAW/index.php/Callsigns");
		}

		private void _exitAllegianceButton_Click(object sender, EventArgs e)
		{
			Logout();
		}

		private void _playOfflineButton_Click(object sender, EventArgs e)
		{
			//LoginToLobby(LobbyType.None);

			this.Enabled = false;

			AllegianceLoader.StartAllegiance(String.Empty, LobbyType.Production, String.Empty, new Utility.TaskDelegate(delegate(Object context)
			{
				AllegianceLoader.AllegianceExit += new EventHandler(AllegianceLoader_AllegianceExit);
			}));
		}

		void AllegianceLoader_AllegianceExit(object sender, EventArgs e)
		{
			if (this.InvokeRequired == true)
				this.Invoke(new EventHandler(AllegianceLoader_AllegianceExit), sender, e);

			this.Enabled = true;
		}
    }
}