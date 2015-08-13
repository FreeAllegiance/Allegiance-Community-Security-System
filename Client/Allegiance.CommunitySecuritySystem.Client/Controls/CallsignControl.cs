using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Allegiance.CommunitySecuritySystem.Client.Service;
using Allegiance.CommunitySecuritySystem.Client.Utility;

namespace Allegiance.CommunitySecuritySystem.Client.Controls
{
    public partial class CallsignControl : UserControl
    {
        #region Event Declarations

        public event EventHandler CreateClick
        {
            add { _createCallsignButton.Click += value; }
            remove { _createCallsignButton.Click -= value; }
        }

        internal event TaskEventHandler<List<Callsign>> CallsignsLoaded;

        #endregion

        #region Constructors

        public CallsignControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        private void _setDefaultButton_Click(object sender, EventArgs e)
        {
            if (_callsignListView.SelectedItems.Count == 0)
                return;

            var li      = _callsignListView.SelectedItems[0];
            var alias   = li.Tag as Callsign;

            //Overwrite the server's default callsign for this account locally
            if (alias != null)
            {
				Callsign.SetDefaultAlias(alias);
               
                LoadCallsigns(true);
            }
        }

        #endregion

        #region Methods

        internal void LoadCallsigns(bool reloadFromServer)
        {
			Allegiance.CommunitySecuritySystem.Client.Service.Callsign.RetrieveCallsignsCompleteDelegate signal = delegate(List<Callsign> callsigns, int availableAliasCount)
            {
				LoadCallsigns(callsigns);
				SetAvailableAliasControls(availableAliasCount);
                SetEnabled(true);
            };

			Callsign.RetrieveCallsigns(delegate(List<Callsign> callsigns, int availableAliasCount)
            {
                //Check if it is safe to interact with the form
                if (this.InvokeRequired)
					this.Invoke(signal, callsigns, availableAliasCount);
                else
					signal(callsigns, availableAliasCount);
            }, reloadFromServer);
        }

		private void SetAvailableAliasControls(int availableAliasCount)
		{
			if (availableAliasCount == 0)
			{
				_createCallsignButton.Visible = false;
				_aliasesRemainingLabel.Text = "You have no more callsigns remaining.";
			}
			else
			{
				_createCallsignButton.Visible = true;

				if (availableAliasCount > 100)
					_aliasesRemainingLabel.Text = "You have unlimited callsigns.";
				else if(availableAliasCount > 1)
					_aliasesRemainingLabel.Text = "You have " +  availableAliasCount + " callsigns remaining.";
				else
					_aliasesRemainingLabel.Text = "You have one callsign remaining.";
			}
		}

        internal void LoadCallsigns(List<Callsign> callsigns)
        {
            _callsignListView.Items.Clear();

			bool foundDefaultCallsign = false;

            foreach (var callsign in callsigns)
            {
				bool callsignIsDefault = false;
				if (foundDefaultCallsign == false && DataStore.LastAlias != null && DataStore.LastAlias == callsign.Name)
				{
					callsignIsDefault = true;
					foundDefaultCallsign = true;
				}

                var li = new ListViewItem(new string[] 
                {
                   callsignIsDefault    ? "Default" : string.Empty,
                    callsign.Name
                    //callsign.Active     ? "Active" : "Inactive" 
                });
                li.Tag = callsign;

				if (callsignIsDefault)
					li.Font = new System.Drawing.Font("Segoe UI", 8f, System.Drawing.FontStyle.Bold);
			
                _callsignListView.Items.Add(li);
            }

            if (CallsignsLoaded != null)
                CallsignsLoaded(this, callsigns);
        }

        private void SetEnabled(bool enabled)
        {
            _callsignListView.Enabled       = enabled;
            _createCallsignButton.Enabled   = enabled;
            _setDefaultButton.Enabled       = enabled;
        }

        #endregion
    }
}