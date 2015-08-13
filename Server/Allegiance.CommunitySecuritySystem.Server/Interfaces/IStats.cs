using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Allegiance.CommunitySecuritySystem.Server.Contracts;

namespace Allegiance.CommunitySecuritySystem.Server.Interfaces
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IStats" in both code and config file together.
	[ServiceContract]
	public interface IStats
	{
		[OperationContract]
		List<DataAccess.StatsLeaderboard> GetCurrentLeaderboard(string token);

		[OperationContract]
		List<DataAccess.StatsLeaderboard> GetCurrentLeaderboardJson(string token);

		[OperationContract]
		List<ActivePlayerData> GetActivePlayers(string token);

		[OperationContract]
		List<ActivePlayerData> GetActivePlayersJson(string token);
	}
}
