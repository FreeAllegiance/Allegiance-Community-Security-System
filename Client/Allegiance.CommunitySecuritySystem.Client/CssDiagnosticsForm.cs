using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace Allegiance.CommunitySecuritySystem.Client
{
	public partial class CssDiagnosticsForm : Form
	{
		public CssDiagnosticsForm()
		{
			InitializeComponent();
		}

		private void CssDiagnosticsForm_Load(object sender, EventArgs e)
		{
			string registrySettings = GetAllegianceRegistrySettings();

			StringBuilder fileVersions = GetAllegianceFileVersions(Integration.AllegianceRegistry.LobbyPath);

			string lastLogOutput = GetLastLogs();

			_diagnosticsOutputTextbox.Text = String.Format(@"
Registry Information
====================
{0}



Directory Information
=====================
{1}



Last Logfile Output
===================
{2}
", registrySettings, fileVersions, lastLogOutput);
		}

		private string GetLastLogs()
		{
			StringBuilder returnValue = new StringBuilder();

			string outputLogPath = Path.Combine(Integration.AllegianceRegistry.LobbyPath, "output.log");
			if (File.Exists(outputLogPath) == true)
			{
				returnValue.AppendLine("Output Log: " + outputLogPath);
				returnValue.AppendLine("===============================================================================");
				returnValue.AppendLine(File.ReadAllText(outputLogPath));
				returnValue.AppendLine("");
				returnValue.AppendLine("");
				returnValue.AppendLine("");
			}

			string exceptionLogPath = Path.Combine(Integration.AllegianceRegistry.LobbyPath, "ExceptionLog.txt");
			if (File.Exists(exceptionLogPath) == true)
			{
				returnValue.AppendLine("Exception Log: " + exceptionLogPath);
				returnValue.AppendLine("===============================================================================");
				returnValue.AppendLine(File.ReadAllText(exceptionLogPath));
				returnValue.AppendLine("");
				returnValue.AppendLine("");
				returnValue.AppendLine("");
			}

			return returnValue.ToString();
		}

		private StringBuilder GetAllegianceFileVersions(string path)
		{
			StringBuilder returnValue = new StringBuilder();

			foreach (string directory in Directory.GetDirectories(path))
				returnValue.Append(GetAllegianceFileVersions(directory));

			bool foundFiles = false;
			StringBuilder directoryContents = new StringBuilder();

			directoryContents.AppendLine(path);
			directoryContents.AppendLine("==================================================================================");

			foreach (string file in Directory.GetFiles(path, "*.exe"))
			{
				var fileVersionInfo = FileVersionInfo.GetVersionInfo(file);
				var fileTime = File.GetLastWriteTime(file);
				directoryContents.AppendLine(file + "," + fileVersionInfo.FileVersion + "," + fileTime);
				foundFiles = true;
			}

			foreach (string file in Directory.GetFiles(path, "*.dll"))
			{
				var fileVersionInfo = FileVersionInfo.GetVersionInfo(file);
				var fileTime = File.GetLastWriteTime(file);
				directoryContents.AppendLine(file + "," + fileVersionInfo.FileVersion + "," + fileTime);
				foundFiles = true;
			}

			directoryContents.AppendLine("");
			directoryContents.AppendLine("");
			directoryContents.AppendLine("");

			if (foundFiles == true)
				returnValue.Append(directoryContents);

			return returnValue;
		}



		private string GetAllegianceRegistrySettings()
		{
			StringBuilder returnValue = new StringBuilder();

			string outputPath64 = Path.GetTempFileName();
			string outputPath32 = Path.GetTempFileName();
			string sixtyFourBitRegPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Microsoft Games\Allegiance";
			string thirtyTwoBitRegPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Microsoft Games\Allegiance";

			try
			{
				Process process64 = Process.Start("regedit", "/e \"" + outputPath64 + "\" \"" + sixtyFourBitRegPath + "\"");
				Process process32 = Process.Start("regedit", "/e \"" + outputPath32 + "\" \"" + thirtyTwoBitRegPath + "\"");

				process64.WaitForExit();
				process32.WaitForExit();

				returnValue.AppendLine(sixtyFourBitRegPath);
				returnValue.AppendLine("============================================================================================");
				returnValue.AppendLine(File.ReadAllText(outputPath64));
				returnValue.AppendLine("============================================================================================");
				returnValue.AppendLine("");
				returnValue.AppendLine("");
				returnValue.AppendLine("");

				returnValue.AppendLine(thirtyTwoBitRegPath);
				returnValue.AppendLine("============================================================================================");
				returnValue.AppendLine(File.ReadAllText(outputPath32));
				returnValue.AppendLine("============================================================================================");
				returnValue.AppendLine("");
				returnValue.AppendLine("");
				returnValue.AppendLine("");
			}
			finally
			{
				File.Delete(outputPath64);
				File.Delete(outputPath32);
			}

			return returnValue.ToString();
		}

		private void _copyToClipboardButton_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(_diagnosticsOutputTextbox.Text);
			MessageBox.Show("Diagnostics copied to clipboard.");
		}

		private void _closeButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
