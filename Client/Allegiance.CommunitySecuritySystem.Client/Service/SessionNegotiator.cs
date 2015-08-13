using System;
using Allegiance.CommunitySecuritySystem.Client.ClientService;
using Allegiance.CommunitySecuritySystem.Client.Utility;

namespace Allegiance.CommunitySecuritySystem.Client.Service
{
    class SessionNegotiator
    {
        /// <summary>
        /// Perform autoupdate, validate client, negotiate for new session
        /// </summary>
        public static void Login(string aliasInput, string lobbyName, TaskDelegate onCompleteDelegate)
        {
            TaskHandler.RunTask(delegate(object input) {

                DebugDetector.AssertCheckRunning();

                var parameters  = input as object[];
                var signal      = parameters[0] as TaskDelegate;
                var alias       = parameters[1] as string;
                var message     = string.Empty;
                var ticket      = string.Empty;
				var rank		= 0;
                CheckInStatus status = CheckInStatus.InvalidCredentials;
			
                try
                {
                    //Get available lobbies
                    MainForm.SetStatusBar("Checking lobby status...", 25);
                    var lobbies = ServiceHandler.Service.CheckAvailableLobbies();

                    //Get lobby
                    var lobby   = GetLobbyByName(lobbies, lobbyName);

                    //Check auto-update - This is done by the play control/update check control now.
                    //MainForm.SetStatusBar("Checking for updates...", 25);

					//PendingUpdates pendingUpdates = AutoUpdate.GetPendingUpdateQueues(ServiceHandler.Service);

					//if (AutoUpdate.ProcessPendingUpdates(pendingUpdates, lobby, new AutoUpdate.AutoupdateProgressCallback(AutoupdateProgressUpdate)))
					//    return;

                    //Log in, Display 'logging in' status to user
                    MainForm.SetStatusBar("Logging in...", 50);

                    var loginResult = ServiceHandler.Service.Login(new LoginData()
                    {
                        Alias               = alias,
                        LobbyId             = lobby.LobbyId,
                        LobbyIdSpecified    = true
                    });

                    if (loginResult.StatusSpecified)
                    {
						rank = loginResult.Rank;

                        switch (loginResult.Status)
                        {
                            case LoginStatus.Authenticated:
                            
                                //Perform initial check in
                                MainForm.SetStatusBar("Validating Client...", 75);
								status = ValidateLogin(loginResult.BlackboxData, ref message, ref ticket);

								// Relog in after linking to pick up any alias changes.
								if (status == CheckInStatus.AccountLinked)
								{
									loginResult = ServiceHandler.Service.Login(new LoginData()
									{
										Alias = alias,
										LobbyId = lobby.LobbyId,
										LobbyIdSpecified = true
									});
								}

								//Set up response
								alias = loginResult.AcceptedAlias;

								//if (loginResult.Rank <= 5)
								//    alias += "(" + loginResult.Rank.ToString() + ")";

                                break;

                            case LoginStatus.AccountLocked:
                                message = "Account Locked";
                                break;

                            case LoginStatus.InvalidCredentials:
                                message = "Username or password was incorrect";
                                break;

                            case LoginStatus.PermissionDenied:
                                message = "Permission was denied to this lobby";
                                break;
					
                        }
                    }
                    else
                    {
                        message = "An unknown error occurred";
                    }

                    Log.Write(message);
                }
                catch (Exception error)
                {
                    message = "An error occurred";

                    Log.Write(error);
                }

                //Signal the calling thread
				signal(new object[] { status, message, alias, ticket, rank });

            }, onCompleteDelegate, aliasInput);
        }

		private static void AutoupdateProgressUpdate(string lobbyName, string message, int completionPercentage)
		{
			MainForm.SetStatusBar(message, completionPercentage);
		}

        /// <summary>
        /// Logout from the server, ending the current session
        /// </summary>
        public static void Logout(bool block)
        {
            if (!block)
            {
                TaskHandler.RunTask(delegate()
                {
                    ServiceHandler.Service.Logout(new AuthenticatedData());
                });
            }
            else
                ServiceHandler.Service.Logout(new AuthenticatedData());
        }

		public static CheckInStatus ValidateLogin(byte[] blackboxData, ref string message, ref string ticket)
        {
            var loader  = new AssemblyLoader();
            var results = loader.ValidateEntryAssembly(blackboxData);

			CheckInStatus status = CheckIn(results, ref message, ref ticket);

			if (status == CheckInStatus.Ok)
                message = "Login Succeeded";

			return status;
        }

        /// <summary>
        /// Perform a check-in with the server
        /// </summary>
		public static CheckInStatus CheckIn(byte[] encryptedData, ref string message, ref string ticket)
        {
            var checkInResult = ServiceHandler.Service.CheckIn(new CheckInData()
            {
                SessionIdSpecified  = false,
                EncryptedData       = encryptedData,
            });

            if (checkInResult.StatusSpecified)
            {
                switch (checkInResult.Status)
                {
                    case CheckInStatus.Ok:
                        message = "Check-in Succeeded";
                        ticket  = checkInResult.Ticket;
						break;

                    case CheckInStatus.InvalidCredentials:
                        message = "Invalid Credentials";
						break;

                    case CheckInStatus.InvalidHash:
                        message = "Invalid Client";
						break;

                    case CheckInStatus.Timeout:
                        message = "Client Timeout";
						break;

					case CheckInStatus.VirtualMachineBlocked:
						message = "Virtual Machine Blocked";
						break;

					case CheckInStatus.AccountLinked:
						message = "Account Linked";
						break;

					case CheckInStatus.AccountLocked:
						message = "Account Locked";
						break;

					case CheckInStatus.PermissionDenied:
						message = "Permission Denied";
						break;
                }

				return checkInResult.Status;
            }

			// This shouldn't happen.
			message = "No Checkin Status Available.";
			return CheckInStatus.Timeout;
        }

        /// <summary>
        /// Generates check-in data from Blackbox.
        /// </summary>
        /// <returns></returns>
        public static byte[] GenerateCheckInData()
        {
            var loader = new AssemblyLoader();
            return loader.ValidateEntryAssembly(null);
        }

        private static LobbyResult GetLobbyByName(LobbyResult[] results, string lobbyName)
        {
            foreach (var l in results)
            {
                if (l.Name == lobbyName)
                    return l;
            }
            throw new Exception("Could not find specified lobby: " + lobbyName);
        }
    }
}