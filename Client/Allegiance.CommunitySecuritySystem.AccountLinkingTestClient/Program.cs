using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Allegiance.CommunitySecuritySystem.Common.Envelopes.AuthInfo;
using Allegiance.CommunitySecuritySystem.DataAccess;

namespace AccountLinkingTestClient
{
	class Program
	{
		static void Main(string[] args)
		{
			using (CSSDataContext db = new CSSDataContext())
			{
				var linkedLogin = db.Logins.FirstOrDefault(p => p.Id == 134);

				var linkedMachineRecords = db.MachineRecords.Where(p => linkedLogin.Identity.Logins.Contains(p.Login));
				foreach (var machineRecord in linkedMachineRecords)
				{
					Console.WriteLine("loginID: " + machineRecord.LoginId + ", machineRecord: " + machineRecord.Id + ", " + machineRecord.Identifier);
				}

				var machineRecords = db.MachineRecords.Where(p => p.LoginId == 135);
				List<DeviceInfo> deviceInfos = new List<DeviceInfo>();

				foreach (MachineRecord machineRecord in machineRecords)
				{
					deviceInfos.Add(new DeviceInfo()
					{
						Name = machineRecord.DeviceType.ToString(),
						Type = machineRecord.DeviceType,
						Value = machineRecord.Identifier
					});
				}

				MachineInformation mi = new MachineInformation()
				{
					MachineValues = deviceInfos,
					Token = "Test Token"
				};

				var login = db.Logins.FirstOrDefault(p => p.Id == 135);

				bool wasMerged;
				Identity identity;
				Identity.MatchIdentity(db, (Login)login, mi, out identity, out wasMerged);
			}
		}
	}
}
