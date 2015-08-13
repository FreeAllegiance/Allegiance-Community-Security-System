using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Allegiance.CommunitySecuritySystem.Management.Business;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;

namespace Allegiance.CommunitySecuritySystem.Management.Content.AutoUpdate
{
	public partial class EditPublication : UI.Page
	{
		protected bool PublicationDeployed = false;
		protected bool PublicationDeployFailed = false;

		private int PublicationID
		{
			get { return Int32.Parse(this.Target); }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.IsPostBack == false)
				BindData();

			PublicationDeployed = String.IsNullOrEmpty(Request.Params["publicationDeployed"]) == false;
			PublicationDeployFailed = String.IsNullOrEmpty(Request.Params["publicationDeployFailed"]) == false;
		}

		private void BindData()
		{
			using (var db = new DataAccess.CSSDataContext())
			{
				DataAccess.Lobby lobby = db.Lobbies.FirstOrDefault(p => p.Id == PublicationID);

				txtBasePath.Text = lobby.BasePath;
				txtHost.Text = lobby.Host;
				txtLobbyName.Text = lobby.Name;
				chkEnabled.Checked = lobby.IsEnabled;
				chkRestrictive.Checked = lobby.IsRestrictive;
			}


			List<FileInfo> packageInfos = AutoUpdateManager.GetPackages();

			List<Data.EditablePublication> packages = new List<Data.EditablePublication>();

			foreach (FileInfo packageInfo in packageInfos)
			{
				packages.Add(new Data.EditablePublication()
				{
					IsIncluded = AutoUpdateManager.IsPackageExcludedFromPublication(PublicationID, packageInfo.Name) == false,
					Name = packageInfo.Name
				});
			}

			gvPackages.DataSource = packages;
			gvPackages.DataBind();
		}

		protected void chkIncluded_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox checkbox = (CheckBox)sender;

			GridViewRow gridViewRow = (GridViewRow)checkbox.Parent.Parent;
			string fileName = gridViewRow.Cells[0].Text;

			AutoUpdateManager.SetPackageInclusionForPublication(PublicationID, fileName, checkbox.Checked);

			BindData();
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			Response.Redirect("Publish.aspx", true);
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			using (var db = new DataAccess.CSSDataContext())
			{
				DataAccess.Lobby lobby = db.Lobbies.FirstOrDefault(p => p.Id == PublicationID);

				lobby.Host = txtHost.Text.Trim();
				lobby.BasePath = txtBasePath.Text.Trim();
				lobby.IsEnabled = chkEnabled.Checked;
				lobby.IsRestrictive = chkRestrictive.Checked;

				db.SubmitChanges();
			}

			BindData();
		}

		protected void btnDownloadPublication_Click(object sender, EventArgs e)
		{
			Response.Redirect("DownloadOrDeployPublication.aspx?target=" + PublicationID, true);
		}

		protected void btnDeployPublication_Click(object sender, EventArgs e)
		{
			Response.Redirect("DownloadOrDeployPublication.aspx?target=" + PublicationID + "&deployPublication=1", true);

			//if (AutoUpdateManager.DeployPublication(PublicationID) == true)
			//    PublicationDeployed = true;
			//else
			//    PublicationDeployFailed = true;
		}
	}
}
