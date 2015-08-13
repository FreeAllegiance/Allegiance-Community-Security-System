using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace Allegiance.CommunitySecuritySystem.Management.Content.Motd
{
	public partial class MotdPreviewIframe : System.Web.UI.Page
	{
		//protected string LogoUrl;

		protected void Page_Load(object sender, EventArgs e)
		{
			int lobbyID;

			if (String.IsNullOrEmpty(Request.Params["lobbyID"]) == true || Int32.TryParse(Request.Params["lobbyID"], out lobbyID) == false)
				throw new Exception("Invalid lobby id.");

			using (DataAccess.CSSDataContext db = new DataAccess.CSSDataContext())
			{
				var motd = db.Motds.FirstOrDefault(p => p.LobbyId == lobbyID);

				if (motd == null)
					throw new Exception("Lobby id out of range.");

				//LogoUrl = Page.ResolveUrl("~/Images/motd/" + motd.Logo + ".png");
				imgLogo.ImageUrl = "~/Images/motd/" + motd.Logo + ".png";

				lblBanner.Text = FormatLineBreaks(motd.Banner);
				lblDetails.Text = FormatLineBreaks(motd.Details);
				lblPadding.Text = GeneratePadding(motd.PaddingCrCount);
				lblPrimaryHeading.Text = FormatLineBreaks(motd.PrimaryHeading);
				lblPrimaryText.Text = FormatLineBreaks(motd.PrimaryText);
				lblSecondaryHeading.Text = FormatLineBreaks(motd.SecondaryHeading);
				lblSecondaryText.Text = FormatLineBreaks(motd.SecondaryText);
				lblUpdated.Text = FormatLineBreaks(motd.LastUpdated.ToShortDateString());
			}
		}

		private string FormatLineBreaks(string textToFormat)
		{
			return textToFormat.Replace("\n", "<br />");
		}

		private string GeneratePadding(int crlfCount)
		{
			StringBuilder padding = new StringBuilder();

			for (int i = 0; i < crlfCount; i++)
			{
				padding.Append("<br />");
			}

			return padding.ToString();
		}
	}

	
}
