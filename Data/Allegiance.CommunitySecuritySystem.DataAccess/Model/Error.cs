using System;
using System.Configuration;
using System.Text;
using Allegiance.CommunitySecuritySystem.Common.Utility;
using System.Diagnostics;
using System.Linq;

namespace Allegiance.CommunitySecuritySystem.DataAccess
{
    public partial class Error
    {
		public static void Clean()
		{
			using (var db = new CSSDataContext())
			{
				db.Errors.DeleteAllOnSubmit(db.Errors.Where(p => p.DateOccurred < DateTime.Now.AddDays(-30)));
				db.SubmitChanges();
			}
		}

        public static bool Write(Exception exception)
        {
            using (var db = new CSSDataContext())
                return Write(db, exception, true);
        }

        public static bool Write(CSSDataContext db, Exception exception)
        {
            return Write(db, exception, true);
        }

        public static bool Write(CSSDataContext db, Exception exception, bool submit)
        {
            try
            {
                string innerMessage = null;
                if (exception.InnerException != null)
                    innerMessage = exception.InnerException.Message;

                db.Errors.InsertOnSubmit(new Error()
                {
                    DateOccurred    = DateTime.Now,
                    ExceptionType   = exception.GetType().Name,
                    Message         = exception.Message,
                    StackTrace      = exception.StackTrace,
                    InnerMessage    = innerMessage
                });

                if (submit)
                    db.SubmitChanges();

                return true;
            }
            catch (Exception error)
            {
                var message = new StringBuilder()
                    .AppendLine("Failed to log error:").AppendLine(exception.ToString())
                    .AppendLine()
                    .AppendLine("Due to error:").AppendLine(error.ToString());

                try
                {
					Debug.WriteLine(message);

                    MailManager.SendMessage("Attempt to log error failed.",
                        message.ToString(),
                        ConfigurationManager.AppSettings["ErrorRecipients"]);
                }
                catch { 
                 /* TODO: Log to static file */
                }
                return false;
            }
        }
    }
}