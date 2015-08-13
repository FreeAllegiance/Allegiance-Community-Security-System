using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Allegiance.CommunitySecuritySystem.Client
{
	public partial class VirtualMachineInfo : Form
	{
		public VirtualMachineInfo()
		{
			InitializeComponent();
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnShowInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("http://www.freeallegiance.org/FAW/index.php/Virtual_Machine");
		}
	}
}
