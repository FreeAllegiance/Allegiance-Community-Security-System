using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Allegiance.CommunitySecuritySystem.Server.Contracts
{
	[DataContract]
	public class IsAliasLegacyResult
	{
		[DataMember]
		public bool IsAliasLegacy { get; set; }
	}
}