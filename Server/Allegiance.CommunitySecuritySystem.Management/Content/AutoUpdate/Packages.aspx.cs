using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Allegiance.CommunitySecuritySystem.Management.Business;
using System.IO;

namespace Allegiance.CommunitySecuritySystem.Management.Content.AutoUpdate
{
	public partial class Packages : UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.IsPostBack == false)
				BindData();
		}

		private void BindData()
		{
			List<FileInfo> packageInfos = AutoUpdateManager.GetPackages();
			List<Data.EditablePackage> packages = new List<Data.EditablePackage>();

			foreach(FileInfo packageInfo in packageInfos)
			{
				packages.Add(new Data.EditablePackage()
				{
					DateCreated = Format.DateTime(packageInfo.CreationTime),
					LastModified = Format.DateTime(packageInfo.LastWriteTime),
					Name = packageInfo.Name
				});
			}

			gvPackages.DataSource = packages;
			gvPackages.DataBind();
		}

		protected void btnNewPackage_Click(object sender, EventArgs e)
		{
			Response.Redirect("EditPackage.aspx");
		}
	}
}
