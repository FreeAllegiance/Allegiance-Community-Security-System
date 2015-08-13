using System;
using System.Configuration;
using System.IO;
using System.Linq;
using Allegiance.CommunitySecuritySystem.DataAccess;
using Allegiance.CommunitySecuritySystem.DataAccess.Enumerations;

namespace Allegiance.CommunitySecuritySystem.Cleanup
{
    public static class Task
    {
        public static void Execute()
        {
            Execute(true);
        }

        public static void Execute(bool cleanupSessions)
        {
            Execute(cleanupSessions, false);
        }

        public static void Execute(bool cleanupSessions, bool cleanupKeys)
        {
            Execute(cleanupSessions, cleanupKeys, false, false, false, true);
        }

        public static void Execute(bool cleanupSessions, bool cleanupKeys, bool cleanupPolls, bool cleanupCaptchas, bool recalculatePolls, bool cleanLogs)
        {
            if (cleanupSessions)
            {
                //Cleanup old sessions
                Console.WriteLine("Removing old Sessions...");
                CleanupSessions();
            }

            if (cleanupKeys)
            {
                //Delete old blackboxes & active keys
                Console.WriteLine("Deleting old Keys...");
                CleanupBlackboxes();
            }

            if (cleanupPolls)
            {
                //Match up PollOption records with duplicate poll + identities from merging
                Console.WriteLine("Merging Poll Votes from Matched Identities...");
                CleanupPolls();
            }

			if (cleanupCaptchas == true)
			{
				Console.WriteLine("Clearing expired Captchas...");
				CleanupCaptchas();
			}

			if (recalculatePolls == true)
			{
				Console.WriteLine("Calculating Polls...");
				RecalculatePolls();
			}

			if (cleanLogs == true)
			{
				Console.WriteLine("Cleaning database logs...");
				CleanLogs();
			}
        }

		private static void CleanLogs()
		{
			try
			{
				DataAccess.Log.Clean();
				DataAccess.Error.Clean();
				DataAccess.LogIP.Clean();
			}
			catch (Exception error)
			{
				Error.Write(error);
			}
		}

		private static void RecalculatePolls()
		{
			try
			{
				DataAccess.Poll.RecalculateAllPolls();
			}
			catch (Exception error)
			{
				Error.Write(error);
			}
		}

		private static void CleanupCaptchas()
		{
			try
			{
				DataAccess.Captcha.RemoveExpiredCaptchas();
			}
			catch (Exception error)
			{
				Error.Write(error);
			}
		}

        private static void CleanupSessions()
        {
            try
            {
                using (var db = new CSSDataContext())
                {
                    //Find all expired sessions
                    var timeout = DateTime.Now.AddSeconds(-Session.DefaultStandardTimeout);
                    var oldSessions = from s in db.Sessions
                                      where s.SessionStatusId == (int)SessionStatusEnum.Closed
                                        || s.DateLastCheckIn < timeout
                                      select s;

                    db.Sessions.DeleteAllOnSubmit(oldSessions);
                    db.SubmitChanges();
                }
            }
            catch (Exception error)
            {
                Error.Write(error);
            }
        }

        private static void CleanupBlackboxes()
        {
            try
            {
                using (var db = new CSSDataContext())
                {
                    var minUsedKeys = Math.Min(db.Logins.Count(), ActiveKey.PreferredMinUsedKeys);

                    //Find all activekeys
                    var latest  = DateTime.Now.AddHours(-ActiveKey.PreferredMinLifetime);
                    var oldKeys = from a in db.ActiveKeys
                                  where a.Sessions.Count() == 0
                                    && a.UsedKeys.Count() >= minUsedKeys
                                    && a.DateCreated < latest
                                  select a;

                    var filenames = oldKeys.Select(p => p.Filename).ToList();

                    //Delete unused keys
                    foreach (var usedKeys in oldKeys.Select(p => p.UsedKeys))
                        db.UsedKeys.DeleteAllOnSubmit(usedKeys);

                    db.ActiveKeys.DeleteAllOnSubmit(oldKeys);
                    db.SubmitChanges();

                    //Delete all associated files
                    var root = ConfigurationManager.AppSettings["OutputRoot"];
                    foreach (var filename in filenames)
                    {
                        var fullPath = Path.Combine(root, filename);

                        if (File.Exists(fullPath))
                            File.Delete(fullPath);
                    }
                }
            }
            catch (Exception error)
            {
                Error.Write(error);
            }
        }

        private static void CleanupPolls()
        {
            try
            {
                using (var db = new CSSDataContext())
                {
                    //var duplicates = db.PollVotes.GroupBy(p => p.Login.Identity).Where(p => p.Count() > 1);
                    var duplicates = db.PollVotes.GroupBy(p => new { p.Login.Identity, p.PollOption.Poll }).Where(p => p.Count() > 1);
					
					foreach(var duplicate in duplicates)
					{
						var votesToDelete = duplicate.OrderBy(p => p.Login.DateCreated).Skip(1);
						db.PollVotes.DeleteAllOnSubmit(votesToDelete);
					}

					////Retrieve duplicate all votes per poll
					//var polls = from v in db.DuplicateVotes
					//            group v by v.PollId into p
					//            select p;

					//foreach (var poll in polls)
					//{
					//    //Retrieve all unique identities per poll
					//    var idents = from v in poll
					//                 group v by v.IdentityId into i
					//                 select i;

					//    foreach (var ident in idents)
					//    {
					//        //If the user has voted more than once on a single
					//        //poll, delete all polls after the first vote
					//        if (ident.Count() > 1)
					//        {
					//            var toDelete = ident.Skip(1);

					//            foreach (var duplicate in toDelete)
					//            {
					//                var vote = db.PollVotes.Single(p => p.Id == duplicate.PollVoteId);
					//                db.PollVotes.DeleteOnSubmit(vote);
					//            }
					//        }
					//    }
					//}

                    db.SubmitChanges();
                }
            }
            catch (Exception error)
            {
                Error.Write(error);
            }
        }
    }
}