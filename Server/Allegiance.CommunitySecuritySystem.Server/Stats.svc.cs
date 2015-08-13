using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web.Caching;
using System.Web;
using Allegiance.CommunitySecuritySystem.Server.Contracts;

namespace Allegiance.CommunitySecuritySystem.Server
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Stats" in code, svc and config file together.
	//[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
	public class Stats : Interfaces.IStats
	{
		[WebInvoke(UriTemplate = "GetCurrentLeaderboardJson?token={token}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
		public List<DataAccess.StatsLeaderboard> GetCurrentLeaderboardJson(string token)
		{
			return GetCurrentLeaderboard(token);
		}

		[WebInvoke(UriTemplate = "GetCurrentLeaderboard?token={token}", Method = "GET")]
		public List<DataAccess.StatsLeaderboard> GetCurrentLeaderboard(string token)
		{
			ValidateToken(token);

			string cacheKey = "Stats::GetCurrentLeaderboard?token={token}";
			List<DataAccess.StatsLeaderboard> returnValue = (List<DataAccess.StatsLeaderboard>)HttpRuntime.Cache.Get(cacheKey);

			if (returnValue == null)
			{
				using (DataAccess.CSSStatsDataContext statsDB = new Allegiance.CommunitySecuritySystem.DataAccess.CSSStatsDataContext())
				{
					returnValue = DataAccess.StatsLeaderboard.GetSortedLeaderboard(statsDB);
					HttpRuntime.Cache.Add(cacheKey, returnValue, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, 30), CacheItemPriority.Normal, null);
				}
			}

			return returnValue;
		}


		[WebInvoke(UriTemplate = "GetActivePlayersJson?token={token}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, Method = "GET")]
		public List<ActivePlayerData> GetActivePlayersJson(string token)
		{
			return GetActivePlayers(token);
		}

		[WebInvoke(UriTemplate = "GetActivePlayers?token={token}", Method = "GET")]
		public List<ActivePlayerData> GetActivePlayers(string token)
		{
			ValidateToken(token);

			string cacheKey = "Stats::GetActivePlayers";

			List<ActivePlayerData> returnValue = (List<ActivePlayerData>)HttpRuntime.Cache.Get(cacheKey);

			if (returnValue == null)
			{
				using (DataAccess.CSSDataContext db = new Allegiance.CommunitySecuritySystem.DataAccess.CSSDataContext())
				{
					//var activeLogins = db.Logins
					//    .Where(p => p.Sessions.Where(r => r.DateLastCheckIn > DateTime.Now.AddMinutes(-3)).Count() > 0)
					//    .OrderBy(p => p.Username)
					//    .ToList();

					var activeSessions = db.Sessions
						.Where(r => r.DateLastCheckIn > DateTime.Now.AddMinutes(-3))
						.OrderBy(p => p.Alias.Callsign)
						.ToList();

					using (DataAccess.CSSStatsDataContext statsDB = new DataAccess.CSSStatsDataContext())
					{
						returnValue = activeSessions.GroupJoin(statsDB.StatsLeaderboards, p => p.LoginId, r => r.LoginID, (session, leaderboard) => new { session, leaderboard })
							.SelectMany(z => z.leaderboard.DefaultIfEmpty(), (z, leaderboard) => new { z.session, leaderboard })
							.Select(p => new ActivePlayerData()
							{
								PlayerName = p.session.Alias.Callsign,
								Rank = p.leaderboard == null ? 0 : (int)Math.Floor(p.leaderboard.Rank)
							})
							.OrderBy(p => p.PlayerName)
							.ToList();

						//returnValue = activeLogins.GroupJoin(statsDB.StatsLeaderboards, p => p.Id, r => r.LoginID, (login, leaderboard) => new {login, leaderboard})
						//    .SelectMany(z => z.leaderboard.DefaultIfEmpty(), (z, leaderboard) => new { z.login, leaderboard } )
						//    .Select(p => new ActivePlayerData()
						//    {
						//        PlayerName = p.login.Username,
						//        Rank = p.leaderboard == null ? 0 : (int) Math.Floor(p.leaderboard.Rank)
						//    })
						//    .OrderBy(p => p.PlayerName)
						//    .ToList();

						//var loginStats = statsDB.StatsLeaderboards
						//    .Join(activeLogins, p => p.LoginID, r => r.Id, ( p, 
						//    .Where(p => activeLogins.Select(r => r.Id).Contains(p.LoginID) == true).OrderBy(p => p.LoginUsername);
						//returnValue = loginStats.ToList();
					}

					HttpRuntime.Cache.Add(cacheKey, returnValue, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, 30), CacheItemPriority.Normal, null);
				}
			}

			return returnValue;
		}

		private void ValidateToken(string token)
		{
			if (String.IsNullOrEmpty(token) == false)
			{
				foreach (string userToken in Configuration.Instance.CssServer.UserTokens)
				{
					if (token.Equals(Configuration.Instance.CssServer.UserTokens[userToken]) == true)
						return;
				}
			}

			throw new HttpException(401, "Unauthorized access");
		}
	}
}
