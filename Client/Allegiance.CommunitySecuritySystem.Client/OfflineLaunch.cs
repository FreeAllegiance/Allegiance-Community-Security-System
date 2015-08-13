using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using Allegiance.CommunitySecuritySystem.Client.Integration;

namespace Allegiance.CommunitySecuritySystem.Client
{
	public partial class OfflineLaunch : Form
	{
		private string _helpLinkUrl;

		public OfflineLaunch()
		{
			InitializeComponent();
		}

		public OfflineLaunch(string prompt, string helpLinkUrl, string helpLinkName) : this()
		{
			_promptLabel.Text = prompt;
			_helpLinkLinkLabel.Text = helpLinkName;
			_helpLinkUrl = helpLinkUrl;

			if (String.IsNullOrEmpty(helpLinkUrl) == true)
				_helpLinkLinkLabel.Visible = false;
		}

		private void _helpLinkLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(_helpLinkUrl);
		}

		private void _playOfflineButton_Click(object sender, EventArgs e)
		{
			string allegiancePath = Path.Combine(AllegianceRegistry.LobbyPath, LobbyType.Production.ToString());
			string exePath = Path.Combine(allegiancePath, "Allegiance.exe");
			Process.Start(exePath);
			this.Close();
		}

		private void _retryLoginButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
