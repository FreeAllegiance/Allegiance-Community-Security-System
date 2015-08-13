using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Allegiance.CommunitySecuritySystem.Client.Service;

namespace Allegiance.CommunitySecuritySystem.Client
{
	public partial class UpdateCheckForm : Form
	{
		// For some reason, adding this control to the designer causes VS2010sp1 to crash out.
		// So I will do it by hand for now, at some point this should be moved back to the designer (VS2012?)
		private Controls.UpdateCheckControl _updateCheckControl = new Controls.UpdateCheckControl();

		public UpdateCheckForm()
		{
			InitializeComponent();
		}

		private void UpdateCheckForm_Load(object sender, EventArgs e)
		{
			_updateCheckControl.Dock = DockStyle.Fill;
			_updateCheckControl.AutoupdateComplete += new Client.Controls.UpdateCheckControl.AutoupdateCompleteHandler(_updateCheckControl_AutoupdateComplete);

			this.Controls.Add(_updateCheckControl);
		}

		void _updateCheckControl_AutoupdateComplete(bool updateWasAborted)
		{
			if(updateWasAborted == true)
				this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			else
				this.DialogResult = System.Windows.Forms.DialogResult.OK;

			this.Close();
		}

		public bool HasPendingUpdates
		{
			get
			{
				return _updateCheckControl.HasPendingUpdates;
			}
		}

		public PendingUpdates PendingUpdates
		{
			get
			{
				return _updateCheckControl.PendingUpdates;
			}
		}
	}
}
