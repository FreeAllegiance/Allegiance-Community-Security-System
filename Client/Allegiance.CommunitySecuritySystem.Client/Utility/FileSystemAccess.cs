using System;
using System.Collections.Generic;
using System.Text;
using System.Security.AccessControl;
using System.IO;
using System.Security.Principal;
using System.Diagnostics;

namespace Allegiance.CommunitySecuritySystem.Client.Utility
{
	public static class FileSystemAccess
	{
		public static bool DoesUserHaveAccessToDirectory(string path, string usernameOrSID, System.Security.AccessControl.FileSystemRights fileSystemRights)
		{
			DirectorySecurity directorySecurity = Directory.GetAccessControl(path, AccessControlSections.All);

			foreach (FileSystemAccessRule fileSystemAccessRule in directorySecurity.GetAccessRules(true, true, typeof(NTAccount)))
			{
				IdentityReference sidIdentityReference = fileSystemAccessRule.IdentityReference.Translate(typeof(SecurityIdentifier));

				Debug.WriteLine(String.Format("IdentityReference: {0}", fileSystemAccessRule.IdentityReference.Value));
				Debug.WriteLine(String.Format("FileSystemRights: {0}", fileSystemAccessRule.FileSystemRights));
				Debug.WriteLine("");

				if (
						(usernameOrSID.Equals(fileSystemAccessRule.IdentityReference.Value, StringComparison.InvariantCultureIgnoreCase) == true
						|| usernameOrSID.Equals(sidIdentityReference.Value, StringComparison.InvariantCultureIgnoreCase) == true)
					&& (fileSystemAccessRule.FileSystemRights & fileSystemRights) == fileSystemRights)
					return true;
			}

			return false;
		}

		public static void SetDirectoryAccessByUserName(string path, string userName, System.Security.AccessControl.FileSystemRights fileSystemRights)
		{
			NTAccount userAccount = new NTAccount(userName);
			SetDirectoryAccess(path, userAccount, fileSystemRights);
		}

		public static void SetDirectoryAccessBySID(string path, string sidString, System.Security.AccessControl.FileSystemRights fileSystemRights)
		{
			SecurityIdentifier sid = new SecurityIdentifier(sidString);
			SetDirectoryAccess(path, sid, fileSystemRights);
		}

		private static void SetDirectoryAccess(string path, IdentityReference user, System.Security.AccessControl.FileSystemRights fileSystemRights)
		{
			DirectorySecurity directorySecurity = Directory.GetAccessControl(path, AccessControlSections.All);

			directorySecurity.AddAccessRule(new FileSystemAccessRule(user, fileSystemRights, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));

			Directory.SetAccessControl(path, directorySecurity);
		}
	}
}
