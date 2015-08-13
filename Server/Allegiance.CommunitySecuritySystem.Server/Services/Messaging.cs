using System;
using System.Collections.Generic;
using System.Linq;
using Allegiance.CommunitySecuritySystem.DataAccess;
using Allegiance.CommunitySecuritySystem.DataAccess.Model;
using Allegiance.CommunitySecuritySystem.Server.Contracts;
using Allegiance.CommunitySecuritySystem.Server.Interfaces;

namespace Allegiance.CommunitySecuritySystem.Server
{
    public partial class ClientService : IClientService
    {
        #region Contracts

        public ListMessageResult ListMessages(AuthenticatedData data)
        {
            try
            {
                if (!data.Authenticate())
                    return null;

                using (var db = new CSSDataContext())
                {
					var messages = new List<IMessage>();
                    messages.AddRange(GroupMessage.GetGroupMessages(db, data.Username));
                    messages.AddRange(PersonalMessage.GetPrivateMessages(db, data.Username));
					messages.AddRange(GroupMessage.GetGlobalMessages(db, data.Username));

                    //Re-insert messages into list since windows services 
                    //don't seem to be able to handle objects wrapped in a baseclass.
					//var result = messages
					//    .OrderByDescending(p => p.DateToSend)
					//    .Select(p => new BaseMessage()
					//{
					//    Id = p.Id,
					//    Sender = p.Sender,
					//    Subject = p.Subject,
					//    Message = p.Message,
					//    DateCreated = p.DateToSend,     //User doesn't actually have to know the date the message was created - only sent.
					//    DateToSend = p.DateToSend,
					//    DateExpires = p.DateExpires,
					//}).ToList();

                    //return result;

					ListMessageResult returnValue = new ListMessageResult();
					returnValue.Messages = new List<ListMessageResult.ListMessage>();
					messages.ForEach(p => returnValue.Messages.Add(new ListMessageResult.ListMessage()
					{
						DateCreated = p.DateCreated,
						DateExpires = p.DateExpires,
						DateToSend = p.DateToSend,
						Id = p.Id,
						Sender = p.Sender,
						Subject = p.Subject,
						Message = p.Message
					}));

					
					return returnValue;
                }
            }
            catch (Exception error)
            {
                Error.Write(error);
                throw;
            }
        }

        #endregion
    }
}
