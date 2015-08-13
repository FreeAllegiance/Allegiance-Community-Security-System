using System;
using System.Collections.Generic;
using System.Linq;
using Allegiance.CommunitySecuritySystem.DataAccess.Model;

namespace Allegiance.CommunitySecuritySystem.DataAccess
{
	public partial class PersonalMessage : IMessage
    {
        #region Properties

        public string Sender
        {
            get { return this.Alias.Callsign; }
            set { }
        }

        #endregion

        #region Methods

        public void MarkRead(CSSDataContext db, int aliasId)
        {
            DateViewed = DateTime.Now;
            db.SubmitChanges();
        }

        static public List<IMessage> GetPrivateMessages(CSSDataContext db, string alias)
        {
            var id          = Login.FindLoginByUsernameOrCallsign(db, alias).Id;
            var messages    = db.PersonalMessages
                                .Where(pm => pm.LoginId == id && pm.DateViewed == null 
                                    && pm.DateToSend < DateTime.Now).ToList();

            foreach (var msg in messages)
                msg.DateViewed = DateTime.Now;

            db.SubmitChanges();

            //return messages.Select(p => p as BaseMessage).ToList();
			return messages.Select(p => p as IMessage).ToList();
        }

        static public void NewMessage(CSSDataContext db, string subject, 
            string message, Alias sender, Login recipient, DateTime sendDate)
        {
            db.PersonalMessages.InsertOnSubmit(new PersonalMessage()
            {
                Subject         = subject,
                Message         = message,
                SenderAliasId   = sender.Id,
                LoginId         = recipient.Id,
                DateCreated     = DateTime.Now,
                DateExpires     = DateTime.Now.AddYears(1), //Leaving at one year until discussion
                DateToSend      = sendDate,
                DateViewed      = null
            });

            db.SubmitChanges();
        }

        #endregion
    }
}