using System;
using System.Collections.Generic;
using System.Text;
using Allegiance.CommunitySecuritySystem.Client.Service;
using Allegiance.CommunitySecuritySystem.Client.ClientService;

namespace Allegiance.CommunitySecuritySystem.Client.Utility
{
    public partial class DataStore
    {
        #region Fields

        private static DataStore _instance = null;

        #endregion

        #region Properties

        public static DataStore Instance
        {
            get
            {
                if (_instance == null)
                    _instance = DataStore.Open("configuration.ds", "89e!as^3hep!upKhu");
                return _instance;
            }
        }

        public static List<Callsign> Callsigns
        {
            get { return Instance.LoadNode<List<Callsign>>("Callsigns"); }
            set { Instance["Callsigns"] = value; }
        }

        public static string LastAlias
        {
            get { return Instance.LoadNode<string>("LastAlias"); }
            set { Instance["LastAlias"] = value; }
        }

        public static string Username
        {
            get { return Instance.LoadNode<string>("Username"); }
            set { Instance["Username"] = value; }
        }

        public static string Password
        {
            get { return Instance.LoadNode<string>("Password"); }
            set { Instance["Password"] = value; }
        }

		public static int AvailableAliasCount
		{
			get { return Instance.LoadNode<int>("AvailableAliasCount"); }
			set { Instance["AvailableAliasCount"] = value; }
		}


        public static GlobalSettings Preferences
        {
            get
            {
                var result = Instance.LoadNode<GlobalSettings>("Preferences");

                if (result == null)
                    Preferences = result = GlobalSettings.SetupDefaults();

                return result;
            }
            set { Instance["Preferences"] = value; }
        }

        #endregion
    }
}