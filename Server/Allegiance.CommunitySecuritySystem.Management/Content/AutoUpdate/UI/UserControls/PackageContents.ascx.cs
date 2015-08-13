using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Allegiance.CommunitySecuritySystem.Management.Business;
using System.IO;

namespace Allegiance.CommunitySecuritySystem.Management.Content.AutoUpdate.UI.UserControls
{
	public partial class PackageContents : System.Web.UI.UserControl
	{
		public string Target { get; set; }
		public bool ShowDelete = true;

		protected void Page_Load(object sender, EventArgs e)
		{
			//if (this.IsPostBack == false)
				BindData();
		}

		public void BindData()
		{
			List<UpdateItem> updateItems = AutoUpdateManager.GetFilesInUpdatePackage(Target).OrderBy(p => p.RelativeDirectory).ThenBy(p => p.Name).ToList();

			foreach (UpdateItem updateItem in updateItems)
				updateItem.RelativeDirectory = updateItem.RelativeDirectory.Replace("\\", "\\\\");

			gvPackageFiles.Visible = updateItems.Count() > 0;

			if(ShowDelete == false)
			{
				foreach(DataControlField c in gvPackageFiles.Columns)
				{
					if (c.HeaderText == "Delete")
						c.Visible = false;
				}
			}

			gvPackageFiles.DataSource = updateItems;
			gvPackageFiles.DataBind();
		}

		protected void txtFileToDelete_ValueChanged(object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(txtFileToDelete.Value) == true)
				return;

			AutoUpdateManager.DeleteFileFromPackage(Target, txtFileToDeleteRelativeDirectory.Value, txtFileToDelete.Value);

			txtFileToDelete.Value = "";

			BindData();
		}
	}
}