using System.Collections.Generic;
using System.ServiceModel;
using Allegiance.CommunitySecuritySystem.DataAccess;
using Allegiance.CommunitySecuritySystem.Server.Contracts;

namespace Allegiance.CommunitySecuritySystem.Server.Interfaces
{
    [ServiceContract]
    public interface IAdministration
    {
        [OperationContract]
        bool SetBan(BanData data);

        [OperationContract]
        void RemoveBan(BanData data);

        [OperationContract]
        List<Ban> ListBans(AuthenticatedData data, string alias);

        [OperationContract]
        List<Alias> ListAliases(AuthenticatedData data, string alias);

        [OperationContract]
        List<BanType> ListInfractionTypes(AuthenticatedData data);
    }
}