using System;
using System.Collections.Generic;
using Allegiance.CommunitySecuritySystem.BlackboxGenerator;
using Allegiance.CommunitySecuritySystem.Common.Enumerations;
using Allegiance.CommunitySecuritySystem.DataAccess;
using Allegiance.CommunitySecuritySystem.DataAccess.Enumerations;
using Allegiance.CommunitySecuritySystem.Server.Contracts;
using Allegiance.CommunitySecuritySystem.Server.Interfaces;
using System.Configuration;
using System.IO;
using System.Drawing.Imaging;
using System.Web;
using System.ServiceModel.Activation;
using System.Linq;

namespace Allegiance.CommunitySecuritySystem.Server
{
	[AspNetCompatibilityRequirements(RequirementsMode=AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class ClientService : IClientService
    {
        #region Contracts

		public LoginResult GetBlackBoxForUser(LoginData loginData)
		{
			try
			{
				bool useDebugBlackbox = Boolean.Parse(ConfigurationManager.AppSettings["UseDebugBlackbox"]);

				return loginData.Verify(useDebugBlackbox);
			}
			catch (Exception error)
			{
				Error.Write(error);
				throw;
			}
		}

		public LauncherSignInResult LauncherSignIn(LauncherSignInData data)
        {
            try
            {
				LauncherSignInResult result = new LauncherSignInResult();

				string loginUserName;

				bool succeeded = data.Authenticate(out loginUserName);

				if (succeeded == true)
				{
					result.Status = CheckInStatus.Ok;
					result.LoginUserName = loginUserName;

					using (CSSDataContext db = new CSSDataContext())
					{
						var login = DataAccess.Login.FindLoginByUsernameOrCallsign(db, data.Username);

						if (login == null)
						{
							result.Status = CheckInStatus.InvalidCredentials;
						}
						else if (Identity.IsLinkedToAnOlderAccount(db, data.Username) == true)
						{
							result.LinkedAccount = Identity.GetOldestLinkedAcccountUsername(db, data.Username);
							result.Status = CheckInStatus.AccountLinked;
						}
						else
						{
							result.LoginID = login.Id;
						}
					}
				}
				else
				{
					using (CSSDataContext db = new CSSDataContext())
					{
						var login = DataAccess.Login.FindLoginByUsernameOrCallsign(db, data.Username);

						if (login == null)
							result.Status = CheckInStatus.InvalidCredentials;
						else if (login.IsBanned == true)
							result.Status = CheckInStatus.AccountLocked;
						else
							result.Status = CheckInStatus.InvalidCredentials;
					}
					
				}

				return result;
            }
            catch (Exception error)
            {
                Error.Write(error);
                throw;
            }
        }

        public LoginResult Login(LoginData data)
        {
            try
            {
				bool useDebugBlackbox = Boolean.Parse(ConfigurationManager.AppSettings["UseDebugBlackbox"]);

				return data.Verify(useDebugBlackbox);
            }
            catch (Exception error)
            {
                Error.Write(error);
                throw;
            }
        }

        public CheckInResult CheckIn(CheckInData data)
        {
            try
            {
                if (!data.Authenticate())
                    return new CheckInResult() { Status = CheckInStatus.InvalidCredentials };

                return data.Verify();
            }
            catch (Exception error)
            {
                Error.Write(error);
                throw;
            }
        }

        public void Logout(AuthenticatedData data)
        {
            try
            {
                if (!data.Authenticate())
                    return;

                Validation.EndSession(data.Username, data.Password);
            }
            catch (Exception error)
            {
                Error.Write(error);
                throw;
            }
        }

        public CheckAliasResult CreateAlias(LoginData data)
        {
            try
            {
                if (!data.Authenticate())
                    return CheckAliasResult.InvalidLogin;

				//if (data.CheckCaptcha() == false)
				//	return CheckAliasResult.CaptchaFailed;

                return Validation.CreateAlias(data.Username, data.Password, data.Alias, data.LegacyPassword);
            }
            catch (Exception error)
            {
                Error.Write(error);
                throw;
            }
        }

		public CheckAliasResult IsAliasAvailable(string callsignWithTags)
		{
			// Prevent slamming on this call.
			System.Threading.Thread.Sleep(500);

			try
			{
				return Validation.ValidateAlias(callsignWithTags);
			}
			catch (Exception error)
			{
				Error.Write(error);
				throw;
			}

		}

		public bool CheckLegacyAliasExists(string callsignWithTags)
		{
			// Prevent slamming on this call.
			System.Threading.Thread.Sleep(500);

			using (CSSDataContext db = new CSSDataContext())
			{
				string callsign = Alias.GetCallsignFromStringWithTokensAndTags(db, callsignWithTags);
				return Alias.CheckLegacyAliasExists(callsign);
			}
		}

		public CheckAliasResult ValidateLegacyCallsignUsage(string callsignWithTags, string legacyPassword)
		{
			// Prevent slamming on this call.
			System.Threading.Thread.Sleep(500);

			using (CSSDataContext db = new CSSDataContext())
			{
				string callsign = Alias.GetCallsignFromStringWithTokensAndTags(db, callsignWithTags);
				return Alias.ValidateLegacyCallsignUsage(callsign, legacyPassword);
			}
		}

        public CheckAliasResult CheckAlias(LoginData data)
        {
            try
            {
                if (!data.Authenticate())
                    return CheckAliasResult.InvalidLogin;

                return Validation.ValidateAlias(data.Username, data.Password, data.Alias);
            }
            catch (Exception error)
            {
                Error.Write(error);
                throw;
            }
        }

		public ListAliasesResult ListAliases(AuthenticatedData data)
        {
            try
            {
                if (!data.Authenticate())
                    return null;

				ListAliasesResult result = new ListAliasesResult();

				using (var db = new CSSDataContext())
				{
					result.Aliases = Alias.ListDecoratedAliases(db, data.Username, true);

					result.AvailableAliasCount = Alias.GetAliasCount(db, data.Username);
				}

				return result;
            }
            catch (Exception error)
            {
                Error.Write(error);
                throw;
            }
        }

		public bool SetDefaultAlias(SetDefaultAliasData data)
		{
			try
			{
				if (!data.Authenticate())
					return false;

				return data.SetDefaultAlias();
			}
			catch (Exception error)
			{
				Error.Write(error);
				throw;
			}
		}


		public CaptchaResult GetCaptcha(int width, int height)
		{
			Guid captchaToken;
			string captchaAnswer;
			string requestorIpAddress = String.Empty;

			if (HttpContext.Current != null)
				requestorIpAddress = HttpContext.Current.Request.UserHostAddress;

			DataAccess.Captcha.GetNewCaptchaAnswer(requestorIpAddress, out captchaToken, out captchaAnswer);

			Common.Utility.CaptchaImage captchaImage = new Common.Utility.CaptchaImage(captchaAnswer, width, height);

			MemoryStream memoryStream = new MemoryStream();
			captchaImage.Image.Save(memoryStream, ImageFormat.Jpeg);

			CaptchaResult returnValue = new CaptchaResult()
			{
				CaptchaImage = memoryStream.GetBuffer(),
				CaptchaToken = captchaToken 
			};

			return returnValue;
		}

		public CreateLoginResult CreateLogin(string username, string password, string email, Guid captchaToken, string captchaAnswer)
		{
			System.Web.Security.MembershipCreateStatus status;

			// Prevent DDOS from slamming the server on this method. 
			System.Threading.Thread.Sleep(500);

			try
			{
				if (DataAccess.Captcha.CheckCaptcha(captchaToken, captchaAnswer) == false)
				{
					status = System.Web.Security.MembershipCreateStatus.InvalidAnswer;
				}
				else
				{
					DataAccess.MembershipProviders.CssMembershipProvider membershipProvider = new DataAccess.MembershipProviders.CssMembershipProvider();

					membershipProvider.CreateUser(username, password, email, String.Empty, Guid.NewGuid().ToString(), true, null, out status);
				}

				return new CreateLoginResult()
				{
					MembershipCreateStatus = status
				};
			}
			catch (Exception ex)
			{
				Log.Write(LogType.AuthenticationServer, ex.ToString());

				return new CreateLoginResult()
				{
					MembershipCreateStatus = System.Web.Security.MembershipCreateStatus.ProviderError
				};
			}
		}

		public GetUsernameFromCallsignOrUsernameResult GetUsernameFromCallsignOrUsername(string usernameOrCallsign)
		{
			GetUsernameFromCallsignOrUsernameResult result = new GetUsernameFromCallsignOrUsernameResult();

			using (var db = new CSSDataContext())
			{
				var login = DataAccess.Login.FindLoginByUsernameOrCallsign(db, usernameOrCallsign);

				if(login != null)
					result.Username = login.Username;
			}

			return result;
		}

		public GetBannedUsernamesAfterTimestampResult GetBannedUsernamesAfterTimestamp(long unixTimeStamp)
		{
			DateTime currentTime = DateTime.Now;
			string bannedUserList = String.Empty;

			if (unixTimeStamp > 0)
			{
				DateTime timestamp = new DateTime(unixTimeStamp);

				List<String> bannedUserNames = Ban.GetBanedUsernamesSinceTimestamp(timestamp);

				foreach (string bannedUserName in bannedUserNames)
				{
					if (bannedUserList.Length > 0)
						bannedUserList += "|";

					bannedUserList += bannedUserName;
				}
			}

			GetBannedUsernamesAfterTimestampResult result = new GetBannedUsernamesAfterTimestampResult();
			result.BannedUserNames = bannedUserList;
			result.CurrentTimestamp = currentTime.Ticks;

			return result;
		}

        #endregion
	}
}
