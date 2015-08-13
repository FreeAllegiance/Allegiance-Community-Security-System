using System;
using System.Collections.Generic;
using System.Text;

namespace Allegiance.CommunitySecuritySystem.UtilityTest
{
	class Program
	{
		static void Main(string[] args)
		{
			UtilityWrapper.CRCUtils c = new UtilityWrapper.CRCUtils();

			string error = String.Empty;
			Console.WriteLine(c.GetCrc32ForFile(args[0], ref error));
			Console.WriteLine(error);
		}
	}
}
