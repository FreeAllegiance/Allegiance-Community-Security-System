using System;
using System.Drawing;
using System.Windows.Forms;
using Allegiance.CommunitySecuritySystem.Client.ClientService;
using Allegiance.CommunitySecuritySystem.Client.Service;
using Allegiance.CommunitySecuritySystem.Client.Utility;

namespace Allegiance.CommunitySecuritySystem.Client.Controls
{
    public partial class CreateCallsignControl : UserControl
    {
        #region Fields

		private CallsignChecker _callsignChecker;

        private const int CheckCallsignInterval = 700; //milliseconds

        #endregion

        #region Event Declarations

        internal event EventHandler CloseClick
        {
            add { _cancelButton.Click += value; }
            remove { _cancelButton.Click -= value; }
        }

        internal event TaskEventHandler<string> CallsignCreated;

        #endregion

        #region Constructors

        public CreateCallsignControl()
        {
            InitializeComponent();

			_callsignChecker = new CallsignChecker(_newCallsignTextBox, _errorProvider, _createButton, new CallsignChecker.SetCheckMessageDelegate(SetCheckMessage), _legacyPasswordPanel);
        }

        #endregion

        #region Events

        private void _newCallsignTextBox_TextChanged(object sender, EventArgs e)
        {
			_errorProvider.SetError(_asgsPasswordTextbox, "");

			_callsignChecker.RequestCallsignCheck();
        }

		private void _asgsPasswordTextbox_TextChanged(object sender, EventArgs e)
		{
			_errorProvider.SetError(_asgsPasswordTextbox, "");
		}

        
        private void _createButton_Click(object sender, EventArgs e)
        {
			if (_legacyPasswordPanel.Visible == true &&  String.IsNullOrEmpty(_asgsPasswordTextbox.Text) == true)
			{
				_errorProvider.SetError(_asgsPasswordTextbox, "Please specify your ASGS password to claim this callsign.");
				return;
			}

            //Lock the form
            SetEnabled(false);

            var signal = new TaskDelegate(delegate(object input)
            {
				var callsignCreationResult = (CheckAliasResult)input;

				switch (callsignCreationResult)
				{
					case CheckAliasResult.Registered:
						if (CallsignCreated != null)
							CallsignCreated(this, _newCallsignTextBox.Text);

						break;

					case CheckAliasResult.CaptchaFailed:
						SetCheckMessage("Invalid captcha code specified, callsign not created.");
						break;

					case CheckAliasResult.InvalidLogin:
						SetCheckMessage("Invalid login, callsign not created.");
						break;

					case CheckAliasResult.Unavailable:
						SetCheckMessage("This callsign is no longer available.");
						break;

					case CheckAliasResult.AliasLimit:
						SetCheckMessage("You already have the maximum number of aliases.");
						break;

					case CheckAliasResult.InvalidLegacyPassword:
						SetCheckMessage("Invalid ASGS password specified.");
						MessageBox.Show("You are tring to register a callsign that was used in the previous security system. You will need to use your original password to unlock your old callsign. If you do not have this password, please contact the administrators through the forums.");
						break;

					default:
						SetCheckMessage("Callsign not created: " + callsignCreationResult.ToString());
						break;
				}
				
				SetEnabled(true);
            });

            //Ask server to create this callsign
            Callsign.CheckAvailability(_newCallsignTextBox.Text, _asgsPasswordTextbox.Text, true, delegate(object data)
            {
                if (this.InvokeRequired)
                    this.Invoke(signal, data);
                else
                    signal(data);
            });
        }

        #endregion

        #region Methods

        private void SetCheckMessage(string message)
        {
			MainForm.SetStatusBar(message);
        }

        private void SetEnabled(bool enabled)
        {
            _cancelButton.Enabled       = enabled;
            _createButton.Enabled       = enabled;
            _newCallsignTextBox.Enabled = enabled;
        }

		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (keyData == Keys.Enter)
			{
				if (_createButton.Enabled == true)
				{
					_createButton_Click(this, EventArgs.Empty);
					return true;
				}
			}

			return base.ProcessDialogKey(keyData);
		}

        #endregion



		//private void _newCallsignTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		//{
		//    string errorMessage = string.Empty;

		//    if (String.IsNullOrEmpty(_newCallsignTextBox.Text) == false)
		//        Validation.ValidateAlias(_newCallsignTextBox.Text, out errorMessage);

		//    _errorProvider.SetError(_newCallsignTextBox, errorMessage);
		//    MainForm.SetStatusBar(errorMessage);
		//}
    }
}