using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;

namespace Allegiance.CommunitySecuritySystem.Management.HttpHandlers
{
	/// <summary>
	/// Summary description for $codebehindclassname$
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class BlockExecution : IHttpHandler
	{

		public void ProcessRequest(HttpContext context)
		{
			string targetFile = context.Server.MapPath(context.Request.AppRelativeCurrentExecutionFilePath);

			if (File.Exists(targetFile) == true)
			{
				context.Response.Write(File.ReadAllText(targetFile));
				context.Response.ContentType = "text/plain";
			}
			else
			{
				context.Response.StatusCode = 404;
				context.Response.End();
			}
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}
