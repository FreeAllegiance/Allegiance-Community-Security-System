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

        public List<BaseMessage> ListMessages(AuthenticatedData data)
        {
            try
            {
                if (!data.Authenticate())
                    return null;

                using (var db = new CSSDataContext())
                {
                    var messages = new List<BaseMessage>();
                    messages.AddRange(GroupMessage.GetGroupMessages(db, data.Username));
                    messages.AddRange(PersonalMessage.GetPrivateMessages(db, data.Username));

                    //Re-insert messages into list since windows services 
                    //don't seem to be able to handle objects wrapped in a baseclass.
                    var result = messages
                        .OrderByDescending(p => p.DateToSend)
                        .Select(p => new BaseMessage()
                    {
                        Id = p.Id,
                        Sender = p.Sender,
                        Subject = p.Subject,
                        Message = p.Message,
                        DateCreated = p.DateToSend,     //User doesn't actually have to know the date the message was created - only sent.
                        DateToSend = p.DateToSend,
                        DateExpires = p.DateExpires,
                    }).ToList();

                    return result;
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
