using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Allegiance.CommunitySecuritySystem.DataAccess.Model
{
	public interface IMessage
	{
		[DataMember]
		int Id { get; set; }

		[DataMember]
		string Sender { get; set; }

        [DataMember]
        string Message { get; set; }

        [DataMember]
        string Subject { get; set; }

        [DataMember]
        DateTime DateCreated { get; set; }

        [DataMember]
        DateTime DateToSend { get; set; }

        [DataMember]
        DateTime? DateExpires { get; set; }

		//void MarkRead(CSSDataContext db, int aliasId);
	}
}