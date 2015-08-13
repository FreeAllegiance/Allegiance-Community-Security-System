using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Allegiance.CommunitySecuritySystem.Management.Business;
using System.IO;
using Allegiance.CommunitySecuritySystem.Management.Properties;
using System.Text;
using System.Text.RegularExpressions;

namespace Allegiance.CommunitySecuritySystem.Management.Content.AutoUpdate
{
	public partial class BackupDetails : UI.Page
	{
		private bool? _markAll = null;
		private bool MarkAll
		{
			get
			{
				if(_markAll == null)
					_markAll = String.IsNullOrEmpty(HttpContext.Current.Request.Params["markAll"]) == false;

				return _markAll.Value;
			}
		}

		protected bool FilesWereRestored = false;
		protected string BackupCreationDate;
		protected StringBuilder AllCheckboxUniqueIDsToSelect = new StringBuilder();

		private bool _refreshRequired = false;
		private bool _createBackupBeforeRestore = true;

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		protected override void OnLoadComplete(EventArgs e)
		{
			if (this.IsPostBack == false || _refreshRequired == true)
				BindData();

			base.OnLoadComplete(e);
		}

		private void BindData()
		{
			
			StringBuilder allCheckboxesToSelect = new StringBuilder();
			

			BackupItems backupItems = AutoUpdateManager.GetFilesInBackup(this.Target);
			BackupCreationDate = Format.DateTime(AutoUpdateManager.GetBackupInfo(this.Target).CreationTime);

			List<Data.RestorableFile> restorableFiles = new List<Allegiance.CommunitySecuritySystem.Management.Content.AutoUpdate.Data.RestorableFile>();
			foreach (UpdateItem packageFile in backupItems.PackageFiles)
			{
				string includedImage;
				string protectedImage;

				if (packageFile.IsIncluded == false)
				{
					includedImage = Page.ResolveUrl("~/Images/dg_excluded.png");
				}
				else
				{
					includedImage = Page.ResolveUrl("~/Images/dg_included.png");
				}

				if (packageFile.IsProtected == true)
				{
					protectedImage = Page.ResolveUrl("~/Images/dg_protected.png");
				}
				else
				{
					protectedImage = Page.ResolveUrl("~/Images/dg_unprotected.png");
				}

				restorableFiles.Add(new Allegiance.CommunitySecuritySystem.Management.Content.AutoUpdate.Data.RestorableFile()
				{
					Container = packageFile.PackageName,
					DateCreated = Format.DateTime(packageFile.FileInfo.CreationTime),
					LastModified = Format.DateTime(packageFile.FileInfo.LastWriteTime),
					Name = packageFile.Name,
					Type = "Packages",
					IncludedImage = includedImage,
					ProtectedImage = protectedImage,
					RelativeDirectory = packageFile.RelativeDirectory //.Substring(packageFile.PackageName.Length).TrimStart(new char [] {'\\'})
				});
			}

			gvBackupFiles.DataSource = restorableFiles;
			gvBackupFiles.DataBind();
		}

		protected void chkRestore_OnCheckedChanged(object sender, EventArgs e)
		{
			CheckBox checkbox = (CheckBox)sender;

			if (checkbox.Checked == true)
			{
				GridViewRow gridViewRow = (GridViewRow)checkbox.Parent.Parent;
				string fileName = gridViewRow.Cells[5].Text;
				string relativeDirectory = gridViewRow.Cells[3].Text;
				string type = gridViewRow.Cells[1].Text;
				string container = gridViewRow.Cells[2].Text;
				string backupName = this.Target;

				if (relativeDirectory.Equals("&nbsp;", StringComparison.InvariantCultureIgnoreCase) == true)
					relativeDirectory = String.Empty;


				if (_createBackupBeforeRestore == true)
				{
					backupName = GetBackupNameFromPreviousAutoBackup(backupName);
					
					AutoUpdateManager.CreateBackup("Auto Backup - Restoring from " + backupName);
					_createBackupBeforeRestore = false;
				}

				AutoUpdateManager.RestoreFile(backupName, type, container, relativeDirectory, fileName);
			}

			_refreshRequired = true;
			FilesWereRestored = true;
		}

		private string GetBackupNameFromPreviousAutoBackup(string backupName)
		{
			Regex backupNameFinder = new Regex(@"Auto Backup - Restoring from (?<backup>.*?)(($)|(\s#\d+$))");
			Match backupNameMatch = backupNameFinder.Match(backupName);
			if(backupNameMatch.Success == true)
				return backupNameMatch.Groups["backup"].Value;

			return backupName;
		}

		protected void chkRestore_OnPreRender(object sender, EventArgs e)
		{
			CheckBox checkbox = (CheckBox) sender;

			if (this.MarkAll == true)
			{
				if (AllCheckboxUniqueIDsToSelect.Length == 0)
					AllCheckboxUniqueIDsToSelect.AppendFormat("\"{0}\"", checkbox.ClientID);
				else
					AllCheckboxUniqueIDsToSelect.AppendFormat(",\"{0}\"", checkbox.ClientID);
			}
		}

		
	}
}
