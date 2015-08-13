using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Allegiance.CommunitySecuritySystem.Server.Contracts
{
	[DataContract]
	public class GetBannedUsernamesAfterTimestampResult
	{
		[DataMember]
		public string BannedUserNames { get; set; }

		[DataMember]
		public long CurrentTimestamp { get; set; }
	}
}