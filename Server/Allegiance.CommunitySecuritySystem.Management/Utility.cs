using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Win32;
using System.IO;

namespace Allegiance.CommunitySecuritySystem.Management
{
	public class Utility
	{
		// Shamelessly cribbed from:
		// http://stackoverflow.com/questions/58510/using-net-how-can-you-find-the-mime-type-of-a-file-based-on-the-file-signature
		public static string GetMimeType(string fileExtension)
		{
			string mimeType = "application/unknown";

			RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(
				fileExtension.ToLower()
				);

			if (regKey != null)
			{
				object contentType = regKey.GetValue("Content Type");

				if (contentType != null)
					mimeType = contentType.ToString();
			}

			return mimeType;
		}

	}
}
