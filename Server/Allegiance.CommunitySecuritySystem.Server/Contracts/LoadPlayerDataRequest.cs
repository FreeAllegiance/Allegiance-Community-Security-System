using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Allegiance.CommunitySecuritySystem.Server.Contracts
{
	[DataContract]
	public class LoadPlayerDataRequest
	{
		[DataMember]
		public string LoginUsername { get; set; }
		[DataMember]
		public Guid GameGuid { get; set; }
		[DataMember]
		public float Score { get; set; }
		[DataMember]
		public int PilotBaseKills { get; set; }
		[DataMember]
		public int PilotBaseCaptures { get; set; }
		[DataMember]
		public float WarpsSpotted { get; set; }
		[DataMember]
		public float AsteroidsSpotted { get; set; }
		[DataMember]
		public float MinerKills { get; set; }
		[DataMember]
		public float BuilderKills { get; set; }
		[DataMember]
		public float LayerKills { get; set; }
		[DataMember]
		public float CarrierKills { get; set; }
		[DataMember]
		public float PlayerKills { get; set; }
		[DataMember]
		public float BaseKills { get; set; }
		[DataMember]
		public float BaseCaptures { get; set; }
		[DataMember]
		public float TechsRecovered { get; set; }
		[DataMember]
		public int Flags { get; set; }
		[DataMember]
		public int Artifacts { get; set; }
		[DataMember]
		public int Rescues { get; set; }
		[DataMember]
		public int Kills { get; set; }
		[DataMember]
		public int Assists { get; set; }
		[DataMember]
		public int Deaths { get; set; }
		[DataMember]
		public int Ejections { get; set; }
		[DataMember]
		public float CombatRating { get; set; }
		[DataMember]
		public bool Win { get; set; }
		[DataMember]
		public bool Lose { get; set; }
		[DataMember]
		public bool CommandWin { get; set; }
		[DataMember]
		public bool CommandLose { get; set; }
		[DataMember]
		public float TimePlayed { get; set; }
		[DataMember]
		public float TimeCommanded { get; set; }
		[DataMember]
		public bool CommandCredit { get; set; }
	}
}
