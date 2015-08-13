using System;
using System.Collections.Generic;
using System.Text;
using Allegiance.CommunitySecuritySystem.Client.Utility;

namespace Allegiance.CommunitySecuritySystem.Client.ClientService
{
    public partial class AuthenticatedData
    {
        public static string PersistedUser { get; set; }
        public static string PersistedPass { get; set; }

        /// <summary>
        /// Sets the credentials to be used for this session
        /// </summary>
        public static void SetLogin(string username, string password)
        {
            PersistedUser = username;
            PersistedPass = password;
        }

        /// <summary>
        /// Persists credentials to the datastore
        /// </summary>
        public static void PersistLogin(string username, string password)
        {
            //Store username & password to DataStore
            DataStore.Username = username;
            DataStore.Password = password;

            DataStore.Instance.Save();
        }

        /// <summary>
        /// Checks if credentials exist in the data store. If they are
		/// then the username and password are set into the static variable for all instances 
		/// of the AuthenticatedData object.
        /// </summary>
        public static bool CheckLoginData()
        {
            var username = DataStore.Username;
            var password = DataStore.Password;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                SetLogin(username, password);
                return true;
            }

            return false;
        }

        public AuthenticatedData()
        {
            this.Username = PersistedUser;
            this.Password = PersistedPass;
        }
    }
}