using System;
using System.Linq;
using Allegiance.CommunitySecuritySystem.Common.Enumerations;
using Allegiance.CommunitySecuritySystem.DataAccess.Enumerations;
using Allegiance.CommunitySecuritySystem.DataAccess.MembershipProviders;
using Allegiance.CommunitySecuritySystem.Common.Utility;
using System.Collections.Generic;

namespace Allegiance.CommunitySecuritySystem.DataAccess
{
    public partial class Login
    {
        #region Properties

        /// <summary>
        /// Check if any of the user's logins are banned
        /// </summary>
        public bool IsBanned
        {
            get
            {
				return Identity.Bans.Any(p => p.InEffect && (p.DateExpires == null || p.DateExpires > DateTime.Now));
            }
        }

        #endregion

        #region Methods

		public static List<Login> GetAllFromCache()
		{
			return CacheManager<List<Login>>.Get("Login::GetAll()", CacheSeconds.TenSeconds, delegate()
			{
				using (CSSDataContext db = new CSSDataContext())
				{
					return db.Logins.ToList();
				}
			});
		}

		public static bool TryGetAuthenticatedLogin(CSSDataContext db, string username, string password, out Login login, out LoginStatus loginStatus)
		{
			loginStatus = LoginStatus.Authenticated;

			login = Login.FindLoginByUsernameOrCallsign(db, username);

			if (login == null)
				loginStatus = LoginStatus.InvalidCredentials;
			else if (login.IsBanned)
				loginStatus = LoginStatus.AccountLocked;
			else
			{
				CssMembershipProvider provider = new CssMembershipProvider();
				if (provider.ValidateUser(login.Username, password) == false)
					loginStatus = LoginStatus.InvalidCredentials;
				else
					loginStatus = LoginStatus.Authenticated;
			}

			return loginStatus == LoginStatus.Authenticated;
		}

		public static Login FindLoginByUsernameOrCallsign(CSSDataContext db, string usernameOrCallsign)
		{
			var login = FindLoginByUsername(db, usernameOrCallsign);

			if (login == null)
				login = FindLoginByCallsign(db, usernameOrCallsign);

			return login;
		}

		internal static Login FindLoginByUsernameOrCallsign(CSSDataContext db, string usernameOrCallsign, string password)
		{
			var login = FindLoginByUsernameOrCallsign(db, usernameOrCallsign);

			if (login != null)
			{
				if (PasswordHash.ValidatePassword(password, login.Password) == true)
					return login;
			}

			return null;
		}

        /// <summary>
        /// Find the account associated with the input credentials.
        /// </summary>
        private static Login FindLogin(CSSDataContext db, string username, string password)
        {
            return db.Logins.FirstOrDefault(p => p.Username == username && p.Password == password);
        }

		/// <summary>
		/// Find the account associated with the input credentials.
		/// </summary>
		private static Login FindLoginByUsername(CSSDataContext db, string username)
		{
			return db.Logins.FirstOrDefault(p => p.Username == username);
		}

        /// <summary>
        /// Find the account associated with the input alias.
        /// </summary>
		private static Login FindLoginByCallsign(CSSDataContext db, string callsign)
        {
			string cleanCallsign = Alias.GetCallsignFromStringWithTokensAndTags(db, callsign);

			return db.Logins.FirstOrDefault(p => p.Aliases.Any(q => q.Callsign == cleanCallsign));
        }

        /// <summary>
        /// Find the current session associated with this login.
        /// </summary>
        public Session FindCurrentSession()
        {
            return Sessions
                .OrderByDescending(p => p.DateLastCheckIn)
                .Where(p => p.SessionStatusType == SessionStatusEnum.Active
                    || p.SessionStatusType == SessionStatusEnum.PendingVerification)
                .FirstOrDefault();
        }

        public bool HasAnyRole(RoleType[] types)
        {
            foreach (var role in types)
            {
                if (HasRole(role))
                    return true;
            }
            return false;
        }

        public bool HasRole(RoleType type)
        {
            return Login_Roles.Any(p => p.RoleId == (int)type);
        }
        
        #endregion


	}
}