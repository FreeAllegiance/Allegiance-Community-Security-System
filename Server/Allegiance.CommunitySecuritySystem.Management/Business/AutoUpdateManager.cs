using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Allegiance.CommunitySecuritySystem.Management.Properties;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UtilityWrapper;

namespace Allegiance.CommunitySecuritySystem.Management.Business
{
	public class AutoUpdateManager
	{
		private const string PackageGuidFilename = "packageGuid.txt";
		private const string ExcludedMarker = ".excluded";
		private const string ProtectedMarker = ".protected";
		private const string InclusionFileName = "packageInclusions.txt";


		private static AutoUpdateManager _instance = new AutoUpdateManager();

		private AutoUpdateManager()
		{
			if (Directory.Exists(PackageRoot) == false)
				Directory.CreateDirectory(PackageRoot);

			if (Directory.Exists(BackupRoot) == false)
				Directory.CreateDirectory(BackupRoot);

			if (Directory.Exists(PublicationRoot) == false)
				Directory.CreateDirectory(PublicationRoot);
		}

		public static string PackageRoot
		{
			get
			{
				return Path.Combine(Settings.Default.AutoUpdateRootDirectory, "Packages");
			}
		}

		public static string BackupRoot
		{
			get
			{
				return Path.Combine(Settings.Default.AutoUpdateRootDirectory, "Backups");
			}
		}

		public static string PublicationRoot
		{
			get
			{
				return Path.Combine(Settings.Default.AutoUpdateRootDirectory, "Publications");
			}
		}

		public static bool IsFilenameOrDirectorySafe(string packageName)
		{
			foreach (char c in packageName)
			{
				if (c == '/' || c == '\\' || c == ':' || c == 0x08)
					return false;
			}

			return true;
		}

		public static bool DeleteFileFromPackage(string packageName, string relativeDirectory, string fileName)
		{
			string targetDirectory = Path.Combine(Path.Combine(PackageRoot, packageName), relativeDirectory);
			string baseTargetFile = Path.Combine(targetDirectory, fileName);
			string targetFile = baseTargetFile;

			if (File.Exists(targetFile) == false)
				targetFile = baseTargetFile + ExcludedMarker;

			if (File.Exists(targetFile) == false)
				targetFile = baseTargetFile + ProtectedMarker;

			if (File.Exists(targetFile) == false)
				return false;

			File.Delete(targetFile);

			return true;
		}

		public static bool DeleteFolderFromPackage(string packageName, string relativeDirectory)
		{
			string targetDirectory = Path.Combine(Path.Combine(PackageRoot, packageName), relativeDirectory);

			if(Directory.Exists(targetDirectory) == true)
				Directory.Delete(targetDirectory, true);

			return true;
		}

