using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.SqlClient;
using Allegiance.CommunitySecuritySystem.Common.Envelopes.AuthInfo;
using System.Web.Caching;
using Allegiance.CommunitySecuritySystem.Common.Extensions;
using Allegiance.CommunitySecuritySystem.Common.Utility;

namespace Allegiance.CommunitySecuritySystem.DataAccess
{
	public partial class MachineRecordExclusion
	{
		public DeviceType DeviceType
		{
			get
			{
				return (DeviceType)this.RecordTypeId;
			}

			set
			{
				this.RecordTypeId = (int) value;
			}
		}


		public static bool IsMachineRecordExcluded(MachineRecord machineRecord)
		{
			//using (CSSDataContext db = new CSSDataContext())
			//{
			//    if (db.MachineRecordExclusions.FirstOrDefault(p => SqlMethods.Like(machineRecord.Identifier, p.IdentifierMask) && p.RecordTypeId == machineRecord.RecordTypeId) != null)
			//        return true;
			//}

			List<MachineRecordExclusion> allMachineRecordExclusions = GetAllFromCache();

			if (allMachineRecordExclusions.FirstOrDefault(p => machineRecord.Identifier.Like(p.IdentifierMask) && p.RecordTypeId == machineRecord.RecordTypeId) != null)
				return true;

			return false;
		}


		public static List<MachineRecordExclusion> GetAllFromCache()
		{
			List<MachineRecordExclusion> returnValue;

			string cacheKey = "MachineRecordExclusion::GetAllMachineRecordExclusions()";

			returnValue = CacheManager<List<MachineRecordExclusion>>.Get(cacheKey, CacheSeconds.TenSeconds, delegate()
			{
				using (CSSDataContext db = new CSSDataContext())
				{
					return db.MachineRecordExclusions.ToList();
				}
			});

			return returnValue;
		}
	}
}
