using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Activation;

namespace Allegiance.CommunitySecuritySystem.Server.Interfaces
{
	// NOTE: If you change the interface name "ITag" here, you must also update the reference to "ITag" in Web.config.
	[ServiceContract]
	public interface ITag
	{
		[OperationContract]
		int SaveGameData(string gameData, out string message);
	}
}
