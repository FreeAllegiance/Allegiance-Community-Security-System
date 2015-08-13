using System;
using System.Collections.Generic;
using System.Threading;
using Allegiance.CommunitySecuritySystem.Client.ClientService;
using Allegiance.CommunitySecuritySystem.Client.Utility;

namespace Allegiance.CommunitySecuritySystem.Client.Service
{
    [Serializable]
    public class Callsign
    {
        #region Properties

        public bool Default { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }

		public int Id { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Retrieve callsigns from server if callsigns are not stored locally
        /// </summary>
        /// <param name="onCompleteDelegate"></param>
		public delegate void RetrieveCallsignsCompleteDelegate(List<Callsign> callsigns, int AvailableAliasCount);
		public static void RetrieveCallsigns(RetrieveCallsignsCompleteDelegate onCompleteDelegate, bool reloadFromServer)
        {
            //Check the datastore to see if callsigns have already been loaded
            var callsigns = DataStore.Callsigns;

			if (callsigns == null || reloadFromServer == true)
			{
				callsigns = new List<Callsign>();

				//If not, retrieve the callsign list from the server
				TaskHandler.RunTask(delegate(object data)
				{
					var parameters = data as object[];
					var signal = parameters[0] as RetrieveCallsignsCompleteDelegate;
					var list = parameters[1] as List<Callsign>;
					int availableAliasCount = (int) parameters[2];

					try
					{
						ListAliasesResult aliases = ServiceHandler.Service.ListAliases(new AuthenticatedData());

						if (aliases == null)
						{
							signal(new List<Callsign>(), 0);
							return;
						}

						list = new List<Callsign>();
						bool foundLastAliasMatch = false;
						foreach (var alias in aliases.Aliases)
						{
							list.Add(new Callsign()
							{
								Default = alias.IsDefault,
								Name = alias.Callsign,
								Active = alias.IsActive,
								Id = alias.Id
							});

							if (DataStore.LastAlias == alias.Callsign)
								foundLastAliasMatch = true;
						}

						if (foundLastAliasMatch == false || DataStore.LastAlias == null)
						{
							if (list.Count > 0)
								DataStore.LastAlias = list[0].Name;
							else
								DataStore.LastAlias = null;
						}

						DataStore.AvailableAliasCount = aliases.AvailableAliasCount;

						//Store callsign list to datastore
						DataStore.Callsigns = list;
						DataStore.Instance.Save();
					}
					catch (Exception error)
					{
						Log.Write(error);
					}

					//Signal to the calling thread that the operation is complete
					signal(DataStore.Callsigns, DataStore.AvailableAliasCount);

				}, onCompleteDelegate, DataStore.Callsigns, DataStore.AvailableAliasCount);
			}
			else
			{
				onCompleteDelegate(DataStore.Callsigns, DataStore.AvailableAliasCount);
			}
        }

		// This is different from the alias check as it's used in account creation before the user
		// as AuthenticatedData to talk to the web service with.
		public static Thread CheckUsernameAvailability(string username, TaskDelegate onCompleteDelegate)
		{
			if (string.IsNullOrEmpty(username))
			{
				onCompleteDelegate(CheckAliasResult.Unavailable);
				return null;
			}

			return TaskHandler.RunTask(delegate(object data)
			{
				var parameters = data as object[];
				var signal = parameters[0] as TaskDelegate;
				var targetUsername = parameters[1] as string;

				bool specified;
				CheckAliasResult result;

				try
				{
					ServiceHandler.Service.IsAliasAvailable(targetUsername, out result, out specified);
				}
				catch (Exception error)
				{
					specified = false;
					result = CheckAliasResult.Unavailable;

					Log.Write(error);
				}

				//Signal the calling threat that the operation is compete
				if (specified)
					onCompleteDelegate(result);
				else
					onCompleteDelegate(CheckAliasResult.Unavailable);

			}, onCompleteDelegate, username);
		}

        public static Thread CheckAvailability(string desiredCallsign, string asgsPassword, bool canCreate, TaskDelegate onCompleteDelegate)
        {
            if(string.IsNullOrEmpty(desiredCallsign))
            {
                onCompleteDelegate(CheckAliasResult.Unavailable);
                return null;
            }

            return TaskHandler.RunTask(delegate(object data)
            {
                var parameters  = data as object[];
                var signal      = parameters[0] as TaskDelegate;
                var callsign    = parameters[1] as string;
                var create      = (bool)parameters[2];

                bool specified;
                CheckAliasResult result;

                try
                {
					result = ValidateLegacyCallsignUsage(desiredCallsign, asgsPassword);
					if (result != CheckAliasResult.Available)
					{
						specified = true;
					}
					else
					{
						if (create)
							ServiceHandler.Service.CreateAlias(new LoginData() { Alias = callsign, LegacyPassword = asgsPassword }, out result, out specified);
						else
							ServiceHandler.Service.CheckAlias(new LoginData() { Alias = callsign, LegacyPassword = asgsPassword }, out result, out specified);
					}
                }
                catch (Exception error)
                {
                    specified   = false;
                    result      = CheckAliasResult.Unavailable;

                    Log.Write(error);
                }

                //Signal the calling threat that the operation is compete
                if (specified)
                    onCompleteDelegate(result);
                else
                    onCompleteDelegate(CheckAliasResult.Unavailable);

            }, onCompleteDelegate, desiredCallsign, canCreate);
        }

		public static void SetDefaultAlias(Callsign alias)
		{
			DataStore.LastAlias = alias.Name;
			DataStore.Instance.Save();

			ClientService.ClientService clientService = ServiceHandler.Service;

			SetDefaultAliasData setDefaultAliasData = new SetDefaultAliasData()
			{
				 AliasId = alias.Id,
				 AliasIdSpecified = true
			};
			bool setDefaultAliasResult;
			bool setDefaultAliasResuleSpecified;

			clientService.SetDefaultAlias(setDefaultAliasData, out setDefaultAliasResult, out setDefaultAliasResuleSpecified);
		}

		public static ClientService.CheckAliasResult ValidateLegacyCallsignUsage(string callsign, string legacyPassword)
		{
			ClientService.ClientService clientService = ServiceHandler.Service;

			ClientService.CheckAliasResult checkAliasResult;
			bool checkAliasResultSpecified;
			clientService.ValidateLegacyCallsignUsage(callsign, legacyPassword, out checkAliasResult, out checkAliasResultSpecified);

			return checkAliasResult;
		}

        public override string ToString()
        {
            return this.Name;
        }

        #endregion
    }
}