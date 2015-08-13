using System;
using System.Text;
using Allegiance.CommunitySecuritySystem.DataAccess.Enumerations;
using System.Diagnostics;
using System.Linq;

namespace Allegiance.CommunitySecuritySystem.DataAccess
{
    public partial class Log
    {
		public static void Clean()
		{
			using (var db = new CSSDataContext())
			{
				db.Logs.DeleteAllOnSubmit(db.Logs.Where(p => p.DateOccurred < DateTime.Now.AddDays(-30)));
				db.SubmitChanges();
			}
		}


        public static bool Write(LogType type, string message)
        {
            using (var db = new CSSDataContext())
                return Write(db, type, message, true);
        }

        public static bool Write(CSSDataContext db, LogType type, string message)
        {
            return Write(db, type, message, true);
        }

        public static bool Write(CSSDataContext db, LogType type, string message, bool submit)
        {
            try
            {
                db.Logs.InsertOnSubmit(new Log()
                {
                    DateOccurred = DateTime.Now,
                    Message     = message,
                    Type        = (byte)type
                });

                if (submit)
                    db.SubmitChanges();

				Console.WriteLine(String.Format("{0}: {1} {2}", DateTime.Now, type.ToString(), message));

                return true;
            }
            catch(Exception error)
            {
                var errorMessage = new StringBuilder()
                    .AppendLine("Failed to write data to log:")
                    .Append("Type: ").AppendLine(type.ToString())
                    .Append("Message: ").AppendLine(message);

				Debug.WriteLine(errorMessage);

                Error.Write(db, new Exception(errorMessage.ToString(), error), submit);
                return false;
            }
        }
    }
}