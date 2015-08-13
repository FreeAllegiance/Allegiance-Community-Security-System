using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Allegiance.CommunitySecuritySystem.Client.Utility;
using Allegiance.CommunitySecuritySystem.Client.Service;

namespace Allegiance.CommunitySecuritySystem.Client.Controls
{
	public partial class UpdateCheckControl : UserControl
	{
		private bool _abortUpdate = false;

		private delegate void UiInteraction();

		public delegate void AutoupdateCompleteHandler(bool updateWasAborted);
		public event AutoupdateCompleteHandler AutoupdateComplete;

		private PendingUpdates _pendingUpdates = null;
		public PendingUpdates PendingUpdates
		{
			get
			{
				if(_pendingUpdates == null)
					_pendingUpdates = AutoUpdate.GetPendingUpdateQueues(ServiceHandler.Service);

				return _pendingUpdates;
			}
		}

		public bool HasPendingUpdates
		{
			get
			{
				return PendingUpdates.HasPendingUpdates;
			}
		}

		public UpdateCheckControl()
		{
			InitializeComponent();
		}

		private void UpdateCheck_Load(object sender, EventArgs e)
		{
			_updateStatusLabel.Text = String.Empty;

			TaskHandler.RunTask(delegate(object input)
			{
				object[] parameters = input as object[];
				UpdateCheckControl formReference = parameters[0] as UpdateCheckControl;

				DebugDetector.AssertCheckRunning();

				if (HasPendingUpdates == true)
				{
					
					var signal = new TaskDelegate(delegate(object data)
						{
							var updateCount = Convert.ToInt32(data);

							if (MessageBox.Show(new Form() { TopMost = true }, "Free Allegiance Updater has " + updateCount + " files to download.\nYou will not be able to play until these updates are installed.\nWould you like to do this now?", "New updates found!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
							{
								MessageBox.Show(new Form() { TopMost = true }, "Please restart the application when you are ready to apply updates.", "Update Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
								Application.Exit();
							}
						}); 

					if (this.InvokeRequired == true)
						this.Invoke(signal, PendingUpdates.PendingUpdateCount);
					else
						signal(PendingUpdates.PendingUpdateCount);
					
				}
				else
				{
					return;
				}

				//Get available lobbies
				UpdateProgress(String.Empty, "Checking lobby status...", 0);
				var lobbies = ServiceHandler.Service.CheckAvailableLobbies();

				foreach (var lobby in lobbies)
				{
					if (formReference._abortUpdate == true)
						break;

					UpdateProgress(lobby.Name, "Checking lobby for updates.", 0);

					if (AutoUpdate.ProcessPendingUpdates(PendingUpdates, lobby, new AutoUpdate.AutoupdateProgressCallback(UpdateProgress), this))
						return;
				}

				TaskDelegate onUpdateCompleteHandler = delegate(object data)
				{
					UpdateCheckControl updateCompleteFormReference = data as UpdateCheckControl;

					if (AutoupdateComplete != null)
						AutoupdateComplete(updateCompleteFormReference._abortUpdate);
				};

				if(formReference.InvokeRequired == true)
					formReference.Invoke(onUpdateCompleteHandler, formReference);
				else
					onUpdateCompleteHandler(formReference);

			}, this);
		}

		private delegate bool UpdateProgressDelegate(string lobbyName, string message, int completionPercentage);
		private bool UpdateProgress(string lobbyName, string message, int completionPercentage)
		{
			if (this.InvokeRequired == true)
			{
				this.Invoke(new UpdateProgressDelegate(UpdateProgress), lobbyName, message, completionPercentage);
				return _abortUpdate == false;
			}

			_updateStatusLabel.Text = String.Format("Updating {0} Lobby: {1}", lobbyName, message);

			if(completionPercentage >= _updateProgressBar.Minimum && completionPercentage <= _updateProgressBar.Maximum)
				_updateProgressBar.Value = completionPercentage;

			return _abortUpdate == false;
		}

		private void _exitWithoutUpdatingButton_Click(object sender, EventArgs e)
		{
			var signal = new UiInteraction(delegate()
			{
				if (MessageBox.Show("Are you sure you wish to exit Allegiance without updating? You will have to restart any updates already in progress.", "Cancel Update?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					_updateCheckLabel.Text = "Aborting updates, please wait...";
					_abortUpdate = true;
				}
			});

			if (this.InvokeRequired == true)
				this.Invoke(signal);
			else
				signal();
		}
	}
}
