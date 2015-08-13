using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Linq.Expressions;
using System.Data.Linq.SqlClient;
using Allegiance.CommunitySecuritySystem.DataAccess.Enumerations;
using System.Net.Mail;
using Allegiance.CommunitySecuritySystem.Common.Utility;
using Allegiance.CommunitySecuritySystem.DataAccess.IPConvergeProvider;
using Allegiance.CommunitySecuritySystem.DataAccess.Properties;

namespace Allegiance.CommunitySecuritySystem.DataAccess.MembershipProviders
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
			using (var db = new DataAccess.CSSDataContext())
			{
				DataAccess.Login login;

				if (Settings.Default.UseIPConverge == true)
				{
					login = DataAccess.Login.FindLoginByUsernameOrCallsign(db, username);

					if (login == null)
						return false;

					var connect = new IPConvergeProvider.Connect();

					// TODO: If IP Converge is to be used ever, then working around IPC's MD5 password hashs will need to be done.
					//connect.ChangePassword(login.Email, newPasswordHash);
				}
				else
				{
					login = DataAccess.Login.FindLoginByUsernameOrCallsign(db, username);

					if (login == null)
						return false;

					if (PasswordHash.ValidatePassword(oldPassword, login.Password) == false)
						return false;
				}

				login.Password = PasswordHash.CreateHash(newPassword);
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
			DataAccess.Identity identity = null;

			var connect = new Connect();

			using (var db = new DataAccess.CSSDataContext())
			{
				if (DataAccess.Login.FindLoginByUsernameOrCallsign(db, username) != null)
				{
					status = System.Web.Security.MembershipCreateStatus.DuplicateUserName;
					return null;
				}

				if (DataAccess.Alias.ListAliases(db, username).Count > 0)
				{
					status = System.Web.Security.MembershipCreateStatus.UserRejected;
					return null;
				}

				if (Settings.Default.UseIPConverge == true)
				{
					if (connect.CheckEmail(email) == false)
					{
						status = MembershipCreateStatus.DuplicateEmail;
						return null;
					}

					if (connect.CheckUsername(username) == false)
					{
						status = MembershipCreateStatus.DuplicateUserName;
						return null;
					}
				}

				status = DataAccess.Identity.TryCreateIdentity(db, username, password, email, out identity);

				if (status == MembershipCreateStatus.Success)
				{
					if (Settings.Default.UseIPConverge == true)
					{
						string ipAddress = "127.0.0.1";
						if (HttpContext.Current != null)
							ipAddress = HttpContext.Current.Request.UserHostAddress;

						// TODO: If IP Converge is to be used ever, then working around IPC's MD5 password hashs will need to be done.
						//if (connect.AddMember(email, username, passwordHash, ipAddress) == false)
						//{
						//    status = MembershipCreateStatus.ProviderError;
						//    return null;
						//}
					}
				}

				db.SubmitChanges();

				if(identity != null)
				{
					DataAccess.Login createdLogin = DataAccess.Login.FindLoginByUsernameOrCallsign(db, username);

					if(createdLogin != null)
					{
						status = System.Web.Security.MembershipCreateStatus.Success;
						var memebershipUser = MembershipUserUtility.CreateMembershipUserFromLogin(createdLogin);

						if (memebershipUser != null)
						{
							SendWelcomeEmail(memebershipUser);
						}

						return memebershipUser;
					}
				}
			}

			status = System.Web.Security.MembershipCreateStatus.ProviderError;
			return null;
		}

		private void SendWelcomeEmail(MembershipUser membershipUser)
		{
			MailMessage mail = new MailMessage();

			mail.To.Add(new MailAddress(membershipUser.Email));

			mail.IsBodyHtml = true;

			mail.Subject = "Welcome " + membershipUser.UserName + " to www.FreeAllegiance.org!";

			mail.Body = String.Format(@"
<html>
	<body>
		<h3>Welcome to FreeAllegiance.org</h3>
		<hr />
		<p>
			Thank you for joining www.FreeAllegiance.org. We look forward to seeing you in game!<br />
			<br />
			Account Name: <b>{0}</b><br />
			<br />
			<i>- Please note, your game account will not work on with our forum software. You will need to create a seperate forum account to post and search on our forums.</i>
		</p>
		<p>
			Need more information? We're here to help!
			<ul>
				<li><a href=""http://www.freeallegiance.org"">Free Allegiance Home Page</a> - Game client and downloads.</li>
				<li><a href=""http://www.freeallegiance.org/FAW/index.php/Main_Page"">Free Allegiance Wiki</a> - Everything you need to know.</li>
				<li><a href=""http://www.freeallegiance.org/forums/index.php"">Free Allegiance Forums</a> - Questions, answers and community.</li>
			</ul>
		</p>
		<p>
			Thanks for stopping by, see you in game!<br />
			- The FreeAllegiance staff.
		</p>
	</body>
</html>
", membershipUser.UserName);

			MailManager.SendMailMessage(mail);
		}

		public override bool DeleteUser(string username, bool deleteAllRelatedData)
		{
			using (var db = new DataAccess.CSSDataContext())
			{
				DataAccess.Login login = DataAccess.Login.FindLoginByUsernameOrCallsign(db, username);

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
			// TODO: (Optional) if userIsOnline is true, then the user's last action timestamp may be updated.
			// the current db schema does not support this column.

			MembershipUser returnValue = MembershipUserUtility.FindUserByPredicate(p => p.Username == username);

			if (returnValue == null)
			{
				using (CSSDataContext db = new CSSDataContext())
				{
					var login = Login.FindLoginByUsernameOrCallsign(db, username);

					if(login != null)
						returnValue = MembershipUserUtility.FindUserByPredicate(p => p.Username == login.Username);
				}
			}

			return returnValue;
		}

		public override System.Web.Security.MembershipUser GetUser(object providerUserKey, bool userIsOnline)
		{
			// TODO: (Optional) if userIsOnline is true, then the user's last action timestamp may be updated.
			// the current db schema does not support this column.
			return MembershipUserUtility.FindUserByPredicate(p => p.Id == Convert.ToInt32(providerUserKey));
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
			string hashedPassword = PasswordHash.CreateHash(newPassword);

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
			using (var db = new DataAccess.CSSDataContext())
			{
				var login = db.Logins.FirstOrDefault(p => p.Username == username.Trim());

				if (login == null)
				{
					var alias = DataAccess.Alias.GetAliasByCallsign(db, username);

					if (alias != null)
						login = alias.Login;
				}

				if (login == null)
					return false;

				if (Settings.Default.UseIPConverge == true)
				{
					var connect = new IPConvergeProvider.Connect();

					AuthenticationStatus authenticationStatus;
					string email;

					connect.Authenticate(login.Username, password, out authenticationStatus, out email);

					// Always update the user's email to the IPBoard email if the CSS email is different.
					// This way if the user uses the forgot password features, then the email will go to 
					// their forum email which is the system of record.
					if (login.Email != email)
					{
						login.Email = email;
						db.SubmitChanges();
					}

					return authenticationStatus == AuthenticationStatus.Success;
				}
				else
				{
					try
					{
						// Supports calling this provider from both the CSS Server service and the web interface.
						return login != null && (PasswordHash.ValidatePassword(password, login.Password) == true || login.Password == password);
					}
					catch(FormatException)
					{
						Log.Write(LogType.AuthenticationServer, "LoginId: " + login.Id + ", loginName: " +  login.Username + ", Legacy password couldn't be decoded. This is normal for a beta account.");
						return false;
					}
				}
			}
		}
	}
}
