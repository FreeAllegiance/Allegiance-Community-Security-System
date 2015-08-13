using System.Runtime.Serialization;
using Allegiance.CommunitySecuritySystem.Common.Enumerations;
using System.Collections.Generic;
using Allegiance.CommunitySecuritySystem.DataAccess;

namespace Allegiance.CommunitySecuritySystem.Server.Contracts
{
    [DataContract]
	public class ListAliasesResult 
    {
        #region Properties

        [DataMember]
        public List<Alias> Aliases { get; set; }

        [DataMember]
        public int AvailableAliasCount { get; set; }

        #endregion
    }
}