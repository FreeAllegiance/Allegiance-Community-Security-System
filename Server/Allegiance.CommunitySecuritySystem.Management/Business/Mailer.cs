using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Allegiance.CommunitySecuritySystem.Management.Properties;
using System.Net.Mail;

namespace Allegiance.CommunitySecuritySystem.Management.Business
{
	public class Mailer
	{
		public static void SendMail(MailMessage mailMessage)
		{
			System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient();

			if (Settings.Default.UseSSLMailTransport == true)
				smtpMail.EnableSsl = true;

			smtpMail.Send(mailMessage);
		}
	}
}
