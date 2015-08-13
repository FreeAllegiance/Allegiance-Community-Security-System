using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Allegiance.CommunitySecuritySystem.Management.Business;
using System.IO;
using Allegiance.CommunitySecuritySystem.Management.Content.AutoUpdate.Data;

namespace Allegiance.CommunitySecuritySystem.Management.Content.AutoUpdate
{
	public partial class DownloadOrDeployPublication : UI.Page
	{
		protected string PublicationDownloaderUrl;

		protected int PublicationID
		{
			get { return Int32.Parse(this.Target); }
		}

		private bool DeployPublication
		{
			get
			{
				return String.IsNullOrEmpty(Request.Params["deployPublication"]) == false;
			}
		}

		private string PackageName
		{
			get { return Request.Params["packageName"]; }
		}


		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.IsPostBack == false)
				BindData();
		}

		private void BindData()
		{
			List<FileCollision> fileCollisions = new List<FileCollision>();
			Dictionary<string, UpdateItem> filesInPublication = new Dictionary<string, UpdateItem>();

			AutoUpdateManager.TryGetPublicationFiles(PublicationID, out filesInPublication, out fileCollisions);

			if (fileCollisions.Count > 0)
			{
				pFileCollisions.Visible = true;
				pNoFileCollisions.Visible = false;

				pPublishMergedPublications.Visible = DeployPublication;
				pDownloadMergedPublications.Visible = DeployPublication == false;

				gvCollisions.DataSource = fileCollisions;
				gvCollisions.DataBind();
			}
			else
			{
				pNoFileCollisions.Visible = false;
				pFileCollisions.Visible = false;

				if (DeployPublication == true)
				{
					btnDeployPublication_Click(this, EventArgs.Empty);
				}
				else
				{
					pNoFileCollisions.Visible = true;
				}
			}

			PublicationDownloaderUrl = "PublicationDownloaderIframe.aspx?target=" + PublicationID;

		}

		protected void btnDeployPublication_Click(object sender, EventArgs e)
		{
			if (AutoUpdateManager.DeployPublication(PublicationID) == true)
			{
				if(String.IsNullOrEmpty(PackageName) == true)
					Response.Redirect("EditPublication.aspx?target=" + PublicationID + "&publicationDeployed=1", true);
				else
					Response.Redirect("EditPackage.aspx?target=" + PackageName + "&publicationDeployed=1", true);
			}
			else
			{
				if (String.IsNullOrEmpty(PackageName) == true)
					Response.Redirect("EditPublication.aspx?target=" + PublicationID + "&publicationDeployFailed=1", true);
				else
					Response.Redirect("EditPackage.aspx?target=" + PackageName + "&publicationDeployFailed=1", true);
			}
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			Response.Redirect("EditPublication.aspx?target=" + PublicationID, true);
		}

	}
}
