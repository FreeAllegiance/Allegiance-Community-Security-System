using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using Microsoft.Win32;
using System.Text;
using System.IO;
using Allegiance.CommunitySecuritySystem.Client.Integration;
using System.Diagnostics;
using System.Windows.Forms;
using Allegiance.CommunitySecuritySystem.Client.Utility;
using System.Net;
using System.Runtime.InteropServices;


namespace Allegiance.CommunitySecuritySystem.Client.Install
{
	[RunInstaller(true)]
	public partial class BetaInstall : Installer
	{
		private const float Windows8Version = 6.2F;

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

		public const int SWP_ASYNCWINDOWPOS = 0x4000;
		public const int SWP_DEFERERASE = 0x2000;
		public const int SWP_DRAWFRAME = 0x0020;
		public const int SWP_FRAMECHANGED = 0x0020;
		public const int SWP_HIDEWINDOW = 0x0080;
		public const int SWP_NOACTIVATE = 0x0010;
		public const int SWP_NOCOPYBITS = 0x0100;
		public const int SWP_NOMOVE = 0x0002;
		public const int SWP_NOOWNERZORDER = 0x0200;
		public const int SWP_NOREDRAW = 0x0008;
		public const int SWP_NOREPOSITION = 0x0200;
		public const int SWP_NOSENDCHANGING = 0x0400;
		public const int SWP_NOSIZE = 0x0001;
		public const int SWP_NOZORDER = 0x0004;
		public const int SWP_SHOWWINDOW = 0x0040;

		public const int HWND_TOP = 0;
		public const int HWND_BOTTOM = 1;
		public const int HWND_TOPMOST = -1;
		public const int HWND_NOTOPMOST = -2;



		private const string BuiltinUsersSID = "S-1-5-32-545";

		private float WindowsVersion
		{
			get
			{
				return (float) System.Environment.OSVersion.Version.Major + ((float) System.Environment.OSVersion.Version.Minor / 10F);
			}
		}

		public BetaInstall()
		{
			InitializeComponent();
		}

		

		protected override void OnBeforeInstall(IDictionary savedState)
		{
			if (this.Context.Parameters.ContainsKey("Beta") == true)
			{
				bool betaMode;
				if (Boolean.TryParse(this.Context.Parameters["Beta"], out betaMode) == true || betaMode == true)
					DoBetaBeforeInstall();
			}
		
			base.OnBeforeInstall(savedState);
		}

		private string GetAllegianceRoot()
		{
			//string allegianceRoot;
			//if (Interop.Is64Bit() == false)
			//    allegianceRoot = @"Software\Microsoft\Microsoft Games\Allegiance\";
			//else
			//    allegianceRoot = @"Software\Wow6432Node\Microsoft\Microsoft Games\Allegiance\";

			//return allegianceRoot;

			return @"Software\Microsoft\Microsoft Games\Allegiance\";
		}

		private void DoBetaBeforeInstall()
		{
			RegistryKey allegianceRoot = Registry.LocalMachine.OpenSubKey(GetAllegianceRoot());
			
			if(allegianceRoot == null)
				throw new System.Configuration.Install.InstallException("Couldn't open Allegiance registry root. Please install Allegiance from the standard installer on http://www.freeallegiance.org before installing the ACSS Beta.");

			RegistryKey productionBranch = allegianceRoot.OpenSubKey("1.0");

			if (productionBranch == null)
				throw new System.Configuration.Install.InstallException("Couldn't open Allegiance 1.0 sub key. Please install Allegiance from the standard installer on http://www.freeallegiance.org before installing the ACSS Beta.");


			string artPath = (string)productionBranch.GetValue("ArtPath", null);

			if (artPath == null)
				throw new System.Configuration.Install.InstallException("Couldn't detect the Allegiance ArtPath. Please install Allegiance from the standard installer on http://www.freeallegiance.org before installing the ACSS Beta.");

			var vc2010Key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\VisualStudio\10.0\VC\VCRedist\x86");

			if (vc2010Key == null || vc2010Key.GetValue("Installed") == null || (int)vc2010Key.GetValue("Installed") == 0)
				InstallVC2010();
		}

