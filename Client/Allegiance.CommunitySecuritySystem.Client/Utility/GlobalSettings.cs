using System;
using Allegiance.CommunitySecuritySystem.Client.Integration;

namespace Allegiance.CommunitySecuritySystem.Client.Utility
{
    [Serializable]
    public  class GlobalSettings
    {
        #region Constant Fields

		public const int MinAliasLength = 3;
		public const int MaxAliasLength = 17;

        public const string ClientProcessName       = "Launcher";

        public const string TempProcessName         = "~nlauncher";

        public const string ClientExecutableName    = ClientProcessName + ".exe";

		public const string ClientExecutablePDB		= ClientProcessName + ".pdb";

        public const string TempExecutableName      = TempProcessName   + ".exe";

		public const string TempExecutablePDB		= TempProcessName + ".pdb";

        #endregion

        #region Properties

        //Preferences Information
        public bool LaunchWindowed { get; set; }

        public bool LogChat { get; set; }

        public bool AutoLogin { get; set; }

        public bool SafeMode { get; set; }

        public bool DebugLog { get; set; }

        public bool NoMovies { get; set; }

        #endregion

        #region Methods

        public static GlobalSettings SetupDefaults()
        {
            return new GlobalSettings()
            {
                //Preferences
                LaunchWindowed  = false,
                LogChat         = true,
                AutoLogin       = false,
                SafeMode        = false,
                DebugLog        = true,
                NoMovies        = true,
            };
        }

        #endregion
    }
}