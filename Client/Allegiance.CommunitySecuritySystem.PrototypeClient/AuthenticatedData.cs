using System;
using System.Collections.Generic;
using System.Text;

namespace Allegiance.CommunitySecuritySystem.PrototypeClient.localhost
{
    public partial class AuthenticatedData
    {
        public static string PersistedUser { get; set; }
        public static string PersistedPass { get; set; }

        public static void SetLogin(string username, string password)
        {
            PersistedUser = username;
            PersistedPass = password;
        }

        public AuthenticatedData()
        {
            this.Username = PersistedUser;
            this.Password = PersistedPass;
        }
    }
}