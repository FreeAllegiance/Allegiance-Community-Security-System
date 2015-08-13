using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Allegiance.CommunitySecuritySystem.Management.Business;
using System.IO;
using Allegiance.CommunitySecuritySystem.Management.Properties;

namespace Allegiance.CommunitySecuritySystem.Management.Content.AutoUpdate
{
	public partial class DownloadItem : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			string backupName = Request.Params["backup"];
			string itemType = Request.Params["type"];
			string container = Request.Params["container"];
			string relativeDirectory = Request.Params["rel"];
			string filename = Request.Params["file"];

			string rootDirectory = String.Empty;

			string filepath = AutoUpdateManager.GetFilePath(itemType, container, relativeDirectory, filename);

			UpdateItem itemToDownload = null;

			if (String.IsNullOrEmpty(backupName) == false) // Download the file from a backup.
			{
				if (AutoUpdateManager.IsFilenameOrDirectorySafe(backupName) == false)
					throw new Exception("backup name is invalid.");

				BackupItems backupItems = AutoUpdateManager.GetFilesInBackup(backupName);

				if (itemType == "Packages")
					itemToDownload = backupItems.PackageFiles.FirstOrDefault(p => p.Name == filename && p.PackageName == container && p.RelativeDirectory == relativeDirectory);
				else
					throw new Exception("Unsupported itemType: " + itemType);
			}
			else // Download the file from a package.
			{
				if (itemType == "Packages")
				{
					List<UpdateItem> itemsInPackage = AutoUpdateManager.GetFilesInUpdatePackage(container);
					itemToDownload = itemsInPackage.FirstOrDefault(p => p.Name == filename && p.PackageName == container && p.RelativeDirectory == relativeDirectory);
				}
				else
					throw new Exception("Unsupported itemType: " + itemType);
			}

			if (itemToDownload == null)
			{
				Response.StatusCode = 404;
				Response.End();
			}

			string mimeType = Utility.GetMimeType(Path.GetExtension(itemToDownload.Name));

			byte[] outputBytes = File.ReadAllBytes(itemToDownload.FileInfo.FullName);

			Response.ContentType = mimeType;
			Response.AddHeader("Content-Disposition", "attachment; filename=" + itemToDownload.Name);
			Response.AddHeader("ContentLength", outputBytes.Length.ToString());

			Response.BinaryWrite(outputBytes);
		}

		protected override void Render(HtmlTextWriter writer)
		{
			// Suppess all output from the page.
		}
	}
}
