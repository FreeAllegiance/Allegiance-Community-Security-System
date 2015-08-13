using System;
using System.Diagnostics;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Allegiance.CommunitySecuritySystem.Client.Utility;
using Allegiance.CommunitySecuritySystem.Client.Integration;
using Allegiance.CommunitySecuritySystem.Client.ClientService;

namespace Allegiance.CommunitySecuritySystem.Client.Service
{
    internal class ServiceHandler
    {
        #region Fields

        private const int _timeout = 300000;

        #endregion

        #region Properties

		// BT - Can't make this a singleton. Callback handlers will stack up on the service reference, and cause issues. 
		public static ClientService.ClientService Service
        {
            get
            {
				ClientService.ClientService service;

				service = Initialize();

				return service;
            }
        }

        #endregion

        #region Delegates

		public delegate void LauncherSignInCompleteDelegate(LauncherSignInResult launcherSignInResult);

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the ClientService
		/// </summary>
#if !DEBUG
        [DebuggerStepThrough]
#endif
		private static ClientService.ClientService Initialize()
        {
            DebugDetector.AssertCheckRunning();

            //For testing, we're accepting untrusted server certificates
            ServicePointManager.ServerCertificateValidationCallback
                = new RemoteCertificateValidationCallback(delegate(object sender, 
                    X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            });

			ClientService.ClientService service = new ClientService.ClientService();
			service.Timeout = _timeout;
			service.Url = AllegianceRegistry.ClientService;

			return service;
        }

        /// <summary>
        /// Checks to see if the user's credentials are stored from a previous session
        /// </summary>
        public static bool CheckLogin()
        {
            //Initialize LoginData
            return AuthenticatedData.CheckLoginData();
        }

		public static void LauncherSignIn(LauncherSignInCompleteDelegate finished)
		{
			LauncherSignIn(null, null, finished);
		}

        /// <summary>
        /// Verifies that the input username and password are correct
        /// </summary>
        public static void LauncherSignIn(string username, string password, LauncherSignInCompleteDelegate finished)
        {
			bool updatePersistedCredentials = String.IsNullOrEmpty(username) == false && String.IsNullOrEmpty(password) == false;

			if (updatePersistedCredentials == true)
			{
				AuthenticatedData.SetLogin(username, password);
			}

            TaskHandler.RunTask(delegate() {

               LauncherSignInResult result = new LauncherSignInResult();

                try
                {
					LoginResult blackBoxData = Service.GetBlackBoxForUser(new LoginData()
					{
						Alias = AuthenticatedData.PersistedUser,
						LobbyId = null,
						LobbyIdSpecified = false,
						Password = AuthenticatedData.PersistedPass,
						SessionId = 0, 
						SessionIdSpecified = false,
						Username = AuthenticatedData.PersistedUser
					});

					if (blackBoxData.Status == LoginStatus.Authenticated)
					{

						Log.Write("ServiceHandler::LauncherSignIn() - Data received for: " + AuthenticatedData.PersistedUser + ", data length: " + blackBoxData.BlackboxData.Length);

						string message = null;
						string ticket = null;
						var checkInStatus = SessionNegotiator.ValidateLogin(blackBoxData.BlackboxData, ref message, ref ticket);

						if (checkInStatus != CheckInStatus.Ok && checkInStatus != CheckInStatus.AccountLinked)
						{
							result.Status = checkInStatus;
							result.StatusSpecified = true;
							result.LinkedAccount = String.Empty;
						}
						else
						{
							// Attempt to verify the login credentials
							result = Service.LauncherSignIn(new LauncherSignInData());
						}
					}
					else
					{
						switch (blackBoxData.Status)
						{
							case LoginStatus.AccountLocked:
								result.Status = CheckInStatus.AccountLocked;
								break;

							case LoginStatus.InvalidCredentials:
								result.Status = CheckInStatus.InvalidCredentials;
								break;

							case LoginStatus.PermissionDenied:
								result.Status = CheckInStatus.PermissionDenied;
								break;

							default:
								throw new NotSupportedException(blackBoxData.Status.ToString());
						}
						
						result.StatusSpecified = true;
						result.LinkedAccount = String.Empty;
					}

					
                }
                catch(Exception error)
                {
                    result = new LauncherSignInResult() 
					{
						Status = CheckInStatus.Timeout,
						StatusSpecified = false
					};

                    Log.Write(error);
                }

				if (result.StatusSpecified == true && result.Status == CheckInStatus.Ok)
				{
					DataStore.Callsigns = null;

					if (updatePersistedCredentials == true)
						AuthenticatedData.PersistLogin(username, AuthenticatedData.PersistedPass);
				}

                finished(result);

            });
        }

		

        #endregion
    }
}