		private void InstallVC2010()
		{
			VC2010Install dialog = new VC2010Install();
			dialog.TopMost = true;
			
			if(dialog.ShowDialog() == DialogResult.Cancel)
				throw new System.Configuration.Install.InstallException("VC++ 2010 x86 Runtime were not installed, aborting installation.");


			//Debugger.Break();
			//SetWindowPos(Process.GetCurrentProcess().MainWindowHandle, IntPtr.Zero, 0, 0, 0, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_NOMOVE | SWP_HIDEWINDOW);

			//if (MessageBox.Show("Visual C++ 2010 x86 Runtime is not installed or needs to be updated. Please select OK to download and install the VC++ 2010 Runtimes from: http://download.microsoft.com/download/5/B/C/5BC5DBB3-652D-4DCE-B14A-475AB85EEF6E/vcredist_x86.exe.", "VC++ 2010 Runtime Required", MessageBoxButtons.OKCancel) != DialogResult.OK)
			//{
			//    if(MessageBox.Show("Would you like to continue without installing the VC++ 2010 x86 Runtime (You can install it later if you wish)?", "Skip VC++ 2010 Runtimes?", MessageBoxButtons.YesNo) == DialogResult.No)
			//        throw new System.Configuration.Install.InstallException("VC++ 2010 x86 Runtime were not installed, aborting installation.");
			//}

			//WebClient webClient = new WebClient();
			//string tempFile = Path.Combine(Path.GetTempPath(), "vcredist_x86.exe");

			//if (File.Exists(tempFile) == true)
			//    File.Delete(tempFile);

			//try
			//{
			//    webClient.DownloadFile("http://download.microsoft.com/download/5/B/C/5BC5DBB3-652D-4DCE-B14A-475AB85EEF6E/vcredist_x86.exe", tempFile);

			//    if (File.Exists(tempFile) == true)
			//    {
			//        var process = Process.Start(tempFile);

			//        Log.Write("process.MainWindowHandle: " + process.MainWindowHandle);


			//        SetWindowPos(process.MainWindowHandle, IntPtr.Zero, 0, 0, 0, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_NOMOVE | SWP_HIDEWINDOW);
			//        SetWindowPos(process.MainWindowHandle, IntPtr.Zero, 0, 0, 0, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_NOMOVE | SWP_SHOWWINDOW);

			//        //this.SendToBack();
			//        //this.WindowState = FormWindowState.Minimized;
			//        //this.WindowState = FormWindowState.Normal;
			//        //this.BringToFront();


			//        process.WaitForExit();
			//    }
			//}
			//catch
			//{
			//    if (File.Exists(tempFile) == true)
			//        File.Delete(tempFile);
			//}
			//finally
			//{
			//    SetWindowPos(Process.GetCurrentProcess().MainWindowHandle, IntPtr.Zero, 0, 0, 0, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_NOMOVE | SWP_SHOWWINDOW);
			//}

		}

		//public override void Install(IDictionary stateSaver)
		//{
		//    string launcherPath = this.Context.Parameters["targetpath"];
		//    string launcherExe = Path.Combine(launcherPath, "Launcher.exe");

		//    if (File.Exists(launcherExe) == true)
		//        File.Delete(launcherExe);

		//    base.Install(stateSaver);
		//}