		public static bool CopyFileInPackage(string packageName, string sourceRelativeDirectory, string sourceFilename, string destinationRelativeDirectory)
		{
			string sourceDirectory = Path.Combine(PackageRoot, Path.Combine(packageName, sourceRelativeDirectory));
			string destinationDirectory = Path.Combine(PackageRoot, Path.Combine(packageName, destinationRelativeDirectory));

			string baseSourceFilepath = Path.Combine(sourceDirectory, sourceFilename);
			string sourceFilepath = baseSourceFilepath;

			bool isProtected = false;
			bool isExcluded = false;

			if (File.Exists(sourceFilepath) == false)
			{
				sourceFilepath = baseSourceFilepath + ExcludedMarker;

				if (File.Exists(sourceFilepath) == false)
				{
					sourceFilepath = baseSourceFilepath + ProtectedMarker;

					if (File.Exists(sourceFilepath) == false)
						return false;
					else
						isProtected = true;
				}
				else
				{
					isExcluded = true;
				}
			}

			List<Char> escapeChars = new List<char>( new char [] { '(', ')', '{', '}', '[', ']', '.', '!', '&', '$', '^', '*', '?', '~'} );

			string unescapedPath = Path.GetFileNameWithoutExtension(baseSourceFilepath);
			string escapedPath = String.Empty;
			for (int i = 0; i < unescapedPath.Length; i++)
			{
				if (escapeChars.Contains(unescapedPath[i]) == true)
					escapedPath += "\\" + unescapedPath[i];
				else
					escapedPath += unescapedPath[i];
			}

			Regex filenameFinder = new Regex(escapedPath + @"(\(\d+\))?\" + Path.GetExtension(baseSourceFilepath) + "((\\" + ProtectedMarker + ")|(\\" + ExcludedMarker + "))?", RegexOptions.IgnoreCase);
			int existingCount = 0;
			foreach (string existingFilename in Directory.GetFiles(destinationDirectory))
			{
				if (filenameFinder.Match(existingFilename).Success == true)
					existingCount++;
			}

			string destinationFilepath;

			if (existingCount > 0)
			{
				destinationFilepath = Path.Combine(destinationDirectory, Path.GetFileNameWithoutExtension(baseSourceFilepath) + "(" + existingCount + ")" + Path.GetExtension(baseSourceFilepath));

				if (isProtected == true)
					destinationFilepath += ProtectedMarker;
				else if (isExcluded == true)
					destinationFilepath += ExcludedMarker;
			}
			else
				destinationFilepath = Path.Combine(destinationDirectory, sourceFilename);

			File.Copy(sourceFilepath, destinationFilepath);

			return true;
		}

		public static bool AddFileToPackage(string packageName, string relativeDirectory, string filename)
		{
			if (File.Exists(filename) == false)
				return false;

			string targetDirectory = Path.Combine(Path.Combine(PackageRoot, packageName), relativeDirectory);

			if (Directory.Exists(targetDirectory) == false)
				Directory.CreateDirectory(targetDirectory);

			string targetFile = Path.Combine(targetDirectory, Path.GetFileName(filename));

			if (File.Exists(targetFile + ExcludedMarker) == true)
				targetFile += ExcludedMarker;

			else if (File.Exists(targetFile + ProtectedMarker) == true)
				targetFile += ProtectedMarker;

			File.Copy(filename, targetFile, true);

			return true;
		}

		public static bool RenameFileInPackage(string packageName, string relativeDirectory, string oldFilename, string newFilename)
		{
			return MoveFileInPackage(packageName, relativeDirectory, oldFilename, relativeDirectory, newFilename);

			//string targetDirectory = Path.Combine(PackageRoot, Path.Combine(packageName, relativeDirectory));
			//string sourceFile = Path.Combine(targetDirectory, oldFilename);
			//string targetFile = Path.Combine(targetDirectory, newFilename);

			//if (File.Exists(sourceFile) == false)
			//    return false;

			//if (File.Exists(targetFile) == true)
			//    File.Delete(targetFile);

			//File.Move(sourceFile, targetFile);

			//return true;
		}

		public static bool MoveFileInPackage(string packageName, string sourceRelativeDirectory, string sourceFilename, string destinationRelativeDirectory, string destinationFilename)
		{
			string sourceDirectory = Path.Combine(PackageRoot, Path.Combine(packageName, sourceRelativeDirectory));
			string destinationDirectory = Path.Combine(PackageRoot, Path.Combine(packageName, destinationRelativeDirectory));

			string baseSourceFilepath = Path.Combine(sourceDirectory, sourceFilename);
			string sourceFilepath = baseSourceFilepath;

			if (File.Exists(sourceFilepath) == false)
				sourceFilepath = baseSourceFilepath + ExcludedMarker;

			if (File.Exists(sourceFilepath) == false)
				sourceFilepath = baseSourceFilepath + ProtectedMarker;

			if (File.Exists(sourceFilepath) == false)
				return false;

			string destinationFilePath = Path.Combine(destinationDirectory, Path.GetFileName(destinationFilename));

			File.Move(sourceFilepath, destinationFilePath);

			return true;
		}

		public static bool MoveFolderInPackage(string packageName, string sourceRelativeDirectory, string destinationRelativeDirectory)
		{
			string sourceDirectory = Path.Combine(PackageRoot, Path.Combine(packageName, sourceRelativeDirectory));
			string destinationDirectory = Path.Combine(PackageRoot, Path.Combine(packageName, destinationRelativeDirectory));

			Directory.Move(sourceDirectory, destinationDirectory);

			return true;
		}

		public static List<UpdateItem> GetFilesInUpdatePackage(string packageName)
		{
			return GetFilesInUpdatePackage(PackageRoot, packageName, false);
		}

		public static List<UpdateItem> GetFilesInUpdatePackage(string packageRoot, string packageName, bool includeHiddenFiles)
		{
			List<UpdateItem> allFiles = new List<UpdateItem>();

			string targetDirectory = Path.Combine(packageRoot, packageName);

			if (Directory.Exists(targetDirectory) == false)
				return allFiles;

			AddFilesToUpdateItems(packageRoot, packageName, targetDirectory, includeHiddenFiles, allFiles);

			return allFiles;
		}

		private static void AddFilesToUpdateItems(string packageRoot, string packageName, string targetDirectory, bool includeHiddenFiles, List<UpdateItem> allFiles)
		{
			string relativeDirectory = targetDirectory.Substring(Path.Combine(packageRoot, packageName).Length).TrimStart(new char[] { '\\' });

			foreach (string directory in Directory.GetDirectories(targetDirectory))
				AddFilesToUpdateItems(packageRoot, packageName, directory, includeHiddenFiles, allFiles);

			foreach (string filename in Directory.GetFiles(targetDirectory))
			{
				if (includeHiddenFiles == false && Path.GetFileName(filename).Equals(AutoUpdateManager.PackageGuidFilename) == true)
					continue;

				bool includedInPackage = true;
				bool isProtected = false;
				string cleanFilename = filename;
				if (cleanFilename.EndsWith(AutoUpdateManager.ExcludedMarker) == true)
				{
					cleanFilename = cleanFilename.Remove(filename.Length - AutoUpdateManager.ExcludedMarker.Length);
					includedInPackage = false;
				}

				if (cleanFilename.EndsWith(AutoUpdateManager.ProtectedMarker) == true)
				{
					cleanFilename = cleanFilename.Remove(filename.Length - AutoUpdateManager.ProtectedMarker.Length);
					isProtected = true;
				}

				UpdateItem updateInfo = new UpdateItem(packageName, Path.GetFileName(cleanFilename), relativeDirectory, new FileInfo(filename), includedInPackage, isProtected);

				allFiles.Add(updateInfo);
			}

		}

		public static FileInfo GetPackageInfo(string packageName)
		{
			string targetDirectory = Path.Combine(PackageRoot, packageName);

			return new FileInfo(targetDirectory);
		}


		public static bool CreatePackage(string packageName)
		{
			string targetDirectory = Path.Combine(PackageRoot, packageName);

			if (Directory.Exists(targetDirectory) == false)
				Directory.CreateDirectory(targetDirectory);

			string packageKeyFile = Path.Combine(targetDirectory, PackageGuidFilename);

			if (File.Exists(packageKeyFile) == true)
				return false;

			StreamWriter streamWriter = File.CreateText(packageKeyFile);
			streamWriter.Write(Guid.NewGuid().ToString());
			streamWriter.Close();

			return true;
		}

		public static Guid GetPackageGuid(string packageName)
		{
			string targetDirectory = Path.Combine(PackageRoot, packageName);
			string packageKeyFile = Path.Combine(targetDirectory, PackageGuidFilename);

			if (File.Exists(packageKeyFile) == false)
				CreatePackage(packageName);

			string packageGuid = File.ReadAllText(packageKeyFile);

			return new Guid(packageGuid);
		}

		public static bool RenamePackage(string oldPackageName, string newPackageName)
		{
			string sourceDirectory = Path.Combine(PackageRoot, oldPackageName);
			string targetDirectory = Path.Combine(PackageRoot, newPackageName);

			if (Directory.Exists(targetDirectory) == true)
				return false;

			if (Directory.Exists(sourceDirectory) == false)
				CreatePackage(newPackageName);
			else
				Directory.Move(sourceDirectory, targetDirectory);

			return true;
		}

		public static void SetFileInclusionForPackage(string packageName, string relativeDirectory, string fileName, bool isIncluded)
		{
			string includedFile;

			if (IsFilenameOrDirectorySafe(packageName) == false)
				throw new Exception("package name is invalid: " + packageName);

			if (IsFilenameOrDirectorySafe(fileName) == false)
				throw new Exception("file name is invalid: " + fileName);

			if (fileName.EndsWith(ExcludedMarker, StringComparison.InvariantCultureIgnoreCase) == true)
				includedFile = fileName.Remove(fileName.Length - AutoUpdateManager.ExcludedMarker.Length);
			else
				includedFile = fileName;

			// If the file was protected, then unprotect it.
			if (isIncluded == false)
			{
				if (IsFileProtected(packageName, relativeDirectory, includedFile) == true)
					SetFileProtectionForPackage(packageName, relativeDirectory, includedFile, false);
			}

			string excludedFile = includedFile + ExcludedMarker;

			if (isIncluded == true)
				RenameFileInPackage(packageName, relativeDirectory, excludedFile, includedFile);
			else
				RenameFileInPackage(packageName, relativeDirectory, includedFile, excludedFile);
		}

		

		public static void SetFileProtectionForPackage(string packageName, string relativeDirectory, string fileName, bool isProtected)
		{
			string unprotectedFile;

			if (IsFilenameOrDirectorySafe(packageName) == false)
				throw new Exception("package name is invalid: " + packageName);

			if (IsFilenameOrDirectorySafe(fileName) == false)
				throw new Exception("file name is invalid: " + fileName);

			if (fileName.EndsWith(ProtectedMarker, StringComparison.InvariantCultureIgnoreCase) == true)
				unprotectedFile = fileName.Remove(fileName.Length - ProtectedMarker.Length);
			else
				unprotectedFile = fileName;

			string protectedFile = unprotectedFile + ProtectedMarker;

			if (isProtected == true)
				RenameFileInPackage(packageName, relativeDirectory, unprotectedFile, protectedFile);
			else
				RenameFileInPackage(packageName, relativeDirectory, protectedFile, unprotectedFile);
		}


		public static int GetCountPackages()
		{
			return Directory.GetDirectories(PackageRoot).Count();
		}

		public static int GetCountBackups()
		{
			return Directory.GetDirectories(BackupRoot).Count();
		}

		public static int GetCountPublications()
		{
			return GetPublications().Count();
		}

		public static List<DataAccess.Lobby> GetPublications()
		{
			using (var db = new DataAccess.CSSDataContext())
			{
				return db.Lobbies.ToList();
			}
		}

		public static string GetPublicationName(int publicationID)
		{
			using (var db = new DataAccess.CSSDataContext())
			{
				var lobby = db.Lobbies.FirstOrDefault(p => p.Id == publicationID);
				if (lobby != null)
					return lobby.Name;
			}

			return null;
		}

		public static List<FileInfo> GetPackages()
		{
			List<FileInfo> packageInfos = new List<FileInfo>();

			foreach (string directoryName in Directory.GetDirectories(PackageRoot))
				packageInfos.Add(new FileInfo(directoryName));

			return packageInfos;
		}

		public static bool DoesPackageExist(string packageName)
		{
			return Directory.Exists(Path.Combine(PackageRoot, packageName));
		}

		public static bool IsFileProtected(string packageName, string relativeDirectory, string filename)
		{
			string targetDirectory = Path.Combine(Path.Combine(PackageRoot, packageName), relativeDirectory);
			string targetFile = Path.Combine(targetDirectory, filename) + ProtectedMarker;

			return File.Exists(targetFile);
		}

		public static List<Guid> GetIncludedPackageGuids(int publicationID)
		{
			string publicationPath = Path.Combine(PublicationRoot, publicationID.ToString());
			string inclusionFile = Path.Combine(publicationPath, InclusionFileName);

			List<Guid> returnValue = new List<Guid>();

			if (File.Exists(inclusionFile) == true)
			{
				foreach (string line in File.ReadAllLines(inclusionFile))
					returnValue.Add(new Guid(line));
			}

			return returnValue;
		}

		public static bool IsPackageExcludedFromPublication(int publicationID, string packageName)
		{
			List<Guid> includedPackageGuids = GetIncludedPackageGuids(publicationID);
			Guid packageGuid = GetPackageGuid(packageName);

			return includedPackageGuids.Contains(packageGuid) == false;
		}

		public static void SetPackageInclusionForPublication(int publicationID, string packageName, bool isIncluded)
		{
			List<Guid> includedPackageGuids = GetIncludedPackageGuids(publicationID);
			Guid packageGuid = GetPackageGuid(packageName);

			if (isIncluded == true && includedPackageGuids.Contains(packageGuid) == false)
				includedPackageGuids.Add(packageGuid);

			else if(isIncluded == false && includedPackageGuids.Contains(packageGuid) == true)
				includedPackageGuids.Remove(packageGuid);

			string publicationPath = Path.Combine(PublicationRoot, publicationID.ToString());

			if (Directory.Exists(publicationPath) == false)
				Directory.CreateDirectory(publicationPath);

			string exclusionFile = Path.Combine(publicationPath, InclusionFileName);

			StreamWriter streamWriter = File.CreateText(exclusionFile);

			foreach (Guid includedPackageGuid in includedPackageGuids)
				streamWriter.WriteLine(includedPackageGuid.ToString());

			streamWriter.Close();
		}

		public static bool TryGetPublicationFiles(int publicationID, out Dictionary<string, UpdateItem> filesInPublication, out List<FileCollision> fileCollisions)
		{
			filesInPublication = new Dictionary<string, UpdateItem>();
			fileCollisions = new List<FileCollision>();

			foreach (FileInfo packageInfo in AutoUpdateManager.GetPackages())
			{
				if (AutoUpdateManager.IsPackageExcludedFromPublication(publicationID, packageInfo.Name) == false)
				{
					foreach (UpdateItem fileInfo in AutoUpdateManager.GetFilesInUpdatePackage(packageInfo.Name))
					{
						if (fileInfo.IsIncluded == false)
							continue;

						string relativeFilePath = Path.Combine(fileInfo.RelativeDirectory, fileInfo.Name);

						if (filesInPublication.ContainsKey(relativeFilePath) == true)
						{
							FileCollision fileCollision = new FileCollision();
							fileCollision.CollidingFile = Path.Combine(packageInfo.Name, relativeFilePath);
							fileCollision.PreferredFile = Path.Combine(filesInPublication[relativeFilePath].PackageName, relativeFilePath);

							fileCollisions.Add(fileCollision);

							continue;
						}
						else
						{
							filesInPublication.Add(relativeFilePath, fileInfo);
						}
					}
				}
			}

			return true;
		}



		public static void MovePackageUpInPublication(int publicationID, string packageName)
		{
			MovePackageUpOrDown(publicationID, packageName, true);
		}

		public static void MovePackageDownInPublication(int publicationID, string packageName)
		{
			MovePackageUpOrDown(publicationID, packageName, true);
		}

		// TODO: Finish this one up later.
		private static void MovePackageUpOrDown(int publicationID, string packageName, bool moveUp)
		{

		}

		public static List<FileInfo> GetAvailableBackups()
		{
			List<FileInfo> backupInfos = new List<FileInfo>();

			foreach (string backupDirectory in Directory.GetDirectories(BackupRoot))
			{
				backupInfos.Add(new FileInfo(backupDirectory));
			}

			backupInfos.Sort(delegate(FileInfo f1, FileInfo f2)
			{
				return f2.CreationTime.CompareTo(f1.CreationTime);
			});

			return backupInfos;
		}

		public static bool CreateBackup(string backupName)
		{
			if (IsFilenameOrDirectorySafe(backupName) == false)
				return false;

			string targetDir = Path.Combine(BackupRoot, backupName);

			int nextBackup = Directory.GetDirectories(BackupRoot, backupName + " #*").Length + 1;
			targetDir += " #" + nextBackup;

			if (Directory.Exists(targetDir) == true)
				return false;

			if (Directory.Exists(PackageRoot) == false)
				Directory.CreateDirectory(PackageRoot);

			if (Directory.Exists(PublicationRoot) == false)
				Directory.CreateDirectory(PublicationRoot);

			Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(PackageRoot, Path.Combine(targetDir, Path.GetFileName(PackageRoot)), true);
			Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(PublicationRoot, Path.Combine(targetDir, Path.GetFileName(PublicationRoot)), true);

			return true;
		}

		internal static void RestoreFile(string backupName, string type, string container, string relativeDirectory, string file)
		{
			if (IsFilenameOrDirectorySafe(backupName) == false)
				throw new Exception("backup name is invalid.");

			string sourceDirectory = Path.Combine(BackupRoot, backupName);
			string filePathPart = GetFilePath(type, container, relativeDirectory, file);
			string sourcePathBaseFile = Path.Combine(sourceDirectory, filePathPart);
			string targetPathBaseFile = Path.Combine(Settings.Default.AutoUpdateRootDirectory, filePathPart);

			string sourcePath = sourcePathBaseFile;
			string targetPath = targetPathBaseFile;

			if (File.Exists(sourcePath) == false)
			{
				sourcePath = sourcePathBaseFile + ExcludedMarker;
				targetPath = targetPathBaseFile + ExcludedMarker;
			}

			if (File.Exists(sourcePath) == false)
			{
				sourcePath = sourcePathBaseFile + ProtectedMarker;
				targetPath = targetPathBaseFile + ProtectedMarker;
			}

			if (File.Exists(sourcePath) == false)
				throw new Exception("File: " + sourcePathBaseFile + " does not exist.");

			if (Directory.Exists(Path.GetDirectoryName(targetPath)) == false)
			{
				DirectoryInfo targetDirectoryInfo = new DirectoryInfo(Path.GetDirectoryName(targetPath));
				
				if (targetDirectoryInfo.Parent.FullName.Equals(PackageRoot, StringComparison.InvariantCultureIgnoreCase) == true)
					CreatePackage(targetDirectoryInfo.Name);
				else
					Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
			}

			File.Copy(sourcePath, targetPath, true);
		}

		public static BackupItems GetFilesInBackup(string backupName)
		{
			BackupItems backupItems = new BackupItems();

			string backupDirectory = Path.Combine(BackupRoot, backupName);
			string backupPublicationDirectory = Path.Combine(backupDirectory, Path.GetFileName(PublicationRoot));
			string backupPackageDirectory = Path.Combine(backupDirectory, Path.GetFileName(PackageRoot));

			List<UpdateItem> filesInBackup = new List<UpdateItem>();

			foreach (string publicationSubDirectory in Directory.GetDirectories(backupPublicationDirectory))
			{
				foreach (string filename in Directory.GetFiles(publicationSubDirectory))
				{
					FileInfo fileInfo = new FileInfo(filename);
					backupItems.PublicationFiles.Add(fileInfo);
				}
			}

			foreach (string packageSubDirectory in Directory.GetDirectories(backupPackageDirectory))
			{
				List<UpdateItem> updateItems = GetFilesInUpdatePackage(backupPackageDirectory, Path.GetFileName(packageSubDirectory), true);

				backupItems.PackageFiles.AddRange(updateItems);

				//foreach (string filename in Directory.GetFiles(packageSubDirectory))
				//	filesInBackup.Add(new FileInfo(filename));
			}

			return backupItems;
		}

		public static DirectoryInfo GetBackupInfo(string backupName)
		{
			string backupDirectory = Path.Combine(BackupRoot, backupName);
			DirectoryInfo backupInfo = new DirectoryInfo(backupDirectory);

			return backupInfo;
		}


		/// <summary>
		/// Checks to ensure that all the parts of the file path are legal, and then 
		/// reassembles them into the physical path to the file on the local disk.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="container"></param>
		/// <param name="file"></param>
		/// <returns></returns>
		public static string GetFilePath(string type, string container, string relativeDirectory, string file)
		{
			if (AutoUpdateManager.IsFilenameOrDirectorySafe(type) == false)
				throw new Exception("Invalid type.");

			if (AutoUpdateManager.IsFilenameOrDirectorySafe(container) == false)
				throw new Exception("Invalid container.");

			// The relative directory can contain \, but it shouldn't contain .. ever!
			if(relativeDirectory.StartsWith("\\") || relativeDirectory.StartsWith("//") || relativeDirectory.Contains(".."))
				throw new Exception("Invalid relative directory.");

			if (AutoUpdateManager.IsFilenameOrDirectorySafe(file) == false)
				throw new Exception("Invalid filename.");

			string containerPath = Path.Combine(type, container);
			string relativePath = containerPath;

			if (String.IsNullOrEmpty(relativeDirectory) == false)
				relativePath = Path.Combine(containerPath, relativeDirectory);

			string filePath = Path.Combine(relativePath, file);

			return filePath;
		}

		public static bool DeletePackage(string packageName)
		{
			if (IsFilenameOrDirectorySafe(packageName) == false)
				throw new Exception("package name is invalid: " + packageName);

			CreateBackup("Auto Backup - Deleting Package " + packageName);

			Directory.Delete(Path.Combine(PackageRoot, packageName), true);

			return true;
		}

		public static bool DeployPublication(int publicationID)
		{
			using (var db = new DataAccess.CSSDataContext())
			{
				DataAccess.Lobby lobby = db.Lobbies.FirstOrDefault(p => p.Id == publicationID);

				if(lobby == null)
					throw new Exception("Couldn't get lobby for publication id: " + publicationID);

				if (Directory.Exists(lobby.BasePath) == false)
					Directory.CreateDirectory(lobby.BasePath);

				List<FileCollision> fileCollisions = new List<FileCollision>();
				Dictionary<string, UpdateItem> filesInPublication = new Dictionary<string, UpdateItem>();

				// Remove physical files from the directory. Not doing this with a recursive directory delete because 
				// I don't want someone to put in a bad path into the content manager web UI, and then drill the 
				// whole drive. 
				foreach (DataAccess.AutoUpdateFile_Lobby file in db.AutoUpdateFile_Lobbies.Where(p => p.LobbyId == lobby.Id))
				{
					string fileToDelete = Path.Combine(lobby.BasePath, file.AutoUpdateFile.Filename);
					
					if (File.Exists(fileToDelete) == true)
						File.Delete(fileToDelete);
				}

				// Clear all files for the lobby.
				db.AutoUpdateFile_Lobbies.DeleteAllOnSubmit(db.AutoUpdateFile_Lobbies.Where(p => p.LobbyId == lobby.Id));
				db.SubmitChanges();

				if (AutoUpdateManager.TryGetPublicationFiles(publicationID, out filesInPublication, out fileCollisions) == true)
				{
					foreach (UpdateItem fileInfo in filesInPublication.Values)
					{
						string checksum;
						using (SHA1 hasher = SHA1.Create())
						{
							using (FileStream fs = new FileStream(fileInfo.FileInfo.FullName, FileMode.Open, FileAccess.Read))
								checksum = Convert.ToBase64String(hasher.ComputeHash(fs));
						}

						string fileVersion = String.Empty;
						FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(fileInfo.FileInfo.FullName);

						// Doing it this way, as sometimes there is product or vendor info at the 
						// end of the file version spec. ProductVersion may not correctly reflect the actual 
						// version of the file all the time.
						if (fileVersionInfo != null && fileVersionInfo.FileVersion != null)
							fileVersion = String.Format("{0}.{1}.{2}.{3}", fileVersionInfo.FileMajorPart, fileVersionInfo.FileMinorPart, fileVersionInfo.FileBuildPart, fileVersionInfo.FilePrivatePart);

						string relativeFilePath = Path.Combine(fileInfo.RelativeDirectory, fileInfo.Name);

						DataAccess.AutoUpdateFile autoUpdateFile = db.AutoUpdateFiles.FirstOrDefault(p => p.Filename == relativeFilePath);

						if (autoUpdateFile == null)
						{
							autoUpdateFile = new Allegiance.CommunitySecuritySystem.DataAccess.AutoUpdateFile()
							{
								Filename = relativeFilePath,
								IsProtected = fileInfo.IsProtected
							};

							db.AutoUpdateFiles.InsertOnSubmit(autoUpdateFile);
							db.SubmitChanges();
						}
						else
						{
							if (autoUpdateFile.IsProtected != fileInfo.IsProtected)
							{
								autoUpdateFile.IsProtected = fileInfo.IsProtected;
								db.SubmitChanges();
							}
						}

						DataAccess.AutoUpdateFile_Lobby lobbyFile = db.AutoUpdateFile_Lobbies.FirstOrDefault(p => p.AutoUpdateFileId == autoUpdateFile.Id && p.LobbyId == lobby.Id);

						if (lobbyFile == null)
						{
							lobbyFile = new Allegiance.CommunitySecuritySystem.DataAccess.AutoUpdateFile_Lobby()
							{
								AutoUpdateFileId = autoUpdateFile.Id,
								CurrentVersion = fileVersion,
								DateCreated = fileInfo.FileInfo.CreationTime,
								DateModified = fileInfo.FileInfo.LastWriteTime,
								ValidChecksum = checksum,
								LobbyId = lobby.Id
							};

							db.AutoUpdateFile_Lobbies.InsertOnSubmit(lobbyFile);
							db.SubmitChanges();
						}

						string targetFilePath = Path.Combine(lobby.BasePath, relativeFilePath);
						string targetFileDirectory = Path.GetDirectoryName(targetFilePath);
						if (Directory.Exists(targetFileDirectory) == false)
							Directory.CreateDirectory(targetFileDirectory);

						File.Copy(fileInfo.FileInfo.FullName, targetFilePath, true);
					}

					GenerateFileListForAutoUpdate(lobby);
				}

				// Clean up any unused AutoUpdateFile records.
				//db.AutoUpdateFiles.DeleteAllOnSubmit(db.AutoUpdateFiles.Where(p => db.AutoUpdateFile_Lobbies.Select(r => r.AutoUpdateFileId).Contains(p.Id) == false));
				//db.SubmitChanges();
			}

			return true;
		}

		private static void GenerateFileListForAutoUpdate(DataAccess.Lobby lobby)
		{
			string filelistFile = Path.Combine(lobby.BasePath, "FileList.txt");

			if (File.Exists(filelistFile) == true)
				File.Delete(filelistFile);

			AddFilesToFileList(filelistFile, String.Empty, lobby.BasePath);
		}

		unsafe private static void AddFilesToFileList(string filelistFile, string path, string lobbyBasePath)
		{
			string currentPath = lobbyBasePath;

			if(path.Length > 0)
				currentPath = Path.Combine(lobbyBasePath, path);

			foreach (string filename in Directory.GetFiles(currentPath))
			{
				string subFilePath = Path.GetFileName(filename);
				
				if(path.Length > 0)
					subFilePath= Path.Combine(path, Path.GetFileName(filename));

				FileInfo fileInfo = new FileInfo(filename);
				
				//uint crcChecksum = CRCUtility.ChecksumFile(filename);

				UtilityWrapper.CRCUtils crcUtils = new CRCUtils();
				string errorMessage = String.Empty;

				int crcChecksum = crcUtils.GetCrc32ForFile(filename, ref errorMessage);

				File.AppendAllText(filelistFile,
					String.Format("{0} {1,9} {2,8:X} {3}{4}", 
						fileInfo.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss"), 
						fileInfo.Length.ToString("D9"),
						crcChecksum.ToString("X8"),
						subFilePath,
						System.Environment.NewLine));
			}

			foreach (string directory in Directory.GetDirectories(currentPath))
			{
				AddFilesToFileList(filelistFile, Path.Combine(path, Path.GetFileName(directory)), lobbyBasePath);
			}
		}
	}
}
