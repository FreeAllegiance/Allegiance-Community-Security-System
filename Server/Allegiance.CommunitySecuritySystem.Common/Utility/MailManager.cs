using System;
using System.Net.Mail;

namespace Allegiance.CommunitySecuritySystem.Common.Utility
{
    public static class MailManager
    {
        public static void SendMessage(string subject, string body, params string[] recipient)
        {
            var msg = new MailMessage()
            {
                Subject = subject,
                Body    = body
            };

            //Split recipients
            foreach (var r in recipient)
            {
                var recipients = r.Split(new char[] {';',','}, StringSplitOptions.RemoveEmptyEntries);
                foreach (var i in recipients)
                    msg.Bcc.Add(new MailAddress(i.Trim()));
            }

			SendMailMessage(msg);
        }

		public static void SendMailMessage(MailMessage mailMessage)
		{
			System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient();

			if (Settings.Default.UseSSLMailTransport == true)
				smtpMail.EnableSsl = true;

			smtpMail.Send(mailMessage);
		}
    }
}