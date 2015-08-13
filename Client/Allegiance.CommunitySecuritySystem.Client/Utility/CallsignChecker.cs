using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Allegiance.CommunitySecuritySystem.Client.Service;
using Allegiance.CommunitySecuritySystem.Client.ClientService;

namespace Allegiance.CommunitySecuritySystem.Client.Utility
{
	class CallsignChecker
	{
		private const int CheckCallsignInterval = 700; // milliseconds

		private Timer _timer = null;
		private System.Threading.Thread _activeThread = null;

		public delegate void SetCheckMessageDelegate(string message);

		private SetCheckMessageDelegate _setCheckMessage;
		private TextBox _callsignTextBox;
		private ErrorProvider _errorProvider;
		private Button _continueButton;
		private Panel _legacyPasswordPanel;

		public CallsignChecker(TextBox callsignTextBox, ErrorProvider errorProvider, Button continueButton, SetCheckMessageDelegate setCheckMessage)
			: this(callsignTextBox, errorProvider, continueButton, setCheckMessage, new Panel())
		{
		}

		public CallsignChecker(TextBox callsignTextBox, ErrorProvider errorProvider, Button continueButton, SetCheckMessageDelegate setCheckMessage, Panel legacyPasswordPanel)
		{
			_callsignTextBox = callsignTextBox;
			_errorProvider = errorProvider;
			_continueButton = continueButton;
			_setCheckMessage = setCheckMessage;
			_legacyPasswordPanel = legacyPasswordPanel;
		}

		/// <summary>
		/// If multiple requests are made to this method before the timer elapses, then the old
		/// requests are discarded. 
		/// </summary>
		/// <param name="callsignTextBox"></param>
		/// <param name="errorProvider"></param>
		/// <param name="continueButton"></param>
		public void RequestCallsignCheck()
		{
			//Start a timer to check if the callsign is available
			_continueButton.Enabled = false;
			_legacyPasswordPanel.Visible = false;
			_setCheckMessage(String.Empty);

			if (_timer == null)
			{
				_timer = new Timer();
				_timer.Interval = CheckCallsignInterval;
				_timer.Tick += new EventHandler(_timer_Tick);
			}
			else // If a timer was already running, and the user pressed another key, then cancel the old timer, and restart a new one.
				_timer.Stop();

			//Re-start the timer
			_timer.Start();
		}

		void _timer_Tick(object sender, EventArgs e)
		{
			string usernameAvailableMessage = "Callsign is available";
			string usernameUnavailableMessage = "Callsign is unavailable";
			string usernameAlreadyRegisteredMessage = "Callsign is Already Registered";
			List<string> responseMessages = new List<string>(new string[] { usernameAvailableMessage, usernameUnavailableMessage, usernameAlreadyRegisteredMessage });

			_timer.Stop();

			if (_activeThread != null)
				_activeThread.Abort();

			if (_callsignTextBox.Text.Length == 0)
			{
				_activeThread = null;
				return;
			}

			string errorMessage;
			if (Validation.ValidateAlias(_callsignTextBox.Text, out errorMessage) == false)
			{
				_setCheckMessage(errorMessage);
				_errorProvider.SetError(_callsignTextBox, errorMessage);
			}
			else
			{
				_errorProvider.SetError(_callsignTextBox, "");
			}

			string currentValidationMessage = _errorProvider.GetError(_callsignTextBox);

			// If there is any validation message other than an account status message.
			if (responseMessages.Contains(currentValidationMessage) == false && string.IsNullOrEmpty(currentValidationMessage) == false)
				return;

			_setCheckMessage("Checking Availability...");

			//Check if requested callsign is available (every n milliseconds at most)
			var signal = new TaskDelegate(delegate(object input)
			{

				var result = (CheckAliasResult)input;
				var message = string.Empty;

				switch (result)
				{
					case CheckAliasResult.Available:
						message = usernameAvailableMessage;
						_continueButton.Enabled = true;
						_legacyPasswordPanel.Visible = false;
						_errorProvider.SetError(_callsignTextBox, "");
						break;

					case CheckAliasResult.Unavailable:
						message = usernameUnavailableMessage;
						_errorProvider.SetError(_callsignTextBox, usernameUnavailableMessage);
						break;

					case CheckAliasResult.Registered:
						message = usernameAlreadyRegisteredMessage;
						_errorProvider.SetError(_callsignTextBox, usernameAlreadyRegisteredMessage);
						break;

					case CheckAliasResult.InvalidLogin:
						message = "Invalid Credentials";
						break;

					case CheckAliasResult.AliasLimit:
						message = "You already have the maximum number of callsigns.";
						break;

					case CheckAliasResult.LegacyExists:
						message = "Please use ASGS password to create callsign!";
						_continueButton.Enabled = true;
						_legacyPasswordPanel.Visible = true;
						_errorProvider.SetError(_callsignTextBox, "");
						break;
				}

				_setCheckMessage(message);
			});

			_activeThread = Callsign.CheckUsernameAvailability(_callsignTextBox.Text, delegate(object data)
			{
				if (System.Threading.Thread.CurrentThread != _activeThread)
					return;

				if (_callsignTextBox.InvokeRequired)
					_callsignTextBox.Invoke(signal, data);
				else
					signal(data);
			});

		}

		public static bool ValidateCallsign(string alias, out string errorMessage)
		{
			errorMessage = String.Empty;

			var match = Regex.Match(alias,
				string.Concat(@"^(?<token>\W)?(?<callsign>[a-z]\w+)(?<tag>@\w+)?$"),
				RegexOptions.Compiled | RegexOptions.IgnoreCase);

			var token = match.Groups["token"].Value;
			var callsign = match.Groups["callsign"].Value;
			var tag = match.Groups["tag"].Value;

			if (callsign.Length < GlobalSettings.MinAliasLength)
				errorMessage = "The alias length is too small or contains invalid characters, " + GlobalSettings.MinAliasLength + " character minimum.";

			if (callsign.Length > GlobalSettings.MaxAliasLength)
				errorMessage = "The alias length is too large, " + GlobalSettings.MaxAliasLength + " character maximum.";

			return String.IsNullOrEmpty(errorMessage);
		}
	}
}
