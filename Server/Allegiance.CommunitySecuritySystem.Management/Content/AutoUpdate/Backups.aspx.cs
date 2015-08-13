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
	public partial class Backups : UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.IsPostBack == false)
				BindData();
		}

		private void BindData()
		{
			List<Data.BackupItem> backupItems = new List<Allegiance.CommunitySecuritySystem.Management.Content.AutoUpdate.Data.BackupItem>();

			List<FileInfo> backupInfos = AutoUpdateManager.GetAvailableBackups();
			foreach (FileInfo backupInfo in backupInfos)
			{
				backupItems.Add(new Data.BackupItem()
				{
					Name = Path.GetFileName(backupInfo.Name),
					DateCreated = Format.DateTime(backupInfo.CreationTime),
					LastModified = Format.DateTime(backupInfo.LastWriteTime)
				});
			}

			if (backupItems.Count > 0)
			{
				gvBackups.DataSource = backupItems;
				gvBackups.DataBind();
			}
			else
			{
				lblNoBackups.Visible = true;
				gvBackups.Visible = false;
			}
		}

		protected void btnCreate_Click(object sender, EventArgs e)
		{
			
		}

		protected void gvBackups_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			gvBackups.PageIndex = e.NewPageIndex;
			BindData();
		}

		
	}
}
