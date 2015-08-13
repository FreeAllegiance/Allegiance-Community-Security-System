using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Allegiance.CommunitySecuritySystem.Management.Business;

namespace Allegiance.CommunitySecuritySystem.Management.Content.AutoUpdate
{
	public partial class FileManager : UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Master.PageHeader = "CSS - File Manager";

			if (this.IsPostBack == false)
			{
				List<UpdateItem> updateItems = AutoUpdateManager.GetFilesInUpdatePackage(this.Target);

				string workingDirectory = Server.MapPath("~/TempFiles/" + this.Target);

				if(Directory.Exists(workingDirectory) == true)
					Directory.Delete(workingDirectory, true);

				Directory.CreateDirectory(workingDirectory);

				foreach (UpdateItem updateItem in updateItems)
				{
					string targetDirectory = Path.Combine(workingDirectory, updateItem.RelativeDirectory);
					if (Directory.Exists(targetDirectory) == false)
						Directory.CreateDirectory(targetDirectory);

					string destinationFile = Path.Combine(targetDirectory, updateItem.Name);
					File.Copy(updateItem.FileInfo.FullName, destinationFile, true);
				}

				//FileInfo packageInfo = AutoUpdateManager.GetPackageInfo(this.Target);
				//CopyItemsToTempDirectory(packageInfo.FullName, workingDirectory);

				wcFileManager.RootDirectories.Add(new IZ.WebFileManager.RootDirectory()
				{
					DirectoryPath = "~/TempFiles/" + this.Target,
					Text = this.Target,
					ShowRootIndex = false
				});
			}
		}

		private void CopyItemsToTempDirectory(string sourceDirectory, string targetDirectory)
		{
			foreach (string directory in Directory.GetDirectories(sourceDirectory))
				CopyItemsToTempDirectory(directory, Path.Combine(targetDirectory, Path.GetFileName(directory)));

			if (Directory.Exists(targetDirectory) == false)
				Directory.CreateDirectory(targetDirectory);

			foreach (string filename in Directory.GetFiles(sourceDirectory))
			{
				File.Copy(filename, Path.Combine(targetDirectory, Path.GetFileName(filename)), true);
			}
		}

		protected bool CommitChanges()
		{
			List<UpdateItem> updateItems = AutoUpdateManager.GetFilesInUpdatePackage(this.Target);

			string workingDirectory = Server.MapPath("~/TempFiles/" + this.Target);

			if (FindAllLauncherInstancesForDirectory(workingDirectory) > 1)
			{
				lblErrorMessage.Text = "Multiple launcher.exe instances found. Only one launcher.exe may be specified.";
				return false;
			}

			CommitItems(updateItems, workingDirectory, workingDirectory);

			CleanUpPackageDirectories(updateItems, workingDirectory, workingDirectory);

			return true;
		}

		private int FindAllLauncherInstancesForDirectory(string workingDirectory)
		{
			int launcherCount = 0;

			foreach (string directory in Directory.GetDirectories(workingDirectory))
				launcherCount += FindAllLauncherInstancesForDirectory(directory);

			foreach (string filename in Directory.GetFiles(workingDirectory))
			{
				if (Path.GetFileName(filename).Equals("launcher.exe", StringComparison.InvariantCultureIgnoreCase) == true)
					launcherCount++;
			}

			return launcherCount;
		}

		private void CleanUpPackageDirectories(List<UpdateItem> updateItems, string rootDirectory, string sourceDirectory)
		{
			string relativeDirectory = sourceDirectory.Substring(rootDirectory.Length).TrimStart(new char[] { '\\' });

			foreach (UpdateItem updateItem in updateItems)
			{
				if(Directory.Exists(Path.Combine(rootDirectory, updateItem.RelativeDirectory)) == false)
				{
					AutoUpdateManager.DeleteFolderFromPackage(this.Target, updateItem.RelativeDirectory);
				}
			}
		}

		private void CommitItems(List<UpdateItem> updateItems, string rootDirectory, string sourceDirectory)
		{
			string relativeDirectory = sourceDirectory.Substring(rootDirectory.Length).TrimStart(new char [] { '\\' });

			foreach (string directory in Directory.GetDirectories(sourceDirectory))
				CommitItems(updateItems, rootDirectory, directory);

			foreach(string filename in Directory.GetFiles(sourceDirectory))
			{
				bool foundExistingItem = false;
				foreach (UpdateItem updateItem in updateItems)
				{
					if (updateItem.RelativeDirectory == relativeDirectory && updateItem.Name == Path.GetFileName(filename))
					{
						AutoUpdateManager.DeleteFileFromPackage(this.Target, updateItem.RelativeDirectory, updateItem.Name);
						AutoUpdateManager.AddFileToPackage(this.Target, relativeDirectory, filename);

						if (updateItem.IsProtected == true)
							AutoUpdateManager.SetFileProtectionForPackage(this.Target, relativeDirectory, Path.GetFileName(filename), updateItem.IsProtected);

						if(updateItem.IsIncluded == false)
							AutoUpdateManager.SetFileInclusionForPackage(this.Target, relativeDirectory, Path.GetFileName(filename), updateItem.IsIncluded);

						foundExistingItem = true;
						break;
					}
				}

				if(foundExistingItem == false)
					AutoUpdateManager.AddFileToPackage(this.Target, relativeDirectory, filename);
			}

			// Clean up files that are no longer in the package.
			foreach (UpdateItem updateItem in updateItems)
			{
				if (updateItem.RelativeDirectory == relativeDirectory)
				{
					if (File.Exists(Path.Combine(sourceDirectory, updateItem.Name)) == false)
						AutoUpdateManager.DeleteFileFromPackage(this.Target, updateItem.RelativeDirectory, updateItem.Name);
				}
			}
		}

		protected void wcFileManager_ItemRenamed(object sender, IZ.WebFileManager.RenameEventArgs e)
		{
		
		}

		protected void wcFileManager_ItemRenaming(object sender, IZ.WebFileManager.RenameCancelEventArgs e)
		{
			string relativeDirectory = Path.GetDirectoryName(TrimPackageNameFromVirtualPathAndMakeRelative(this.Target, e.FileManagerItem.FileManagerPath));
			AutoUpdateManager.RenameFileInPackage(this.Target, relativeDirectory, Path.GetFileName(e.FileManagerItem.PhysicalPath), e.NewName);
		}

		protected void wcFileManager_FileUploading(object sender, IZ.WebFileManager.UploadFileCancelEventArgs e)
		{

		}

		protected void wcFileManager_NewFolderCreating(object sender, IZ.WebFileManager.NewFolderCancelEventArgs e)
		{

		}

		private string TrimPackageNameFromVirtualPathAndMakeRelative(string packageName, string virtualPath)
		{
			return virtualPath.Replace('/', '\\').Replace("\\" + packageName, "").TrimStart(new char [] { '\\' });
		}

		protected void wcFileManager_SelectedItemsAction(object sender, IZ.WebFileManager.SelectedItemsActionCancelEventArgs e)
		{
			//string relativePath = e.DestinationDirectory.FileManagerPath.Replace('/', '\\').TrimStart(new char[] { '\\' });
			string destinationRelativePath = TrimPackageNameFromVirtualPathAndMakeRelative(this.Target, e.DestinationDirectory.FileManagerPath);

			switch (e.Action)
			{
				case IZ.WebFileManager.SelectedItemsAction.Copy:
					foreach (var selectedItem in e.SelectedItems)
					{
						string sourceFile = TrimPackageNameFromVirtualPathAndMakeRelative(this.Target, selectedItem.FileManagerPath);
						AutoUpdateManager.CopyFileInPackage(this.Target, Path.GetDirectoryName(sourceFile), Path.GetFileName(sourceFile), destinationRelativePath);
					}
					break;

				case IZ.WebFileManager.SelectedItemsAction.Delete:
				    foreach (var selectedItem in e.SelectedItems)
				    {
						if ((File.GetAttributes(selectedItem.PhysicalPath) & FileAttributes.Directory) == FileAttributes.Directory)
							AutoUpdateManager.DeleteFolderFromPackage(this.Target, destinationRelativePath);
						else
							AutoUpdateManager.DeleteFileFromPackage(this.Target, destinationRelativePath, Path.GetFileName(selectedItem.PhysicalPath));
				    }
					break;

				case IZ.WebFileManager.SelectedItemsAction.Move:
					foreach (var selectedItem in e.SelectedItems)
					{
						if ((File.GetAttributes(selectedItem.PhysicalPath) & FileAttributes.Directory) == FileAttributes.Directory)
							AutoUpdateManager.MoveFolderInPackage(this.Target, TrimPackageNameFromVirtualPathAndMakeRelative(this.Target, selectedItem.FileManagerPath), destinationRelativePath);
						else
							AutoUpdateManager.MoveFileInPackage(this.Target, TrimPackageNameFromVirtualPathAndMakeRelative(this.Target, selectedItem.FileManagerPath), Path.GetFileName(selectedItem.PhysicalPath), destinationRelativePath, Path.GetFileName(selectedItem.PhysicalPath));
					}
					break;
			}
		}

		protected void wcFileManager_ToolbarCommand(object sender, CommandEventArgs e)
		{

		}

		protected void wcFileManager_SelectedItemsActionComplete(object sender, IZ.WebFileManager.SelectedItemsActionEventArgs e)
		{

		}

		protected void btnReturnToPackageManager_Click(object sender, EventArgs e)
		{
			if(CommitChanges() == true)
				Response.Redirect("EditPackage.aspx?target=" + Server.UrlEncode(Target) + "&packageUpdated=1");
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			Response.Redirect("EditPackage.aspx?target=" + Server.UrlEncode(Target));
		}
	}
}
