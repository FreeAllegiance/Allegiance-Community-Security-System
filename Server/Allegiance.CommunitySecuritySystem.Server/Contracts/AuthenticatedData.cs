using System.Runtime.Serialization;
using Allegiance.CommunitySecuritySystem.BlackboxGenerator;
using Allegiance.CommunitySecuritySystem.Common.Enumerations;
using System;

namespace Allegiance.CommunitySecuritySystem.Server.Contracts
{
    [DataContract]
    public class AuthenticatedData
    {
        #region Properties

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Password { get; set; }


        #endregion

        #region Methods

        internal bool Authenticate(out string loginUserName)
        {
			bool returnValue = Validation.ValidateLogin(Username, Password, out loginUserName);
			return returnValue;
        }

        internal bool Authenticate(params RoleType[] requiredRoles)
        {
			string loginUserName;

			bool returnValue = Validation.ValidateLogin(Username, Password, out loginUserName, requiredRoles);

			return returnValue;
        }

        #endregion
    }
}