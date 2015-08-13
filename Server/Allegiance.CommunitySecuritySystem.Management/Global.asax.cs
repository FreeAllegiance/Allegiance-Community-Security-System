using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Net.Configuration;
using System.Configuration;
using Allegiance.CommunitySecuritySystem.Management.Properties;
using Allegiance.CommunitySecuritySystem.Management.Business;
using System.Reflection;
using System.Text;
using System.Collections;
using Allegiance.CommunitySecuritySystem.DataAccess;
using Allegiance.CommunitySecuritySystem.Common.Utility;

namespace Allegiance.CommunitySecuritySystem.Management
{
	public class Global : System.Web.HttpApplication
	{

		protected void Application_Start(object sender, EventArgs e)
		{

		}

		protected void Session_Start(object sender, EventArgs e)
		{

		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{

		}

		protected void Application_Error(object sender, EventArgs e)
		{
			Exception ex = Context.Error;
			if (ex is HttpUnhandledException)
				ex = Context.Error.InnerException;

			MailMessage mail = new MailMessage();

			foreach(string mailAddress in Settings.Default.AdminEmails.Split(','))
				mail.To.Add(new MailAddress(mailAddress));

			mail.IsBodyHtml = true;

			mail.Body = "<html><body>";
			mail.Body += FormatObjectToHtml("CSS Exception", ex);

			mail.Body += FormatObjectToHtml("HttpContext.Current.Request", Request);
			mail.Body += "</body></html>";

			if (mail.To.Count > 0)
			{
				mail.Subject = "CSS Exception: " + ex.Message;

				MailManager.SendMailMessage(mail);
			}

			using (CSSDataContext db = new CSSDataContext())
			{
				db.Errors.InsertOnSubmit(new Error()
				{
					DateOccurred = DateTime.Now, 
					ExceptionType = ex.GetType().Name,
					Id = 0,
					InnerMessage = ex.InnerException == null ? String.Empty : ex.InnerException.Message,
					Message = ex.Message,
					StackTrace = ex.StackTrace.ToString()
				});

				db.SubmitChanges();
			}
		}


		private string FormatObjectToHtml(string blockTitle, Object obj)
		{
			StringBuilder html = new StringBuilder();
			html.AppendFormat("<table style='border: 1px solid black; word-wrap: break-word;' cellpadding='10' cellspacing='0' rowspacing='1'><tr><td colspan='2'><h3>{0}</h3></td></tr>", blockTitle);

			foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
			{
				if (propertyInfo.PropertyType.IsArray == true)
					continue;

				if (propertyInfo.PropertyType.IsPublic == false)
					continue;

				if (propertyInfo.GetIndexParameters().Count() > 0)
					continue;

				try
				{
					object propertyValue = propertyInfo.GetValue(obj, null);

					if (propertyValue != null)
					{
						propertyValue = propertyValue.ToString().Replace("\n", "<br />");

						html.AppendFormat("<tr valign='top'><td style='border-bottom: 1px solid black; text-align: right;'><div style='width: 200px;'>{0}</div></td><td style='border-bottom: 1px solid black;'><div style='width: 700px;'>{1}</div></td></tr>", propertyInfo.Name, propertyValue);
					}
				}
				catch
				{
					html.AppendFormat("<tr valign='top'><td style='border-bottom: 1px solid black; text-align: right;'><div style='width: 200px;'>{0}</div></td><td style='border-bottom: 1px solid black;'><div style='width: 700px;'>{1}</div></td></tr>", propertyInfo.Name, "Error getting value.");
				}
			}

			html.Append("</table><br /><br /><br />");

			return html.ToString();
		}

		protected void Session_End(object sender, EventArgs e)
		{

		}

		protected void Application_End(object sender, EventArgs e)
		{

		}
	}
}