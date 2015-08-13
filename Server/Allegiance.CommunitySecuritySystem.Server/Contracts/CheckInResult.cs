using System.Runtime.Serialization;
using Allegiance.CommunitySecuritySystem.Common.Enumerations;
using System;

namespace Allegiance.CommunitySecuritySystem.Server.Contracts
{
    [DataContract]
    public class CheckInResult
    {
        #region Properties

        [DataMember]
        public CheckInStatus Status { get; set; }

        [DataMember]
        public string Ticket { get; set; }

        #endregion
    }
}