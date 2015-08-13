using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegiance.CommunitySecuritySystem.Common.Enumerations
{
	public enum AllegianceEventIDs
	{
		GameTagged = -2000,
		DrawFailed = -1003,
		DrawPassed = -1002,
		ResignFailed = -1001,
		ResignPassed = -1000,
		StationCreated = 201,
		StationDestroyed = 202,
		StationCaptured = 203,
		ShipKilled = 302,
		PlayerDropped = 2004,
		LobbyLost = 2028,
		GameStarted = 2029,
		GameEnded = 2030,
		GameOver = 2031,
		LobbyConnected = 2033,
		LobbyDisconnecting = 2034,
		LobbyDisconnected = 2035,
		ChatMessage = 4030,
		GameCreated = 4040,
		GameDestroyed = 4041,
		LoginGame = 4050,
		LogoutGame = 4051,
		JoinedTeam = 4060,
		LeftTeam = 4061,
		TeamInfoChanged = 4062
	}
}
