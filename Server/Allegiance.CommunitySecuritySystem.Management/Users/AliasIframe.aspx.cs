using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Allegiance.CommunitySecuritySystem.Management.Users
{
	/// <summary>
	/// There were too many issues with dynamically generating the alias tabs and 
	/// having the post back events wire up correctly, so just going to iframe that 
	/// for now, maybe revist it later.
	/// </summary>
	public partial class AliasIframe : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			int aliasID;

			if (Int32.TryParse(Request.Params["aliasID"], out aliasID) == false)
				throw new Exception("Must specify aliasID.");

			ucAliasDetail.AliasID = aliasID;
		}
	}
}
