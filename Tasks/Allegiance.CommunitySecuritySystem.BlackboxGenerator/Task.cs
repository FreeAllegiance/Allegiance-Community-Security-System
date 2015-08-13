using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Allegiance.CommunitySecuritySystem.DataAccess;

namespace Allegiance.CommunitySecuritySystem.BlackboxGenerator
{
    public class Task
    {
        #region Methods

        public static void Execute(int numBlackboxesToGenerate, bool debugMode)
        {
            Console.Write("Generating {0} blackbox(es)... ", numBlackboxesToGenerate);

            using (var db = new CSSDataContext())
            {
				int numberOfAvailableBlackboxes = db.ActiveKeys.Where(p => p.UsedKeys.Count() == 0).Count();

				if (debugMode == false && numberOfAvailableBlackboxes > numBlackboxesToGenerate)
				{
					Console.Write(string.Format("There are already {0} black boxes available, not generating more.", numberOfAvailableBlackboxes));
					return;
				}

                for (int i = 0; i < numBlackboxesToGenerate; i++)
					GenerateBlackbox(db, debugMode);
            }

            Console.WriteLine("Done.");
        }

		internal static ActiveKey GenerateBlackbox(CSSDataContext db, bool debugMode)
        {
			var key = Compiler.Build(debugMode);

            db.ActiveKeys.InsertOnSubmit(key);
            db.SubmitChanges();

            return key;
        }

        #endregion
    }
}