using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Allegiance.CommunitySecuritySystem.Management.Properties;
using Allegiance.CommunitySecuritySystem.Management.Business;
using System.Security;
using System.Security.Cryptography;
using Allegiance.CommunitySecuritySystem.DataAccess;
using System.Transactions;

namespace Allegiance.CommunitySecuritySystem.Management.Content.AutoUpdate
{
	public partial class EditPackage : UI.Page
	{
		protected bool PublicationDeployed
		{
			get
			{
				return Request.Params["publicationDeployed"] == "1";
			}
		}

		protected bool PublicationDeployFailed
		{
			get
			{
				return Request.Params["publicationDeployFailed"] == "1";
			}
		}


		protected override void OnInit(EventArgs e)
		{
			ucPackageContents.Target = Target;

			base.OnInit(e);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.IsPostBack == false)
				BindData();

			lblUploadStatus.Text = "";

			if (String.IsNullOrEmpty(Request.Params["packageUpdated"]) == false)
				CheckPackageForUpdatedLauncher();
		}

		
		private void BindData()
		{
			txtPackageName.Text = Target;

			if (String.IsNullOrEmpty(Target) == false)
			{
				FileInfo packageInfo = AutoUpdateManager.GetPackageInfo(Target);
				txtCreateDate.Text = Format.DateTime(packageInfo.CreationTime);
				txtLastModifiedDate.Text = Format.DateTime(packageInfo.LastWriteTime);
				tblPackageContents.Visible = true;
			}
			else
			{
				tblPackageContents.Visible = false;
			}

			var packageGuid = AutoUpdateManager.GetPackageGuid(this.Target);

			List<Lobby> availablePublications = new List<Lobby>();

			foreach (Lobby lobby in AutoUpdateManager.GetPublications())
			{
				foreach (var includedPackageGuid in AutoUpdateManager.GetIncludedPackageGuids(lobby.Id))
				{
					if (includedPackageGuid == packageGuid && availablePublications.Contains(lobby) == false)
					{
						availablePublications.Add(lobby);
					}
				}
			}
			
			// Bind the availablePublications to the repeater.
			rptQuickDeploy.DataSource = availablePublications;
			rptQuickDeploy.DataBind();
		}

		protected void btnAddFile_Click(object sender, EventArgs e)
		{
			if (fuFileUpload.HasFile == true)
			{
				if (Path.GetExtension(fuFileUpload.FileName).ToLower() != ".zip")
				{
					lblUploadStatus.Text = "Upload failed: please upload .zip files only! Use the File Manager to upload single files.";
					return;
				}

				
				string tempFile = Path.GetTempFileName();
				fuFileUpload.SaveAs(tempFile);

				ICSharpCode.SharpZipLib.Zip.FastZip fastZip = new ICSharpCode.SharpZipLib.Zip.FastZip();

				string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

				fastZip.ExtractZip(tempFile, tempDirectory, "");

				if (ValidateZipFileContainsOnlyOneLauncher(tempDirectory) == false)
				{
					lblUploadStatus.Text = "Upload failed: upload package contains multiple launcher.exe files.";
					return;
				}

				if (ValidateCombinedPackageAndZipFileContainOnlyOneLauncher(this.Target, tempDirectory) == false)
				{
					lblUploadStatus.Text = "Upload failed: the package contains a launcher.exe that is in a different location in the zip file.";
					return;
				}

				AutoUpdateManager.CreateBackup("AutoBackup - Uploading " + fuFileUpload.FileName + " to " + this.Target);

				AddUploadedFilesToPackage(this.Target, tempDirectory, tempDirectory);

				File.Delete(tempFile);
				Directory.Delete(tempDirectory, true);

				lblUploadStatus.Text = "Upload for " + fuFileUpload.FileName + " complete.";

				BindData();

				ucPackageContents.BindData();

				CheckPackageForUpdatedLauncher();
			}
		}

		private bool ValidateCombinedPackageAndZipFileContainOnlyOneLauncher(string targetPackage, string tempDirectory)
		{
			List<string> launcherInstancesInZipFile = GetLauncherInstancesInDirectory(tempDirectory);
			List<string> launcherIntancesInPackageFile = new List<string>();
			List<UpdateItem> updateItems = AutoUpdateManager.GetFilesInUpdatePackage(targetPackage);

			//string packagePath = Path.Combine(AutoUpdateManager.PackageRoot, targetPackage);

			int matchedItemCounter = 0;
			foreach (UpdateItem updateItem in updateItems)
			{
				if (updateItem.Name.Equals("launcher.exe", StringComparison.InvariantCultureIgnoreCase) == true)
				{
					string updateItemRelativeFilename = Path.Combine(updateItem.RelativeDirectory, updateItem.Name);
					launcherIntancesInPackageFile.Add(updateItemRelativeFilename);

					foreach (string zipFileLauncherInstance in launcherInstancesInZipFile)
					{
						string relativePath = zipFileLauncherInstance.Substring(tempDirectory.Length + 1);

						if (relativePath.Equals(updateItemRelativeFilename, StringComparison.InvariantCultureIgnoreCase) == true)
						{
							matchedItemCounter++;
							break;
						}
					}
				}
			}

			if (launcherInstancesInZipFile.Count + launcherIntancesInPackageFile.Count - matchedItemCounter > 1)
				return false;

			return true;
		}

