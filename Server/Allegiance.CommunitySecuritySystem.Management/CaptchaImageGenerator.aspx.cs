using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Allegiance.CommunitySecuritySystem.DataAccess;
using System.IO;
using System.Drawing.Imaging;
using System.Threading;

namespace Allegiance.CommunitySecuritySystem.Management
{
	public partial class CaptchaImageGenerator : System.Web.UI.Page
	{
		private static Object _syncObject = new Object(); 

		protected void Page_Load(object sender, EventArgs e)
		{
			byte[] imageData = null;
			int width = 200;
			int height = 100;

			// Prevent people from slamming this page.
			lock (_syncObject)
			{
				Thread.Sleep(250);
			}

			if (string.IsNullOrEmpty(Request.Params["token"]) == false
				&& string.IsNullOrEmpty(Request.Params["width"]) == false
				&& string.IsNullOrEmpty(Request.Params["height"]) == false
				&& Int32.TryParse(Request.Params["width"], out width) == true
				&& Int32.TryParse(Request.Params["height"], out height) == true)
			{
				Guid captchaToken = new Guid(Request.Params["token"]);

				using (CSSDataContext db = new CSSDataContext())
				{
					var captcha = db.Captchas.FirstOrDefault(p => p.Id == captchaToken);
					if (captcha != null)
					{
						Allegiance.CommunitySecuritySystem.Common.Utility.CaptchaImage captchaImage = new Common.Utility.CaptchaImage(captcha.Answer, width, height);
						MemoryStream imageStream = new MemoryStream();
						captchaImage.Image.Save(imageStream, ImageFormat.Jpeg);
						imageData = imageStream.GetBuffer();
					}
				}
			}

			if (imageData == null)
				imageData = File.ReadAllBytes(Server.MapPath("~/Images/dg_banned.png"));

			Response.ContentType = "image/jpeg";
			Response.AppendHeader("Content-Length", imageData.Length.ToString());
			Response.BinaryWrite(imageData);
			Response.End();
		}
	}
}