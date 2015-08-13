using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Allegiance.CommunitySecuritySystem.Client.Integration;

namespace Allegiance.CommunitySecuritySystem.Client.Install
{
	public partial class CopyProgress : Form
	{
		public string SourceDirectory;
		public string TargetDirectory;
		public LobbyType LobbyType;

		public CopyProgress()
		{
			InitializeComponent();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Are you sure you wish to cancel the installation?", "Cancel Installation?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				bwtBackgroundWorkerThread.CancelAsync();
				this.DialogResult = DialogResult.Abort;
				this.Close();
			}
		}

		private void CopyProgress_Load(object sender, EventArgs e)
		{
			this.Text = "Copying: " + SourceDirectory + " for " + LobbyType.ToString();
			
			bwtBackgroundWorkerThread.RunWorkerAsync(this);

			System.Threading.Thread.Sleep(1000);
			this.SendToBack();
			this.WindowState = FormWindowState.Minimized;
			this.WindowState = FormWindowState.Normal;
			this.BringToFront();
		}

		private void bwtBackgroundWorkerThread_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = (BackgroundWorker) sender;
			CopyProgress hostForm = (CopyProgress) e.Argument;
			List<string> filesToProcess = GetFilesToProcess(hostForm.SourceDirectory);

			//Debugger.Break();

			int currentFileCounter = 1;
			foreach(string fileToProcess in filesToProcess)
			{
				if (backgroundWorker.CancellationPending == true)
				{
					e.Cancel = true;
					break;
				}

				string relativePath = fileToProcess.Substring(hostForm.SourceDirectory.Length);
				if (relativePath.StartsWith("\\") == true)
					relativePath = relativePath.Substring(1);

				string targetFile = Path.Combine(TargetDirectory, relativePath);

				hostForm.SetCurrentFile(fileToProcess);

				if (File.Exists(targetFile) == false)
				{
					if (Directory.Exists(Path.GetDirectoryName(targetFile)) == false)
						Directory.CreateDirectory(Path.GetDirectoryName(targetFile));

					File.Copy(fileToProcess, targetFile, true);
				}

				int currentProgressPercent = (int)Math.Floor((double)currentFileCounter / (double)filesToProcess.Count * 100D);

				if(currentProgressPercent > 100)
					currentProgressPercent = 100;

				System.Diagnostics.Debug.WriteLine(currentProgressPercent);

				backgroundWorker.ReportProgress(currentProgressPercent);

				currentFileCounter++;
			}

			e.Result = true;
		}

		private void bwtBackgroundWorkerThread_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			this.pbProgress.Value = e.ProgressPercentage;
		}

		private void bwtBackgroundWorkerThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Cancelled == false)
			{
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}


		private List<string> GetFilesToProcess(string sourceDirectory)
		{
			List<string> returnValue = new List<string>();

			foreach (string directory in Directory.GetDirectories(sourceDirectory))
			{
				// Due to R6 file collisions, the dev directory can contain incompatible files from the R6 beta that will
				// break the R5 build that CSS is working with. For now, skip all Dev directories in the production folder.
				if (directory.EndsWith("\\dev", StringComparison.CurrentCultureIgnoreCase) == true)
					continue;

				returnValue.AddRange(GetFilesToProcess(directory));
			}


			foreach (string filename in Directory.GetFiles(sourceDirectory))
				returnValue.Add(filename);

			return returnValue;
		}

		private delegate void SetCurrentFileDelegate(string filename);
		public void SetCurrentFile(string currentFilename)
		{
			if (this.InvokeRequired == true)
			{
				this.Invoke(new SetCurrentFileDelegate(delegate(string filename)
				{
					SetCurrentFile(filename);
				}), new object[] { currentFilename });

				return;
			}

			lblCurrentFile.Text = currentFilename;
		}
	}
}
