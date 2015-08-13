using System;
using System.Web;
using System.Text;
using System.IO;

namespace Allegiance.CommunitySecuritySystem.Server
{
	public class PhpRequestLoggerModule : IHttpModule
	{
		/// <summary>
		/// You will need to configure this module in the web.config file of your
		/// web and register it with IIS before being able to use it. For more information
		/// see the following link: http://go.microsoft.com/?linkid=8101007
		/// </summary>
		#region IHttpModule Members

		public void Dispose()
		{
			//clean-up code here.
		}

		public void Init(HttpApplication context)
		{
			context.BeginRequest += new EventHandler(context_BeginRequest);
			
		}

		void context_BeginRequest(object sender, EventArgs e)
		{
			var context = HttpContext.Current;

			var streamWriter = new StreamWriter(@"c:\php_requests.log", true);

			streamWriter.WriteLine("\r\n\r\n" + DateTime.Now.ToString() + " Request");
			streamWriter.WriteLine("========================================================");

			streamWriter.WriteLine("Request.Form Vars");
			foreach (string key in context.Request.Form.AllKeys)
			{
				streamWriter.WriteLine("\t" + key + ": " + context.Request.Form[key]);
			}

			streamWriter.WriteLine("Request.RawUrl: " + context.Request.RawUrl);

			if (context.Request.InputStream.Length > 0)
			{
				byte[] buffer = new byte[context.Request.InputStream.Length];
				context.Request.InputStream.Read(buffer, 0, (int)context.Request.InputStream.Length);
				context.Request.InputStream.Position = 0;

				string inputStreamContents = ASCIIEncoding.ASCII.GetString(buffer);

				streamWriter.WriteLine("context.Request.InputStream Length: " + context.Request.InputStream.Length);
				streamWriter.WriteLine(inputStreamContents + "\r\n\r\n");
			}


			streamWriter.Close();
		}

		#endregion
	}
}
