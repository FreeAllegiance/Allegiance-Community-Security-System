using System;
using System.Collections.Generic;
using System.Text;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Diagnostics;
using Microsoft.Win32;

namespace Allegiance.CommunitySecuritySystem.Client.Utility
{
	// http://msdn.microsoft.com/en-us/library/system.security.accesscontrol.registrysecurity.aspx
	public static class RegistryAccess
	{
		public static bool DoesUserHaveAccess(RegistryKey registryKey, string userNameOrSID, RegistryRights accessType)
		{
			RegistrySecurity registrySecurity = registryKey.GetAccessControl();

			foreach (RegistryAccessRule registryAccessRule in registrySecurity.GetAccessRules(true, true, typeof(NTAccount)))
			{
				IdentityReference sidIdentityReference = registryAccessRule.IdentityReference.Translate(typeof(SecurityIdentifier));

				if (
					(userNameOrSID.Equals(registryAccessRule.IdentityReference.Value, StringComparison.InvariantCultureIgnoreCase) == true
						|| userNameOrSID.Equals(sidIdentityReference.Value, StringComparison.InvariantCultureIgnoreCase) == true)
					&& (registryAccessRule.RegistryRights & accessType) == accessType)
					return true;
			}

			return false;
		}

		public static void SetUserAccessByUserName(RegistryKey registryKey, string userName, RegistryRights accessType)
		{
			NTAccount userAccount = new NTAccount(userName);
			SetUserAccess(registryKey, userAccount, accessType);
		}

		public static void SetUserAccessBySID(RegistryKey registryKey, string sidString, RegistryRights accessType)
		{
			SecurityIdentifier sid = new SecurityIdentifier(sidString);
			SetUserAccess(registryKey, sid, accessType);
		}

		private static void SetUserAccess(RegistryKey registryKey, IdentityReference user, RegistryRights accessType)
		{
			RegistrySecurity registrySecurity = registryKey.GetAccessControl();

			RegistryAccessRule rule = new RegistryAccessRule(
				user,
				accessType,
				InheritanceFlags.ContainerInherit,
				PropagationFlags.None,
				AccessControlType.Allow
			);

			registrySecurity.AddAccessRule(rule);

			registryKey.SetAccessControl(registrySecurity);
		}
	}
}
