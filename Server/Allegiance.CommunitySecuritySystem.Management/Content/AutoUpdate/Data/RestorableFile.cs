using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Allegiance.CommunitySecuritySystem.Management.Content.AutoUpdate.Data
{
	public class RestorableFile : FileInfoBase
	{
		public string Type { get; set; }
		public string Container{ get; set; }
		public string Path { get; set; }
		public string IncludedImage { get; set; }
		public bool Selected { get; set; }
		public string ProtectedImage { get; set; }
		public string RelativeDirectory { get; set; }
	}
}
