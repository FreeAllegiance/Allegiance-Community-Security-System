using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace Allegiance.CommunitySecuritySystem.Client.Install
{
	public partial class VC2010Install : Form
	{
		public VC2010Install()
		{
			InitializeComponent();
		}

		private void VC2010Install_Load(object sender, EventArgs e)
		{
			// Force this window to rise above the install dialog.
			this.SendToBack();
			this.WindowState = FormWindowState.Minimized;
			this.WindowState = FormWindowState.Normal;
			this.BringToFront();
		}

		private void btnContinue_Click(object sender, EventArgs e)
		{
			if (rbInstallVc.Checked == true)
			{
				WebClient webClient = new WebClient();
				string tempFile = Path.Combine(Path.GetTempPath(), "vcredist_x86.exe");

				if (File.Exists(tempFile) == true)
					File.Delete(tempFile);

				try
				{
					this.Cursor = Cursors.WaitCursor;

					webClient.DownloadFile("http://download.microsoft.com/download/5/B/C/5BC5DBB3-652D-4DCE-B14A-475AB85EEF6E/vcredist_x86.exe", tempFile);

					if (File.Exists(tempFile) == true)
					{
						var process = Process.Start(tempFile);
						process.WaitForExit();

						if (process.ExitCode != 0)
						{
							this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
							this.Close();
						}
					}
				}
				catch
				{
					if (File.Exists(tempFile) == true)
						File.Delete(tempFile);
				}
				finally
				{
					this.Cursor = Cursors.Default;
				}

				this.DialogResult = DialogResult.OK;
			}
			else if (rbSkipInstall.Checked == true)
			{
				this.DialogResult = DialogResult.Ignore;
			}
			else
			{
				this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			}

			this.Close();
		}

		private void lnkDownloadLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("http://download.microsoft.com/download/5/B/C/5BC5DBB3-652D-4DCE-B14A-475AB85EEF6E/vcredist_x86.exe");
		}
	}
}
