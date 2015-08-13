using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Allegiance.CommunitySecuritySystem.Management.Business
{
	public class UpdateItem
	{
		public bool IsIncluded { get; set; }
		public string Name { get; set; }
		public DateTime LastModified { get; set; }
		public DateTime DateCreated { get; set; }
		public bool IsProtected { get; set; }
		public FileInfo FileInfo { get; set; }
		public string PackageName { get; set; }
		public string RelativeDirectory { get; set; }

		public UpdateItem(string packageName, string filename, string relativeDirectory, FileInfo fileInfo, bool isIncluded, bool isProtected)
		{
			PackageName = packageName;
			IsIncluded = isIncluded;
			Name = filename;
			LastModified = fileInfo.LastWriteTime;
			DateCreated = fileInfo.CreationTime;
			IsProtected = isProtected;
			FileInfo = fileInfo;
			RelativeDirectory = relativeDirectory;
		}
	}
}
