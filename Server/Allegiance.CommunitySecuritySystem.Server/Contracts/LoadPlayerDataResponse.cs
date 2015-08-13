using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Allegiance.CommunitySecuritySystem.DataAccess;

namespace Allegiance.CommunitySecuritySystem.Server.Contracts
{
	[DataContract]
	public class LoadPlayerDataResponse
	{
		[DataMember]
		public bool Succeeded { get; set; }
		[DataMember]
		public string ErrorMessage { get; set; }

		public LoadPlayerDataResponse() { }

		public LoadPlayerDataResponse(LoadPlayerDataRequest playerData)
		{
			using (CSSStatsDataContext statsDB = new CSSStatsDataContext())
			{
				using (CSSDataContext db = new CSSDataContext())
				{
					var login = Login.FindLoginByUsernameOrCallsign(db, playerData.LoginUsername);

					if (login == null)
					{
						Succeeded = false;
						ErrorMessage = "Couldn't find player's login user id.";
						return;
					}

					ScoreQueue scoreQueue = statsDB.ScoreQueues.FirstOrDefault(p => p.LoginId == login.Id && p.GameGuid == playerData.GameGuid);

					if (scoreQueue == null)
					{
						statsDB.ScoreQueues.InsertOnSubmit(new ScoreQueue()
						{
							LoginId = login.Id,
							GameGuid = playerData.GameGuid,
							Score = playerData.Score,
							PilotBaseKills = playerData.PilotBaseKills,
							PilotBaseCaptures = playerData.PilotBaseCaptures,
							WarpsSpotted = playerData.WarpsSpotted,
							AsteroidsSpotted = playerData.AsteroidsSpotted,
							MinerKills = playerData.MinerKills,
							BuilderKills = playerData.BuilderKills,
							LayerKills = playerData.LayerKills,
							CarrierKills = playerData.CarrierKills,
							PlayerKills = playerData.PlayerKills,
							BaseKills = playerData.BaseKills,
							BaseCaptures = playerData.BaseCaptures,
							TechsRecovered = playerData.TechsRecovered,
							Flags = playerData.Flags,
							Artifacts = playerData.Artifacts,
							Rescues = playerData.Rescues,
							Kills = playerData.Kills,
							Assists = playerData.Assists,
							Deaths = playerData.Deaths,
							Ejections = playerData.Ejections,
							Win = playerData.Win,
							Lose = playerData.Lose,
							CommandWin = playerData.CommandWin,
							CommandLose = playerData.CommandLose,
							TimePlayed = playerData.TimePlayed,
							TimeCommanded = playerData.TimeCommanded,
							CommandCredit = playerData.CommandCredit,
							CombatRating = playerData.CombatRating
						});
					}


					
						
				

					statsDB.SubmitChanges();
				}
			}

			this.Succeeded = true;
		}
	}
}
