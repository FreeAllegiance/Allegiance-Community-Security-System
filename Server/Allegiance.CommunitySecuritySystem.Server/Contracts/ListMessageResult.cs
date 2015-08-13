using System.Runtime.Serialization;
using Allegiance.CommunitySecuritySystem.Common.Enumerations;
using Allegiance.CommunitySecuritySystem.DataAccess.Model;
using System.Collections.Generic;

namespace Allegiance.CommunitySecuritySystem.Server.Contracts
{
    [DataContract]
    public class ListMessageResult
    {
		public class ListMessage : IMessage
		{
			#region IMessage Members

			[DataMember]
			public int Id { get; set; }

			[DataMember]
			public string Sender { get; set; }

			[DataMember]
			public string Message { get; set; }

			[DataMember]
			public string Subject { get; set; }

			[DataMember]
			public System.DateTime DateCreated { get; set; }

			[DataMember]
			public System.DateTime DateToSend { get; set; }

			[DataMember]
			public System.DateTime? DateExpires { get; set; }

			//public void MarkRead(DataAccess.CSSDataContext db, int aliasId)
			//{
			//    throw new System.NotImplementedException();
			//}

			#endregion
		}


        #region Properties

        [DataMember]
        public List<ListMessage> Messages { get; set; }

        #endregion
    }
}