using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel.Activation;
using Allegiance.CommunitySecuritySystem.Server.Contracts;

namespace Allegiance.CommunitySecuritySystem.Server
{
	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	//[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
	public class GameData
	{
		private bool TryVerifyConnection(out string currentIPAddress)
		{
			currentIPAddress = "127.0.0.1"; // Supports unit tests.

			if (OperationContext.Current != null)
			{
				//http://nayyeri.net/detect-client-ip-in-wcf-3-5
				OperationContext context = OperationContext.Current;
				MessageProperties messageProperties = context.IncomingMessageProperties;
				RemoteEndpointMessageProperty endpointProperty = (RemoteEndpointMessageProperty)messageProperties[RemoteEndpointMessageProperty.Name];

				currentIPAddress = endpointProperty.Address;
			}

			using (DataAccess.CSSStatsDataContext statsDB = new DataAccess.CSSStatsDataContext())
			{
				string ipAddress = currentIPAddress;

				var gameServer = statsDB.GameServers.FirstOrDefault(p => p.GameServerIPs.Where(r => r.IPAddress == ipAddress).Count() > 0);

				if (gameServer == null)
					return false;
			}

			return true;
		}

		[OperationContract]
		public LoadPlayerDataResponse LoadPlayerData(LoadPlayerDataRequest playerData)
		{
			string currentIPAddress;

			if (TryVerifyConnection(out currentIPAddress) == false)
				return new LoadPlayerDataResponse()
				{
					ErrorMessage = "You may not upload data from this address: " + currentIPAddress,
					Succeeded = false
				};

			return new LoadPlayerDataResponse(playerData);
		}

		[OperationContract]
		public CommitPlayerDataResponse CommitPlayerData(CommitPlayerDataRequest request)
		{
			string currentIPAddress;

			if (TryVerifyConnection(out currentIPAddress) == false)
				return new CommitPlayerDataResponse()
				{
					ErrorMessage = "You may not commit data from this address: " + currentIPAddress,
					Succeeded = false
				};

			return new CommitPlayerDataResponse(request);
		}
	}
}
