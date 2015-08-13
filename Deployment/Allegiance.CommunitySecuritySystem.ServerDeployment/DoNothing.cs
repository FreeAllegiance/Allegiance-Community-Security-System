using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegiance.CommunitySecuritySystem.LobbyDeployment
{
	/// <summary>
	///  This class only exists to enable the post build job to run. 
	///  The post build job then assembles the proper files from the rest of the projects 
	///  into a deployment archive directory. This can then be made part of a 
	///  cruise control job to generate nightly code drops.
	/// </summary>
	public class DoNothing
	{
	}
}
