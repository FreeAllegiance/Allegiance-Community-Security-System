using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Allegiance.CommunitySecuritySystem.DataAccess.Model;

namespace Allegiance.CommunitySecuritySystem.DataAccess
{
	public partial class GroupMessage : IMessage
    {
        #region Properties

        [DataMember]
        public string Sender
        {
            get { return String.Format("{0} ({1})", (this.Group == null || this.Group.Name == null) ? "Global" : this.Group.Name, this.Alias.Callsign); }
            set { }
        }

		public string ShortSubject
		{
			get
			{
				if (this.Subject.Length > 50)
					return this.Subject.Substring(0, 50) + "...";
				else
					return this.Subject;
			}
		}

        #endregion

        #region Methods

        public void MarkRead(CSSDataContext db, int aliasId)
        {
            var ga          = db.GroupMessage_Alias.FirstOrDefault(gma => gma.GroupMessageId == Id && gma.AliasId == aliasId);
            ga.DateViewed   = DateTime.Now;

            db.SubmitChanges(); 
        }

        static public List<IMessage> GetGroupMessages(CSSDataContext db, string callsign)
        {
			var messages = new List<IMessage>();
            var aliases     = Alias.ListAliases(db, callsign);

            foreach (var alias in aliases)
            {
				messages.AddRange(
					alias.GroupMessage_Alias.Where(p => p.DateViewed == null).Select(p => (IMessage) p.GroupMessage).ToList()
					);

				//messages.AddRange(alias.GroupMessages
				//    .Where(
				//        p => p.DateExpires <=  DateTime.Now
				//        &&	p.DateToSend >= DateTime.Now
				//        && p.GroupMessage_Alias
				//            .FirstOrDefault(r => r.Alias == alias && r.DateViewed == null) != null)
				//    .Select(q => (IMessage)q).ToList());

				foreach (var gma in alias.GroupMessage_Alias.Where(p => p.DateViewed == null && messages.Select(q => q.Id).Contains(p.GroupMessageId)))
				{
					//if (messages.FirstOrDefault(p => p.Id == gma.GroupMessageId) != null)
						gma.DateViewed = DateTime.Now;
				}

                db.SubmitChanges();
            }

            return messages;
        }

		static public List<IMessage> GetGlobalMessages(CSSDataContext db, string callsign)
		{
			var messages = new List<IMessage>();
			Alias alias = Alias.GetAliasByCallsign(db, callsign);

			messages.AddRange(
				db.GroupMessages
					.Where(p => p.DateCreated > alias.Login.Identity.LastGlobalMessageDelivery
						&& p.DateToSend.Date <= DateTime.Now.Date
						&& p.DateExpires > DateTime.Now.Date
						&& p.GroupId == null)
					.Select(q => (IMessage)q).ToList()
						);

			alias.Login.Identity.LastGlobalMessageDelivery = DateTime.Now;

			db.SubmitChanges();

			return messages;	
		}

		/// <summary>
		/// If the recipient is null, then the message will be sent to all users.
		/// </summary>
		/// <param name="db"></param>
		/// <param name="subject"></param>
		/// <param name="message"></param>
		/// <param name="recipient"></param>
		/// <param name="sendDate"></param>
		/// <param name="sender"></param>
        static public void NewMessage(CSSDataContext db, string subject, string message, string recipient, DateTime sendDate, Alias sender)
        {
			Group group = null;

			if (recipient != null)
			{
				group = db.Groups.FirstOrDefault(g => g.Name == recipient);

				if (group == null)
					throw new Exception("Could not find associated group.");
			}

            var msg = new GroupMessage()
            {
                Subject     = subject,
                Message     = message,
                Group       = group,
                DateCreated = DateTime.Now,
                DateExpires = DateTime.Now.AddYears(1), //Leaving at one year until discussion
                DateToSend  = sendDate,
				SenderAliasId = sender.Id
            };

			if (recipient != null)
			{
				var aliases = db.Group_Alias_GroupRoles
					.Where(g => g.GroupId == group.Id);

				foreach (var alias in aliases)
				{
					msg.GroupMessage_Alias.Add(new GroupMessage_Alias()
					{
						AliasId = alias.AliasId,
						DateViewed = null
					});
				}
			}
            
            db.GroupMessages.InsertOnSubmit(msg);
            db.SubmitChanges();
        }

        #endregion
    }
}