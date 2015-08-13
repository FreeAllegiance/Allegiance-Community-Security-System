using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using System.Text;

namespace Allegiance.CommunitySecuritySystem.Server.Utilities
{
	// Modified from sample at: 
	// http://stackoverflow.com/questions/3666954/string-or-binary-data-would-be-truncated-linq-exception-cant-find-which-fiel

	public class LinqErrorDetector
	{
		public static String AnalyzeDBChanges(System.Data.Linq.DataContext db)
		{
			StringBuilder stringBuilder = new StringBuilder();

			foreach (object update in db.GetChangeSet().Updates)
			{
				stringBuilder.Append(FindLongStrings(update));
			}

			foreach (object insert in db.GetChangeSet().Inserts)
			{
				stringBuilder.Append(FindLongStrings(insert));
			}

			return stringBuilder.ToString();
		}

		private static String FindLongStrings(object testObject)
		{
			StringBuilder stringBuilder = new StringBuilder();

			foreach (PropertyInfo propInfo in testObject.GetType().GetProperties())
			{
				foreach (ColumnAttribute attribute in propInfo.GetCustomAttributes(typeof(ColumnAttribute), true))
				{
					if (attribute.DbType.ToLower().Contains("varchar"))
					{
						string dbType = attribute.DbType.ToLower();
						int numberStartIndex = dbType.IndexOf("varchar(") + 8;
						int numberEndIndex = dbType.IndexOf(")", numberStartIndex);
						string lengthString = dbType.Substring(numberStartIndex, (numberEndIndex - numberStartIndex));
						int maxLength = 0;
						int.TryParse(lengthString, out maxLength);

						string currentValue = (string)propInfo.GetValue(testObject, null);

						if (!string.IsNullOrEmpty(currentValue) && currentValue.Length > maxLength)
							stringBuilder.AppendLine(testObject.GetType().Name + "." + propInfo.Name + " " + currentValue + " Max: " + maxLength);
					}
				}
			}

			return stringBuilder.ToString();
		}

	}
}