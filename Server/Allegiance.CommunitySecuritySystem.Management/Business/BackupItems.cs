using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Allegiance.CommunitySecuritySystem.Management.Business
{
	public class BackupItems
	{
		public readonly List<FileInfo> PublicationFiles = new List<FileInfo>();
		public readonly List<UpdateItem> PackageFiles = new List<UpdateItem>();
	}
}
