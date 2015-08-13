using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace Allegiance.CommunitySecuritySystem.Management.Content.Motd
{
	public partial class Default : UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			//StringWriter sw = new StringWriter();
			//HtmlTextWriter htw = new HtmlTextWriter(sw);

			//Context.Server.Execute("MotdTemplate.aspx", sw, true);

			
			//Page.ClientScript.RegisterOnSubmitStatement(this.GetType(), "modificationCheck", "if (modificationCheck() == false) return false;");

			//if (Request.Params["__EVENTTARGET"] == ddlLobby.UniqueID)
			//	ddlLobby_SelectedIndexChanged(ddlLobby, EventArgs.Empty);

			if (this.IsPostBack == false)
				BindData(null);
		}

		private void BindData(int? selectedLobby)
		{
			string motdDirectory = Server.MapPath("~/Images/Motd");

			ddlLogo.Items.Clear();
			foreach(string filename in Directory.GetFiles(motdDirectory, "*.png"))
			{
				ddlLogo.Items.Add(new ListItem()
				{
					Text = Path.GetFileNameWithoutExtension(filename),
					Value = Path.GetFileNameWithoutExtension(filename)
				});
			}

			using (DataAccess.CSSDataContext db = new DataAccess.CSSDataContext())
			{
				ddlLobby.DataSource = db.Lobbies.ToList();
				ddlLobby.DataValueField = "Id";
				ddlLobby.DataTextField = "Name";
				ddlLobby.DataBind();

				if (selectedLobby != null)
					ddlLobby.SelectedValue = selectedLobby.Value.ToString();

				var motdSettings = db.Motds.FirstOrDefault(p => p.LobbyId == Int32.Parse(ddlLobby.SelectedValue));

				if (motdSettings != null)
				{
					txtBanner.Text = motdSettings.Banner;
					txtDetails.Text = motdSettings.Details;
					txtPaddingCrCount.Text = motdSettings.PaddingCrCount.ToString();
					txtPrimaryHeading.Text = motdSettings.PrimaryHeading;
					txtPrimaryText.Text = motdSettings.PrimaryText;
					txtSecondaryHeading.Text = motdSettings.SecondaryHeading;
					txtSecondaryText.Text = motdSettings.SecondaryText;
					ddlLogo.SelectedValue = motdSettings.Logo;
				}
				else
				{
					txtBanner.Text = String.Empty;
					txtDetails.Text = String.Empty;
					txtPrimaryHeading.Text = String.Empty;
					txtPrimaryText.Text = String.Empty;
					txtSecondaryHeading.Text = String.Empty;
					txtSecondaryText.Text = String.Empty;

					txtPaddingCrCount.Text = "28";
				}
			}

			//IEnumerable<Data.ImageItem> images =  Directory.GetFiles(motdDirectory, "*.png").Select(p => new Data.ImageItem()
			//{
			//    ImageUrl = Page.ResolveClientUrl("~/Images/Motd/" + Path.GetFileName(p)),
			//    ImageName = Path.GetFileNameWithoutExtension(p)
			//});

			//rptImagePick.DataSource = images;
			//rptImagePick.DataBind();

			lblUpdateStatus.Visible = false;
			
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			int lobbyID = Int32.Parse(ddlLobby.SelectedValue);

			using (DataAccess.CSSDataContext db = new DataAccess.CSSDataContext())
			{
				var motdSettings = db.Motds.FirstOrDefault(p => p.LobbyId == lobbyID);

				if (motdSettings == null)
				{
					motdSettings = new DataAccess.Motd();
					db.Motds.InsertOnSubmit(motdSettings);

					motdSettings.LobbyId = lobbyID;
				}

				motdSettings.Banner = txtBanner.Text;
				motdSettings.Details = txtDetails.Text;
				motdSettings.LastUpdated = DateTime.Now;
				motdSettings.Logo = ddlLogo.SelectedValue;
				motdSettings.PaddingCrCount = Int32.Parse(txtPaddingCrCount.Text);
				motdSettings.PrimaryHeading = txtPrimaryHeading.Text;
				motdSettings.PrimaryText = txtPrimaryText.Text;
				motdSettings.SecondaryHeading = txtSecondaryHeading.Text;
				motdSettings.SecondaryText = txtSecondaryText.Text;

				db.SubmitChanges();
			}

			lblUpdateStatus.Visible = true;
		}

		protected void ddlLobby_SelectedIndexChanged(object sender, EventArgs e)
		{
			BindData(Int32.Parse(ddlLobby.SelectedValue));
		}



	

		//protected void rptImagePick_ItemDataBound(object sender, RepeaterItemEventArgs e)
		//{
		//    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		//    {
		//        Image imgBackground = (Image)e.Item.FindControl("imgBackground");
		//        Data.ImageItem dataItem = (Data.ImageItem)e.Item.DataItem;

		//        imgBackground.ImageUrl = dataItem.ImageUrl;
		//    }
			
		//}


		//protected void imgBackground_OnDataBinding(object sender, EventArgs e)
		//{

		//}
		
	}
}
