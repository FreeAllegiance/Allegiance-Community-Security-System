using System;
using System.Collections.Generic;
using Allegiance.CommunitySecuritySystem.DataAccess;
using Allegiance.CommunitySecuritySystem.Server.Contracts;
using Allegiance.CommunitySecuritySystem.Server.Interfaces;

namespace Allegiance.CommunitySecuritySystem.Server
{
    public partial class ClientService : IClientService
    {
        #region Contracts

        public List<Poll> ListPolls(AuthenticatedData data)
        {
            try
            {
                if (!data.Authenticate())
                    return null;

                return Poll.FindActivePolls(data.Username);
            }
            catch (Exception error)
            {
                Error.Write(error);
                throw;
            }
        }

        public void ApplyVote(PollData data)
        {
            try
            {
                if (!data.Authenticate())
                    return;

                data.ApplyVote();
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