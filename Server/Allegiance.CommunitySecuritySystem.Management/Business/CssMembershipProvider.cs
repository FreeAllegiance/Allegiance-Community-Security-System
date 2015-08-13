using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Linq.Expressions;
using System.Data.Linq.SqlClient;

namespace Allegiance.CommunitySecuritySystem.Management.Business
{
	public class CssMembershipProvider : System.Web.Security.MembershipProvider
	{
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

		public override bool ChangePassword(string username, string oldPassword, string newPassword)
		{
			string oldPasswordHash = Allegiance.CommunitySecuritySystem.Common.Utility.Encryption.SHA256Hash(oldPassword);
			string newPasswordHash = Allegiance.CommunitySecuritySystem.Common.Utility.Encryption.SHA256Hash(newPassword);

			using (var db = new DataAccess.CSSDataContext())
			{
				DataAccess.Login login = DataAccess.Login.FindLogin(db, username, oldPasswordHash);

				if (login == null)
					return false;

				login.Password = newPasswordHash;
				db.SubmitChanges();
			}

			return true;
		}

		public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
		{
			throw new NotImplementedException();
		}

		public override System.Web.Security.MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out System.Web.Security.MembershipCreateStatus status)
		{
			string passwordHash = Allegiance.CommunitySecuritySystem.Common.Utility.Encryption.SHA256Hash(password);

			DataAccess.Identity identity = null;

			using (var db = new DataAccess.CSSDataContext())
			{
				if (DataAccess.Login.FindLoginByUsername(db, username) != null)
				{
					status = System.Web.Security.MembershipCreateStatus.DuplicateUserName;
					return null;
				}

				if (DataAccess.Alias.ListAliases(db, username).Count > 0)
				{
					status = System.Web.Security.MembershipCreateStatus.UserRejected;
					return null;
				}

				if (DataAccess.Identity.TryCreateIdentity(db, username, passwordHash, email, out identity) == true)
					db.SubmitChanges();

				if(identity != null)
				{
					DataAccess.Login createdLogin = DataAccess.Login.FindLoginByUsername(db, username);

					if(createdLogin != null)
					{
						status = System.Web.Security.MembershipCreateStatus.Success;
						return MembershipUserUtility.CreateMembershipUserFromLogin(createdLogin);
					}
				}
			}

			status = System.Web.Security.MembershipCreateStatus.ProviderError;
			return null;
		}

		public override bool DeleteUser(string username, bool deleteAllRelatedData)
		{
			using (var db = new DataAccess.CSSDataContext())
			{
				DataAccess.Login login = DataAccess.Login.FindLoginByUsername(db, username);

				DataAccess.Identity identity = db.Identities.FirstOrDefault(p => p.Id == login.IdentityId);

				if (identity != null)
				{
					db.Identities.DeleteOnSubmit(identity);
					db.SubmitChanges();
					return true;
				}
			}

			return false;
		}

		public override bool EnablePasswordReset
		{
			get { return true; }
		}

		public override bool EnablePasswordRetrieval
		{
			get { return false; }
		}

		public override System.Web.Security.MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			return MembershipUserUtility.FindUsersByPredicate(p => SqlMethods.Like(p.Email, emailToMatch.Trim()), pageIndex, pageSize, out totalRecords);
		}

		public override System.Web.Security.MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			return MembershipUserUtility.FindUsersByPredicate(p => SqlMethods.Like(p.Username, usernameToMatch.Trim()), pageIndex, pageSize, out totalRecords);
		}

		public override System.Web.Security.MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
		{
			return MembershipUserUtility.FindUsersByPredicate(p => true == true, pageIndex, pageSize, out totalRecords);
		}

		public override int GetNumberOfUsersOnline()
		{
			using (var db = new DataAccess.CSSDataContext())
			{
				var logins = db.Logins.Where(p => p.FindCurrentSession() != null);

				return logins.Count();
			}
		}

		public override string GetPassword(string username, string answer)
		{
			throw new NotImplementedException();
		}

		public override System.Web.Security.MembershipUser GetUser(string username, bool userIsOnline)
		{
			return MembershipUserUtility.FindUserByPredicate(p =>
					((userIsOnline == true && p.FindCurrentSession() != null) || userIsOnline == false)
					&& p.Username == username.Trim());
		}

		public override System.Web.Security.MembershipUser GetUser(object providerUserKey, bool userIsOnline)
		{
			return MembershipUserUtility.FindUserByPredicate(p =>
					((userIsOnline == true && p.FindCurrentSession() != null) || userIsOnline == false)
					&& p.Id == Convert.ToInt32(providerUserKey));
		}

		public override string GetUserNameByEmail(string email)
		{
			MembershipUser user = MembershipUserUtility.FindUserByPredicate(p =>
					p.Email == email.Trim());

			if (user != null)
				return user.UserName;
			else
				return null;
		}

		public override int MaxInvalidPasswordAttempts
		{
			get { return Int32.MaxValue; }
		}

		public override int MinRequiredNonAlphanumericCharacters
		{
			get { return 0; }
		}

		public override int MinRequiredPasswordLength
		{
			get { return 6; }
		}

		public override int PasswordAttemptWindow
		{
			get { return 0; }
		}

		public override System.Web.Security.MembershipPasswordFormat PasswordFormat
		{
			get { return MembershipPasswordFormat.Hashed; }
		}

		public override string PasswordStrengthRegularExpression
		{
			get { return ".*"; }
		}

		public override bool RequiresQuestionAndAnswer
		{
			get { return false; }
		}

		public override bool RequiresUniqueEmail
		{
			get { return true; }
		}

		public override string ResetPassword(string username, string answer)
		{
			string newPassword = Membership.GeneratePassword(MinRequiredPasswordLength, MinRequiredNonAlphanumericCharacters);
			string hashedPassword = Allegiance.CommunitySecuritySystem.Common.Utility.Encryption.SHA256Hash(newPassword);

			using (var db = new DataAccess.CSSDataContext())
			{
				var login = db.Logins.FirstOrDefault(p => p.Username == username.Trim());
				login.Password = hashedPassword;
			
				db.SubmitChanges();
			}

			return newPassword;
		}

		public override bool UnlockUser(string userName)
		{
			bool result = false;

			using (var db = new DataAccess.CSSDataContext())
			{
				var login = db.Logins.FirstOrDefault(p => p.Username == userName.Trim());

				if (login.IsBanned == true)
				{
					foreach (var ban in login.Identity.Bans.Where(p => p.InEffect == true))
						ban.InEffect = false;

					result = true;

					db.SubmitChanges();
				}
			}

			return result;
		}

		public override void UpdateUser(System.Web.Security.MembershipUser user)
		{
			using (var db = new DataAccess.CSSDataContext())
			{
				var login = db.Logins.FirstOrDefault(p => p.Id == Convert.ToInt32(user.ProviderUserKey));

				if (login != null)
				{
					login.Email = user.Email;
					login.Username = user.UserName;

					db.SubmitChanges();
				}
			}
		}

		public override bool ValidateUser(string username, string password)
		{
			string hashedPassword = Allegiance.CommunitySecuritySystem.Common.Utility.Encryption.SHA256Hash(password);

			using (var db = new DataAccess.CSSDataContext())
			{
				var login = db.Logins.FirstOrDefault(p => p.Username == username.Trim());

				return login != null && login.Password == hashedPassword;
			}
		}
	}
}
