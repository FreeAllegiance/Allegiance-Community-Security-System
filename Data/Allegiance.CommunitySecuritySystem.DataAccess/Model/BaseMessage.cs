using System;
using System.Runtime.Serialization;

namespace Allegiance.CommunitySecuritySystem.DataAccess.Model
{
    [KnownType(typeof(PersonalMessage))]
    [KnownType(typeof(GroupMessage))]
    [DataContract]
    public class BaseMessage
    {
        #region Properties
        
        [DataMember]
        public virtual int Id { get; set; }

        [DataMember]
        public virtual string Sender { get; set; }

        [DataMember]
        public virtual string Message { get; set; }

        [DataMember]
        public virtual string Subject { get; set; }

        [DataMember]
        public virtual DateTime DateCreated { get; set; }

        [DataMember]
        public virtual DateTime DateToSend { get; set; }

        [DataMember]
        public virtual DateTime? DateExpires { get; set; }

        #endregion

        #region Methods

        public virtual void MarkRead(CSSDataContext db, int aliasId)
        {
        }

        #endregion
    }
}