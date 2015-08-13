using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Allegiance.CommunitySecuritySystem.Management.Business;
using System.Security;

namespace Allegiance.CommunitySecuritySystem.Management.Content.AutoUpdate.UI
{
	public class Page : Content.UI.Page
	{
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			Master.PageHeader = "CSS - Auto Update Manager";

			if (Business.Authorization.IsAdminOrSuperAdmin(User) == false)
				Response.Redirect("~/Default.aspx");

			if (Master.Breadcrumb != null)
				Master.Breadcrumb.Visible = true;
		}

		private string _target = null;
		protected string Target
		{
			get
			{
				if (_target == null)
				{
					_target = Request.Params["target"] ?? String.Empty;

					if (String.IsNullOrEmpty(_target) == false)
					{
						if (AutoUpdateManager.IsFilenameOrDirectorySafe(_target) == false)
							throw new SecurityException("target cannot contain pathing symbols.");
					}
				}

				return _target;
			}
		}
	}
}
