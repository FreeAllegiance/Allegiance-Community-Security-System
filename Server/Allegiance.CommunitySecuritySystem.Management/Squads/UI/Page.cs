using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Allegiance.CommunitySecuritySystem.Management.Squads.UI
{
	public class Page : Management.UI.Page
	{

		protected override void OnLoad(EventArgs e)
		{
			Master.PageHeader = "CSS - Squad Manager";

			base.OnLoad(e);
		}


		private string _target = null;
		protected string Target
		{
			get
			{
				if (_target == null)
				{
					_target = Request.Params["target"] ?? String.Empty;
				}

				return _target;
			}
		}
	}
}
