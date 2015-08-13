using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Allegiance.CommunitySecuritySystem.Server
{
	[DataContract]
	public class CommitPlayerDataRequest
	{
		[DataMember]
		public Guid GameGuid { get; set; }
	}
}
