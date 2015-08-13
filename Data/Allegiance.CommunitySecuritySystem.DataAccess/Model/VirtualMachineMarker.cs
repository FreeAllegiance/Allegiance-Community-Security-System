using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Allegiance.CommunitySecuritySystem.Common.Envelopes.AuthInfo;

namespace Allegiance.CommunitySecuritySystem.DataAccess
{
	public partial class VirtualMachineMarker
	{
		public DeviceType DeviceType
		{
			get
			{
				return (DeviceType)this.RecordTypeId;
			}

			set
			{
				this.RecordTypeId = (int)value;
			}
		}

		public static bool IsMachineInformationFromAVirtualMachine(CSSDataContext db, MachineInformation machineInformation, Login login)
		{
			var virtualMachineRecord = machineInformation.MachineValues.FirstOrDefault(p => db.VirtualMachineMarkers.Count(q => System.Data.Linq.SqlClient.SqlMethods.Like(p.Value, q.IdentifierMask) && (DeviceType) q.RecordTypeId == p.Type) > 0);
			if (virtualMachineRecord != null)
			{
				Log.Write(db, Enumerations.LogType.AuthenticationServer, "LoginID: " + login.Id + ", Name: " + login.Username + ", Virtual Machine Detected: name: " + virtualMachineRecord.Name + ", type: " + virtualMachineRecord.Type + ", value: " + virtualMachineRecord.Value + ".");
				return true;
			}

			return false;
		}
	}
}
