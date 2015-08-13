using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Allegiance.CommunitySecuritySystem.Management.Content.Motd
{
	
	public partial class MotdTemplate : System.Web.UI.Page
	{
		protected string LogoImage;
		protected string PaddingCrLfs;
		protected string LastUpdated;
		protected string PrimaryHeading;
		protected string PrimaryText;
		protected string SecondaryHeading;
		protected string SecondaryText;
		protected string Details;
		protected string Banner;


		protected void Page_Load(object sender, EventArgs e)
		{
			int lobbyID = Int32.Parse(Request.Params["lobbyID"]);

			using(DataAccess.CSSDataContext db = new Allegiance.CommunitySecuritySystem.DataAccess.CSSDataContext())
			{
				var motd = db.Motds.FirstOrDefault(p => p.LobbyId == lobbyID);

				if(motd == null)
					throw new Exception("No motd for lobby id: " + lobbyID);

				LogoImage = FixupCrlf(motd.Logo);
				Banner = FixupCrlf(motd.Banner);
				LastUpdated = motd.LastUpdated.ToShortDateString();
				PrimaryHeading = FixupCrlf(motd.PrimaryHeading);
				PrimaryText = FixupCrlf(motd.PrimaryText);
				SecondaryHeading = FixupCrlf(motd.SecondaryHeading);
				SecondaryText = FixupCrlf(motd.SecondaryText);
				Details = FixupCrlf(motd.Details);

				PaddingCrLfs = String.Empty;
				for (int i = 0; i < motd.PaddingCrCount; i++)
					PaddingCrLfs += "\\n";
			}
		}

		private string FixupCrlf(string stringToFix)
		{
			return stringToFix.Replace("\r\n", "\\n").Replace("\n", "\\n");
		}
	}

	
}
