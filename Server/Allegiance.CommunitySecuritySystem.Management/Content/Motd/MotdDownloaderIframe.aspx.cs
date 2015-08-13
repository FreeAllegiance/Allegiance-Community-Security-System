using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace Allegiance.CommunitySecuritySystem.Management.Content.Motd
{
	public partial class MotdDownloaderIframe : UI.Page
	{
		private int LobbyID
		{
			get { return Int32.Parse(Request.Params["lobbyID"]); }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			StringWriter output = new StringWriter();

			Server.Execute("MotdTemplate.aspx?lobbyID=" + LobbyID, output);

			string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			Directory.CreateDirectory(tempDirectory);

			string tempFile = Path.Combine(tempDirectory, "publicmessageoftheday.mdl"); 
			File.WriteAllText(tempFile, output.ToString());

			ICSharpCode.SharpZipLib.Zip.FastZip fastZip = new ICSharpCode.SharpZipLib.Zip.FastZip();

			string zipFilename = Path.GetTempFileName();

			fastZip.CreateZip(zipFilename, tempDirectory, false, String.Empty);

			byte[] outputBytes = File.ReadAllBytes(zipFilename);

			Directory.Delete(tempDirectory, true);

			File.Delete(zipFilename);

			Response.ContentType = "application/zip";
			Response.AddHeader("Content-Disposition", "attachment; filename=publicmessageoftheday.zip");
			Response.AddHeader("ContentLength", outputBytes.Length.ToString());

			Response.BinaryWrite(outputBytes);
		}

		protected override void Render(HtmlTextWriter writer)
		{

		}
	}
}
