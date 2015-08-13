using System;
using System.IO;
using System.Text;
using System.Threading;

namespace Allegiance.CommunitySecuritySystem.Client.Utility
{
    class Log
    {
        #region Fields

        public const string OutputFile = "output.log";
        private static object _lock     = null;

        #endregion

        #region Methods

        public static void Write(string message)
        {
            if (!DataStore.Preferences.DebugLog)
                return;

            var sb = new StringBuilder();
            if (_lock == null)
            {
                _lock = new object();

                lock (_lock)
                {
                    if (File.Exists(OutputFile))
                        File.Delete(OutputFile);
                }

                sb.AppendLine(string.Format("-------------- Log Start {0:M/d/yy} at {0:h:mm tt} UTC --------------", DateTime.UtcNow))
                    .AppendLine();
            }

            var lines = message.Split(new char[] { '\n' });

            sb.AppendFormat("{0} UTC: \t", DateTime.UtcNow);
            foreach (var line in lines)
                sb.AppendLine(line.Trim()).Append("\t\t\t");

            sb.AppendLine();

			Exception lastException = null;
			for (int i = 0; i < 30; i++)
			{
				try
				{
					lock (_lock)
						File.AppendAllText(OutputFile, sb.ToString());

					lastException = null;
					break;
				}
				catch (Exception ex)
				{
					lastException = ex;
					Thread.Sleep(100);
				}
			}

			if (lastException != null)
			{
				var error = new Exception("Log::Log.Write(): Giving up writing: " + message + " after 30 tries in 3 seconds.", lastException);
				File.WriteAllText("ExceptionLog.txt", error.ToString());
			}
        }

        public static void Write(Exception error)
        {
            Write(error.ToString());
        }

        #endregion
    }
}