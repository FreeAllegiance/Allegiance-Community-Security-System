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
	public partial class PublicationDownloaderIframe : UI.Page
	{
		//private byte[] applicationData = null;


		private int PublicationID
		{
			get { return Int32.Parse(this.Target); }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			List<FileCollision> fileCollisions = new List<FileCollision>();
			Dictionary<string, UpdateItem> filesInPublication = new Dictionary<string, UpdateItem>();

			AutoUpdateManager.TryGetPublicationFiles(PublicationID, out filesInPublication, out fileCollisions);
			
			string publicationName = AutoUpdateManager.GetPublicationName(PublicationID);

			string tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			Directory.CreateDirectory(tempPath);

			foreach (string filename in filesInPublication.Keys)
			{
				string targetFile = Path.Combine(tempPath, filename);

				if(Directory.Exists(Path.GetDirectoryName(targetFile)) == false)
					Directory.CreateDirectory(Path.GetDirectoryName(targetFile));


				File.Copy(filesInPublication[filename].FileInfo.FullName, targetFile, true);
			}

			ICSharpCode.SharpZipLib.Zip.FastZip fastZip = new ICSharpCode.SharpZipLib.Zip.FastZip();

			string zipFilename = Path.GetTempFileName();

			fastZip.CreateZip(zipFilename, tempPath, true, String.Empty);

			byte[] outputBytes = File.ReadAllBytes(zipFilename);

			Directory.Delete(tempPath, true);

			File.Delete(zipFilename);


			Response.ContentType = "application/zip";
			Response.AddHeader("Content-Disposition", "attachment; filename=" + publicationName + ".zip");
			Response.AddHeader("ContentLength", outputBytes.Length.ToString());


			Response.BinaryWrite(outputBytes);


		}

		protected override void Render(HtmlTextWriter writer)
		{
			
		}
	}
}
