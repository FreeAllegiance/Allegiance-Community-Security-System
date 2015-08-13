using System;
using System.Collections.Generic;
using System.Linq;
using Allegiance.CommunitySecuritySystem.DataAccess;
using Allegiance.CommunitySecuritySystem.Server.Contracts;
using Allegiance.CommunitySecuritySystem.Server.Interfaces;

namespace Allegiance.CommunitySecuritySystem.Server
{
    public partial class ClientService : IClientService
    {
        #region Contracts

        public AutoUpdateResult CheckForUpdates(int lobbyId)
        {
            try
            {
                //Check current version, send updated files
                return AutoUpdateResult.RetrieveFileList(lobbyId);
            }
            catch (Exception error)
            {
                Error.Write(error);
                throw;
            }
        }

        public List<LobbyResult> CheckAvailableLobbies()
        {
            try
            {
                using (var db = new CSSDataContext())
                {
					// perform updates for all available lobbies. The update check is moved to application
					// startup, before the user logs in. Otherwise the the launcher can't update if
					// the signature to the authentication service changes.
					return db.Lobbies.Where(p => p.IsEnabled == true).Select(l => new LobbyResult()
					{
						LobbyId = l.Id,
						Name = l.Name,
						Host = l.Host
					}).ToList();
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