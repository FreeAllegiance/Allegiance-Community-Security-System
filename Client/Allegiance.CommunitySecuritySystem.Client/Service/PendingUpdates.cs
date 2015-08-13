using System;
using System.Collections.Generic;
using System.Text;
using Allegiance.CommunitySecuritySystem.Client.ClientService;

namespace Allegiance.CommunitySecuritySystem.Client.Service
{
	public class PendingUpdates
	{
		public Dictionary<int, List<FindAutoUpdateFilesResult>> PendingUpdateList = new Dictionary<int, List<FindAutoUpdateFilesResult>>();
		public Dictionary<int, string> AutoUpdateBaseAddress = new Dictionary<int, string>();
		public Dictionary<int, FindAutoUpdateFilesResult[]> AllFilesInUpdatePackage = new Dictionary<int, FindAutoUpdateFilesResult[]>();

		public bool HasPendingUpdates 
		{
			get
			{
				foreach (List<FindAutoUpdateFilesResult> pendingUpdates in PendingUpdateList.Values)
				{
					if (pendingUpdates.Count > 0)
						return true;
				}

				return false;
			}
		}

		public int PendingUpdateCount
		{
			get
			{
				int pendingUpdateCount = 0;
				foreach (List<FindAutoUpdateFilesResult> pendingUpdates in PendingUpdateList.Values)
				{
					pendingUpdateCount += pendingUpdates.Count;
				}

				return pendingUpdateCount;
			}
		}
	}
}
