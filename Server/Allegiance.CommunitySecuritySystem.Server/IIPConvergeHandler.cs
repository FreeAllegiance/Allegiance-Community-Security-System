using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;
using System.Collections.Specialized;

namespace Allegiance.CommunitySecuritySystem.Server
{
	interface IIPConvergeHandler 
	{
		//////////////////////////////////////////////////////////
		// Handshake
		//////////////////////////////////////////////////////////
		
		[XmlRpcMethod("ipConverge.handshakeStart")]
		[return: XmlRpcReturnValue]
		HandshakeStartResponse HandshakeStart(
			[XmlRpcParameter] HandshakeStartRequest handshakeStartRequest
			);

		[XmlRpcMethod("ipConverge.handshakeEnd")]
		[return: XmlRpcReturnValue]
		HandshakeEndResponse HandshakeEnd(
			[XmlRpcParameter] HandshakeEndRequest handshakeEndRequest
			);

		[XmlRpcMethod("ipConverge.handshakeRemove")]
		[return: XmlRpcReturnValue]
		HandshakeRemoveResponse HandshakeRemove(
			[XmlRpcParameter] HandshakeRemoveRequest handshakeRemoveRequest
			);


		//////////////////////////////////////////////////////////
		// Converge API Methods
		//////////////////////////////////////////////////////////
		[XmlRpcMethod("ipConverge.getMembersInfo")]
		[return: XmlRpcReturnValue]
		ProcessGetMembersInfoResponse ProcessGetMembersInfo(
			[XmlRpcParameter] ProcessGetMembersInfoRequest processGetMembersInfoRequest
			);
	}
}
