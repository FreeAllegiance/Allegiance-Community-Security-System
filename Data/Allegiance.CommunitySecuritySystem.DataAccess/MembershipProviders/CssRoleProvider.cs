using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration.Provider;
using System.Data.Linq.SqlClient;

namespace Allegiance.CommunitySecuritySystem.DataAccess.MembershipProviders
{
	public class CssRoleProvider : System.Web.Security.RoleProvider
	{
		public override void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			using (var db = new DataAccess.CSSDataContext())
			{
				foreach (string username in usernames)
				{
					DataAccess.Login login = db.Logins.FirstOrDefault(p => p.Username == username.Trim());

					if (login != null)
					{
						foreach(string roleName in roleNames)
						{
							DataAccess.Role role = db.Roles.FirstOrDefault(p => p.Name == roleName.Trim());

							if (login.Login_Roles.Count(p => p.LoginId == login.Id && p.RoleId == role.Id) > 0)
								continue;

							db.Login_Roles.InsertOnSubmit(new DataAccess.Login_Role()
							{
								Login = login,
								LoginId = login.Id,
								Role = role,
								RoleId = role.Id
							});
						}
					}
				}

				db.SubmitChanges();
			}
		}

		private string _applicationName = "CssMembershipProvider";
		public override string ApplicationName
		{
			get
			{
				return _applicationName;
			}
			set
			{
				_applicationName = value;
			}
		}

		public override void CreateRole(string roleName)
		{
			using (var db = new DataAccess.CSSDataContext())
			{
				db.Roles.InsertOnSubmit(new Allegiance.CommunitySecuritySystem.DataAccess.Role()
				{
					Name = roleName
				});

				db.SubmitChanges();
			}
		}

		public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
		{
			using (var db = new DataAccess.CSSDataContext())
			{
				var role = db.Roles.FirstOrDefault(p => p.Name == roleName.Trim());

				if (role == null)
					return false;

				if (role.Login_Roles.Count() > 0 && throwOnPopulatedRole == true)
					throw new ProviderException("This role is being used by one or more logins!");

				db.Roles.DeleteOnSubmit(role);
				db.SubmitChanges();
			}

			return true;
		}

		public string[] FindUsersInRole(string roleName)
		{
			return FindUsersInRole(roleName, null);
		}

		public override string[] FindUsersInRole(string roleName, string usernameToMatch)
		{
			List<string> usernamesInRole = new List<string>();

			using (var db = new DataAccess.CSSDataContext())
			{
				var role = db.Roles.FirstOrDefault(p => p.Name == roleName.Trim());
				if(role != null)
				{
					DataAccess.Login_Role loginRole = db.Login_Roles.FirstOrDefault(p => p.RoleId == role.Id);

					if (loginRole != null)
					{
						foreach (var login in db.Logins.Where(p => usernameToMatch == null || SqlMethods.Like(p.Username, usernameToMatch)))
						{
							if (login.Login_Roles.Contains(loginRole) == true)
								usernamesInRole.Add(login.Username);
						}
					}
				}
			}

			return usernamesInRole.ToArray();
		}

		public override string[] GetAllRoles()
		{
			List<string> allRoles = new List<string>();

			using (var db = new DataAccess.CSSDataContext())
			{
				foreach (var role in db.Roles)
					allRoles.Add(role.Name);
			}

			return allRoles.ToArray();
		}

		public override string[] GetRolesForUser(string username)
		{
			List<string> rolesForUser = new List<string>();

			using (var db = new DataAccess.CSSDataContext())
			{
				DataAccess.Login login = Login.FindLoginByUsernameOrCallsign(db, username);

				if (login != null)
				{
					foreach (DataAccess.Login_Role loginRole in login.Login_Roles)
					{
						rolesForUser.Add(loginRole.Role.Name);
					}
				}
			}

			return rolesForUser.ToArray();
		}

		public override string[] GetUsersInRole(string roleName)
		{
			return FindUsersInRole(roleName);
		}

		public override bool IsUserInRole(string username, string roleName)
		{
			string[] usersInRole = FindUsersInRole(roleName, username);
			foreach (string userInRole in usersInRole)
			{
				if (userInRole == username.Trim() == true)
					return true;
			}

			return false;
		}

		public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
		{
			using (var db = new DataAccess.CSSDataContext())
			{
				foreach (string username in usernames)
				{
					DataAccess.Login login = db.Logins.FirstOrDefault(p => p.Username == username.Trim());

					foreach (DataAccess.Login_Role loginRole in login.Login_Roles)
					{
						foreach(string roleName in roleNames)
						{
							if (loginRole.Role.Name.Equals(roleName.Trim()) == true)
								db.Login_Roles.DeleteOnSubmit(loginRole);
						}
					}
				}

				db.SubmitChanges();
			}
		}

		public override bool RoleExists(string roleName)
		{
			using (var db = new DataAccess.CSSDataContext())
			{
				return db.Roles.Count(p => p.Name == roleName.Trim()) > 0;
			}
		}
	}
}
