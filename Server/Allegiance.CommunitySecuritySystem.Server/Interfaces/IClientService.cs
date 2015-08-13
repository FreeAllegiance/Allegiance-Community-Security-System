using System;
using System.Collections.Generic;
using System.ServiceModel;
using Allegiance.CommunitySecuritySystem.DataAccess;
using Allegiance.CommunitySecuritySystem.DataAccess.Enumerations;
using Allegiance.CommunitySecuritySystem.Server.Contracts;
using Allegiance.CommunitySecuritySystem.DataAccess.Model;

namespace Allegiance.CommunitySecuritySystem.Server.Interfaces
{
    [ServiceContract]
    public interface IClientService
    {
        #region Authenticator Operations

		[OperationContract]
		LoginResult GetBlackBoxForUser(LoginData loginData);

        [OperationContract]
		LauncherSignInResult LauncherSignIn(LauncherSignInData data);

        [OperationContract]
        LoginResult Login(LoginData data);

        [OperationContract]
        CheckInResult CheckIn(CheckInData data);

        [OperationContract]
        void Logout(AuthenticatedData data);

        [OperationContract]
        CheckAliasResult CreateAlias(LoginData data);

        [OperationContract]
        CheckAliasResult CheckAlias(LoginData data);

		[OperationContract]
		bool CheckLegacyAliasExists(string callsignWithTags);

		[OperationContract]
		CheckAliasResult ValidateLegacyCallsignUsage(string callsignWithTags, string password);

		[OperationContract]
		CheckAliasResult IsAliasAvailable(string callsignWithTags);

		[OperationContract]
		bool SetDefaultAlias(SetDefaultAliasData data);

        [OperationContract]
        ListAliasesResult ListAliases(AuthenticatedData data);

		[OperationContract]
		CaptchaResult GetCaptcha(int width, int height);

		[OperationContract]
		CreateLoginResult CreateLogin(string username, string password, string email, Guid captchaToken, string captchaAnswer);

		[OperationContract]
		GetUsernameFromCallsignOrUsernameResult GetUsernameFromCallsignOrUsername(string callsignOrUsername);

		[OperationContract]
		GetBannedUsernamesAfterTimestampResult GetBannedUsernamesAfterTimestamp(long unixTimeStamp);

        #endregion

        #region AutoUpdate Operations

        [OperationContract]
        AutoUpdateResult CheckForUpdates(int lobbyId);

        [OperationContract]
        List<LobbyResult> CheckAvailableLobbies();

        #endregion

        #region Messaging Operations

        [OperationContract]
        ListMessageResult ListMessages(AuthenticatedData data);

        #endregion

        #region Polling Operations

        [OperationContract]
        List<Poll> ListPolls(AuthenticatedData data);

        [OperationContract]
        void ApplyVote(PollData data);

        #endregion
    }
}