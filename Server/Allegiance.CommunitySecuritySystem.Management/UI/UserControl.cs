using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Allegiance.CommunitySecuritySystem.Management.UI
{
	public class UserControl : System.Web.UI.UserControl
	{
		public new Page Page
		{
			get
			{
				if (base.Page is UI.Page)
					return (UI.Page)base.Page;

				throw new Exception("Couldn't cast page of type: " + base.Page.GetType().FullName + " to UI.Page.");
			}
		}
	}
}
