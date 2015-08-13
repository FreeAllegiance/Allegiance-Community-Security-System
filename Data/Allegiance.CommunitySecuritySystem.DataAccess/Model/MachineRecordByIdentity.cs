using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Allegiance.CommunitySecuritySystem.Common.Utility;

namespace Allegiance.CommunitySecuritySystem.DataAccess.Model
{
	public class MachineRecordByIdentity
	{
		public int IdentityId { get; set; }
		public int RecordTypeId { get; set; }
		public string Identifier { get; set; }

		public MachineRecordByIdentity()
		{
		}

		public static List<MachineRecordByIdentity> GetAllFromCache()
		{
			return CacheManager<List<MachineRecordByIdentity>>.Get("MachineRecordByIdentity::GetAll()", CacheSeconds.TenSeconds, delegate()
			{
				using(CSSDataContext db = new CSSDataContext())
				{
					return db.MachineRecords.Select(p => new MachineRecordByIdentity()
						{
							Identifier = p.Identifier,
							IdentityId = p.Login.IdentityId,
							RecordTypeId = p.RecordTypeId
						})
						.ToList();
				}
			});
		}
	}
}