		private bool ValidateZipFileContainsOnlyOneLauncher(string tempDirectory)
		{
			List<string> launcherInstances = GetLauncherInstancesInDirectory(tempDirectory);
			if (launcherInstances.Count > 1)
				return false;

			return true;
		}

		private List<string> GetLauncherInstancesInDirectory(string directory)
		{
			List<string> returnValue = new List<string>();

			foreach (string subdirectory in Directory.GetDirectories(directory))
				returnValue.AddRange(GetLauncherInstancesInDirectory(subdirectory));

			foreach (string filename in Directory.GetFiles(directory))
			{
				if (Path.GetFileName(filename).Equals("launcher.exe", StringComparison.InvariantCultureIgnoreCase) == true)
					returnValue.Add(filename);
			}

			return returnValue;
		}

		private void AddUploadedFilesToPackage(string packageName, string rootDirectory, string currentDirectory)
		{
			foreach (string directory in Directory.GetDirectories(currentDirectory))
				AddUploadedFilesToPackage(packageName, rootDirectory, directory);

			string relativeDirectory = currentDirectory.Substring(rootDirectory.Length).TrimStart(new char[] { '\\' });

			foreach (string filename in Directory.GetFiles(currentDirectory))
				AutoUpdateManager.AddFileToPackage(packageName, relativeDirectory, filename);
		}

		protected void btnSavePackageDetails_Click(object sender, EventArgs e)
		{
			if (txtPackageName.Text.Equals(Target, StringComparison.InvariantCultureIgnoreCase) == false)
			{
				bool packageNameUpdated;

				if (String.IsNullOrEmpty(Target) == true)
					packageNameUpdated = AutoUpdateManager.CreatePackage(txtPackageName.Text);
				else
					packageNameUpdated = AutoUpdateManager.RenamePackage(Target, txtPackageName.Text);

				if (packageNameUpdated == true)
					Response.Redirect("EditPackage.aspx?target=" + Server.UrlEncode(txtPackageName.Text));
				else
					cvPackageExists.IsValid = false;
			}
		}

		protected void cvPackageExists_ServerValidate(object source, ServerValidateEventArgs args)
		{
			if (txtPackageName.Text.Equals(Target, StringComparison.InvariantCultureIgnoreCase) == false)
			{
				args.IsValid = (AutoUpdateManager.DoesPackageExist(txtPackageName.Text) == false);
			}
			else
			{
				args.IsValid = true;
			}
		}

		protected void btnFileManager_Click(object sender, EventArgs e)
		{
			Response.Redirect("FileManager.aspx?target=" + Server.UrlEncode(Target), true);
		}

		private void CheckPackageForUpdatedLauncher()
		{
			List<UpdateItem> filesInPackage = AutoUpdateManager.GetFilesInUpdatePackage(txtPackageName.Text);

			UpdateItem launcherInstance = filesInPackage.FirstOrDefault(p => p.Name.ToLower() == "launcher.exe");
			if (launcherInstance != null)
			{
				// Create new launcher hash.
				using (SHA256Managed sha = new SHA256Managed())
				using (FileStream fs = File.Open(launcherInstance.FileInfo.FullName, FileMode.Open, FileAccess.Read))
				{
					byte[] hash;

					if (Settings.Default.UseDebugBlackBox == true)
					{
						hash = new byte[] {	0x37, 0x61, 0xA7, 0x4B, 0x7E, 0x31, 0xC8, 0x5E, 0xD1, 0xC0, 0xF7, 0xB3, 
									0x95, 0xD4, 0x7E, 0x64, 0xB9, 0xC3, 0x22, 0xED, 0x2F, 0x6F, 0xF8, 0xEB, 
									0x4D, 0xA1, 0x00, 0x9A, 0x35, 0xE5, 0x10, 0x19 };
					}
					else
					{
						hash = sha.ComputeHash(fs);
					}

					File.WriteAllBytes(Settings.Default.KnownHashLocation, hash);
				}

				// Invalidate all existing blackboxes to force regeneration of key tokens.
				using (CSSDataContext db = new CSSDataContext())
				{
					// TODO: Is there a LINQ native way to do this? 
					db.ExecuteCommand("UPDATE ActiveKey SET IsValid = 0");
					db.SubmitChanges();
				}
			}
		}

	}
}
