using System;
using System.Configuration;
using Allegiance.CommunitySecuritySystem.Common.Extensions;
using Allegiance.CommunitySecuritySystem.Common.Utility;
using Allegiance.CommunitySecuritySystem.DataAccess.Enumerations;

namespace Allegiance.CommunitySecuritySystem.DataAccess
{
    public partial class Session
    {
        #region Fields

        /// <summary>
        /// The default time (in seconds) which a session may stay open prior to the first CheckIn.
        /// </summary>
        public const int DefaultInitialCheckinTimeout = 10;

        /// <summary>
        /// The standard checkin time (in seconds) which a session may stay open between CheckIns.
        /// </summary>
        public const int DefaultStandardTimeout = 180;

        #endregion

        #region Properties

        /// <summary>
        /// Retrieves the Initial Checkin Timeout value from the Configuration file. The initial checkin timeout
        /// determines the maximum amount of time a session may stay open before a the first CheckIn is done.
        /// </summary>
        protected int InitialCheckinTimeout
        {
            get { return ConfigurationManager.AppSettings.GetValue<int>("InitialCheckinTimeout", p => int.Parse(p), DefaultInitialCheckinTimeout); }
        }

        /// <summary>
        /// Retrieves the Standard Checkin Timeout vaulue frmo the Configuration file. The standard checkin
        /// timeout determines the maximum amount of time a session may stay open between checkins.
        /// </summary>
        protected int StandardCheckinTimeout
        {
            get { return ConfigurationManager.AppSettings.GetValue<int>("StandardCheckinTimeout", p => int.Parse(p), DefaultStandardTimeout); }
        }

        /// <summary>
        /// Retrieves the current Session status.
        /// </summary>
        public SessionStatusEnum SessionStatusType
        {
            get { return (SessionStatusEnum)this.SessionStatusId; }
            set { this.SessionStatusId = (int)value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Check timestamps to ensure the session has not timed out.
        /// </summary>
        /// <returns>True if session has not timed out, and is otherwise valid.</returns>
        public bool ValidateSessionTimeout()
        {
            //Check if blackbox timed out
            if (SessionStatusType == SessionStatusEnum.Closed)
                return false;

            //Check if blackbox has timed out
            if (SessionStatusType == SessionStatusEnum.PendingVerification
                && (DateTime.Now - DateLastCheckIn).TotalSeconds > InitialCheckinTimeout)
                return false;

            //Check if session has timed out
            else if (SessionStatusType == SessionStatusEnum.Active
                && (DateTime.Now - DateLastCheckIn).TotalSeconds > StandardCheckinTimeout)
                return false;

            return true;
        }

        /// <summary>
        /// Returns a SHA1 hashed combination of Session Token & Username.
        /// </summary>
        /// <returns></returns>
        public string HashSession()
        {
			return Encryption.SHA256Hash(this.ActiveKey.Token + this.Login.Username);
        }

        #endregion
    }
}