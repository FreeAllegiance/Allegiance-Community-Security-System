using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using Allegiance.CommunitySecuritySystem.Common.Enumerations;

namespace Allegiance.CommunitySecuritySystem.Management.Business
{
	public class Authorization
	{
		/// <summary>
		/// Returns true if the user is Admin or SuperAdmin.
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public static bool IsAdminOrSuperAdmin(IPrincipal user)
		{
			return 
				user.IsInRole(RoleType.Administrator.ToString())
				|| user.IsInRole(RoleType.SuperAdministrator.ToString());
		}

		/// <summary>
		/// Returns true if the user is Moderator, Zone Lead, Admin or SuperAdmin
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public static bool IsModeratorOrZoneLeadOrAdminOrSuperAdmin(IPrincipal user)
		{
			return
				user.IsInRole(RoleType.Administrator.ToString())
				|| user.IsInRole(RoleType.ZoneLeader.ToString())
				|| user.IsInRole(RoleType.Moderator.ToString())
				|| user.IsInRole(RoleType.SuperAdministrator.ToString());
			
		}

		/// <summary>
		/// Returns true if the user is Moderator, Zone Lead, Admin or SuperAdmin
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public static bool IsZoneLeadOrAdminOrSuperAdmin(IPrincipal user)
		{
			return
				user.IsInRole(RoleType.Administrator.ToString())
				|| user.IsInRole(RoleType.ZoneLeader.ToString())
				|| user.IsInRole(RoleType.SuperAdministrator.ToString());

		}

		public static bool IsLoggedIn(IPrincipal user)
		{
			if (user == null || String.IsNullOrEmpty(user.Identity.Name) == true)
				return false;
			else
				return true;
		}

	}
}