		protected override void OnAfterInstall(IDictionary savedState)
		{
			int stepCount = 0;

			try
			{

				//StringBuilder sb = new StringBuilder();
				//foreach (string key in this.Context.Parameters.Keys)
				//{
				//    sb.AppendLine(String.Format("{0} = {1}", key, this.Context.Parameters[key]));
				//}
				//string exePath = this.Context.Parameters["targetpath"];
				//sb.AppendLine("wtf: " + exePath);
				//File.WriteAllText("c:\\out.txt", sb.ToString());

				// Clear any old DS databases.
				foreach (string filename in Directory.GetFiles(this.Context.Parameters["targetpath"], "*.ds"))
					File.Delete(filename);

				stepCount++;

				// Create registry keys if they don't exist.
				RegistryKey rootKey = Registry.LocalMachine.OpenSubKey(GetAllegianceRoot(), true);
				RegistryKey productionKey = rootKey.OpenSubKey("1.0");
				string artPath = (String)productionKey.GetValue("ArtPath");
				string productionExePath = (String)productionKey.GetValue("EXE Path");

				stepCount++;

				try
				{
					if (RegistryAccess.DoesUserHaveAccess(rootKey, BuiltinUsersSID, System.Security.AccessControl.RegistryRights.FullControl) == false)
						RegistryAccess.SetUserAccessBySID(rootKey, BuiltinUsersSID, System.Security.AccessControl.RegistryRights.FullControl);
				}
				catch (Exception ex)
				{
					this.Context.LogMessage(ex.ToString());

					// Ignore security exceptions for Windows8 and up.
					if (WindowsVersion < Windows8Version)
						throw;


					//if (WindowsVersion >= 6.2)
					//{
					//    if (MessageBox.Show("There was an error granting permissions to the BUILTIN\\User account to: " + rootKey + "\r\n\r\nThis is specific to Windows 8, and can be ignored. Continue Installation?", "Permissions Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
					//        throw;
					//}
					//else
					//    throw;
				}

				stepCount++;

				RegistryKey developmentKey = rootKey.OpenSubKey("1.2", true);
				if (developmentKey == null)
					developmentKey = rootKey.CreateSubKey("1.2", RegistryKeyPermissionCheck.ReadWriteSubTree);

				stepCount++;

				//if (developmentKey.GetValue("Allow3DAcceleration", null) == null)
				developmentKey.SetValue("Allow3DAcceleration", 1, RegistryValueKind.DWord);

				stepCount++;

				//if (developmentKey.GetValue("AllowSecondary", null) == null)
				developmentKey.SetValue("AllowSecondary", 1, RegistryValueKind.DWord);

				stepCount++;

				//if (developmentKey.GetValue("ArtPath", null) == null)
				developmentKey.SetValue("ArtPath", Path.Combine(this.Context.Parameters["targetpath"], Integration.LobbyType.Production.ToString() + "\\Artwork"), RegistryValueKind.String);

				developmentKey.SetValue("ProductionArtPath", Path.Combine(this.Context.Parameters["targetpath"], Integration.LobbyType.Production.ToString() + "\\Artwork"), RegistryValueKind.String);

				developmentKey.SetValue("BetaArtPath", Path.Combine(this.Context.Parameters["targetpath"], Integration.LobbyType.Beta.ToString() + "\\Artwork"), RegistryValueKind.String);

				stepCount++;

				//if (developmentKey.GetValue("Bandwidth", null) == null)
				developmentKey.SetValue("Bandwidth", 2, RegistryValueKind.DWord);

				stepCount++;

				// Don't delete this, might re-add post css beta.
				// If there is already a CfgFile entry, then store it for uninstallation only if there
				// is not already a PreviousCfgFile entry.
				//string cfgFile = (String)developmentKey.GetValue("CfgFile", null);
				//if (cfgFile != null)
				//{
				//    if (developmentKey.GetValue("PreviousCfgFile", null) == null)
				//        developmentKey.SetValue("PreviousCfgFile", cfgFile, RegistryValueKind.String);
				//}

				developmentKey.SetValue("CfgFile", "http://acss.alleg.net/allegiance.txt", RegistryValueKind.String);

				developmentKey.SetValue("ProductionCfgFile", "http://acss.alleg.net/allegiance.txt", RegistryValueKind.String);

				developmentKey.SetValue("BetaCfgFile", "http://acss.alleg.net/allegiance-beta.txt", RegistryValueKind.String);

				stepCount++;

				//if (developmentKey.GetValue("CharacterName", null) == null)
				developmentKey.SetValue("CharacterName", String.Empty, RegistryValueKind.String);

				stepCount++;

				//if (developmentKey.GetValue("CombatFullscreenXSize", null) == null)
				developmentKey.SetValue("CombatFullscreenXSize", 800, RegistryValueKind.DWord);

				stepCount++;

				//if (developmentKey.GetValue("CombatFullscreenYSize", null) == null)
				developmentKey.SetValue("CombatFullscreenYSize", 600, RegistryValueKind.DWord);

				stepCount++;

				//if (developmentKey.GetValue("CombatXSize", null) == null)
				developmentKey.SetValue("CombatXSize", 800, RegistryValueKind.DWord);

				stepCount++;

				//if (developmentKey.GetValue("CombatYSize", null) == null)
				developmentKey.SetValue("CombatYSize", 600, RegistryValueKind.DWord);

				stepCount++;

				//if (developmentKey.GetValue("DeadZone", null) == null)
				developmentKey.SetValue("DeadZone", 30, RegistryValueKind.DWord);

				stepCount++;

				// Don't delete this, might re-add post beta.
				//string exePath = (String)developmentKey.GetValue("EXE Path", null);
				//if (exePath != null)
				//{
				//    if (developmentKey.GetValue("PreviousEXEPath", null) == null)
				//        developmentKey.SetValue("PreviousEXEPath", exePath, RegistryValueKind.String);
				//}

				developmentKey.SetValue("Lobby Path", this.Context.Parameters["targetpath"].TrimEnd('\\'), RegistryValueKind.String);

				stepCount++;

				developmentKey.SetValue("EXE Path", productionExePath, RegistryValueKind.String);

				stepCount++;

				//if (developmentKey.GetValue("Gamma", null) == null)
				developmentKey.SetValue("Gamma", 1.13, RegistryValueKind.String);

				stepCount++;

				//if (developmentKey.GetValue("HasTrained", null) == null)
				developmentKey.SetValue("HasTrained", 1, RegistryValueKind.DWord);

				stepCount++;

				//if (developmentKey.GetValue("LogToFile", null) == null)
				developmentKey.SetValue("LogToFile", 1, RegistryValueKind.String);

				stepCount++;

				//if (developmentKey.GetValue("Music", null) == null)
				developmentKey.SetValue("Music", 1, RegistryValueKind.DWord);

				stepCount++;

				//if (developmentKey.GetValue("OutputDebugString", null) == null)
				developmentKey.SetValue("OutputDebugString", 1, RegistryValueKind.DWord);

				stepCount++;

				developmentKey.SetValue("ClientService", "https://acss.alleg.net/CSSServer/ClientService.svc", RegistryValueKind.String);

				stepCount++;

				developmentKey.SetValue("ManagementWebRoot", "http://acss.alleg.net", RegistryValueKind.String);

				stepCount++;

				try
				{
					if (FileSystemAccess.DoesUserHaveAccessToDirectory(artPath, BuiltinUsersSID, System.Security.AccessControl.FileSystemRights.FullControl) == false)
						FileSystemAccess.SetDirectoryAccessBySID(artPath, BuiltinUsersSID, System.Security.AccessControl.FileSystemRights.FullControl);
				}
				catch (Exception ex)
				{
					this.Context.LogMessage(ex.ToString());

					// Ignore security exceptions for Windows8 and up.
					if (WindowsVersion < Windows8Version)
						throw;

					//if (WindowsVersion >= 6.2)
					//{
					//    if (MessageBox.Show("There was an error granting permissions to the BUILTIN\\User account to: " + artPath + "\r\n\r\nThis is specific to Windows 8, and can be ignored. Continue Installation?", "Permissions Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
					//        throw;
					//}
					//else
					//    throw;
				}

				stepCount++;

				try
				{
					if (FileSystemAccess.DoesUserHaveAccessToDirectory(productionExePath, BuiltinUsersSID, System.Security.AccessControl.FileSystemRights.FullControl) == false)
						FileSystemAccess.SetDirectoryAccessBySID(productionExePath, BuiltinUsersSID, System.Security.AccessControl.FileSystemRights.FullControl);
				}
				catch (Exception ex)
				{
					this.Context.LogMessage(ex.ToString());

					// Ignore security exceptions for Windows8 and up.
					if (WindowsVersion < Windows8Version)
						throw;

					//if (WindowsVersion >= 6.2)
					//{
					//    if (MessageBox.Show("There was an error granting permissions to the BUILTIN\\User account to: " + productionExePath + "\r\n\r\nThis is specific to Windows 8, and can be ignored. Continue Installation?", "Permissions Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
					//        throw;
					//}
					//else
					//    throw;
				}

				stepCount++;

				try
				{

					if (FileSystemAccess.DoesUserHaveAccessToDirectory(this.Context.Parameters["targetpath"], BuiltinUsersSID, System.Security.AccessControl.FileSystemRights.FullControl) == false)
						FileSystemAccess.SetDirectoryAccessBySID(this.Context.Parameters["targetpath"], BuiltinUsersSID, System.Security.AccessControl.FileSystemRights.FullControl);
				}
				catch (Exception ex)
				{
					this.Context.LogMessage(ex.ToString());

					// Ignore security exceptions for Windows8 and up.
					if (WindowsVersion < Windows8Version)
						throw;

					//if (WindowsVersion >= 6.2)
					//{
					//    if (MessageBox.Show("There was an error granting permissions to the BUILTIN\\User account to: " + this.Context.Parameters["targetpath"] + "\r\n\r\nThis is specific to Windows 8, and can be ignored. Continue Installation?", "Permissions Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
					//        throw;
					//}
					//else
					//    throw;
				}

				stepCount++;

				CopyProgress copyProgressBeta = new CopyProgress();
				copyProgressBeta.TopMost = true;
				//copyProgress.MinimizeBox = false;
				copyProgressBeta.SourceDirectory = artPath;
				copyProgressBeta.TargetDirectory = Path.Combine(Path.Combine(this.Context.Parameters["targetpath"], LobbyType.Beta.ToString()), "Artwork");
				copyProgressBeta.LobbyType = LobbyType.Beta;

				if (copyProgressBeta.ShowDialog() != System.Windows.Forms.DialogResult.OK)
					throw new InstallException("Beta Artwork file copy was canceled.");

				stepCount++;

				CopyProgress copyProgressProduction = new CopyProgress();
				copyProgressProduction.TopMost = true;
				//copyProgress.MinimizeBox = false;
				copyProgressProduction.SourceDirectory = artPath;
				copyProgressProduction.TargetDirectory = Path.Combine(Path.Combine(this.Context.Parameters["targetpath"], LobbyType.Production.ToString()), "Artwork");
				copyProgressProduction.LobbyType = LobbyType.Production;

				if (copyProgressProduction.ShowDialog() != System.Windows.Forms.DialogResult.OK)
					throw new InstallException("Production Artwork file copy was canceled.");


				base.OnAfterInstall(savedState);
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					"Windows Version: " + WindowsVersion + 
					"\r\nStep Count: " + stepCount + 
					"\r\n\r\n" + ex.ToString(), "BetaInstall::OnAfterInstall() - Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		protected override void OnAfterUninstall(IDictionary savedState)
		{

			RegistryKey rootKey = Registry.LocalMachine.OpenSubKey(GetAllegianceRoot(), true);

			if (rootKey != null)
			{
				RegistryKey developmentKey = rootKey.OpenSubKey("1.2", true);

				// Remove all the values.
				foreach (string valueName in developmentKey.GetValueNames())
					developmentKey.DeleteValue(valueName);

				if (new List<String>(developmentKey.GetSubKeyNames()).Contains("3DSettings") == true)
					developmentKey.DeleteSubKeyTree("3DSettings");

				if (new List<String>(developmentKey.GetSubKeyNames()).Contains("SquadMemberships") == true)
					developmentKey.DeleteSubKeyTree("SquadMemberships");

				// If there are no sub keys (client install only), then remove the whole 1.2 key
				if (developmentKey.GetSubKeyNames().Length == 0)
					rootKey.DeleteSubKeyTree("1.2");

				// Don't need these steps as we are using a separate subkey for CSS.
				// Don't delete this tho, once CSS beta is over, the key might get moved back to 1.1...

				// 
			//    if (developmentKey != null)
			//    {
			//        string previousCfgFile = (string) developmentKey.GetValue("PreviousCfgFile", null);

			//        if (previousCfgFile != null)
			//        {
			//            developmentKey.SetValue("CfgFile", previousCfgFile, RegistryValueKind.String);
			//            developmentKey.DeleteValue("PreviousCfgFile");
			//        }

			//        string previousEXEPath = (string)developmentKey.GetValue("PreviousEXEPath", null);

			//        if (previousEXEPath != null)
			//        {
			//            developmentKey.SetValue("EXE Path", previousEXEPath, RegistryValueKind.String);
			//            developmentKey.DeleteValue("PreviousEXEPath");
			//        }
			//    }
				
			}

			base.OnAfterUninstall(savedState);
		}
	}
}
