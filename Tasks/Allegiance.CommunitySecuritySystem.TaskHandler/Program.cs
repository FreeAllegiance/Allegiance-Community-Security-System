using System;
using Allegiance.CommunitySecuritySystem.DataAccess;

namespace Allegiance.CommunitySecuritySystem.TaskHandler
{
    class Program
    {
        #region Methods

        static void Main(string[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    switch (args[0].ToLower())
                    {
                        case "-generateblackboxes":
                            BlackboxGenerator.Task.Execute(Configuration.NumBlackboxes, Configuration.DebugMode);
                            break;

                        case "-cleanup":
                            Cleanup.Task.Execute(true, true, true, true, true, true);
                            break;

                        case "-cleanupsessions":
                            Cleanup.Task.Execute(true);
                            break;

                        case "-cleanupblackboxes":
                            Cleanup.Task.Execute(true, true);
                            break;

                        case "-cleanuppolls":
                            Cleanup.Task.Execute(false, false, true, false, false, false);
                            break;

						case "-generatetransforms":
							TransformMethodGenerator.Task.Execute(Configuration.NumberOfTransformMethods, Configuration.TransformMethodComplexityLevel);
							break;

						default:
							PrintHelp();
							break;
                    }
                }
				else
				{
					PrintHelp();
				}
            }
            catch (Exception error)
            {
                Error.Write(error);
                throw error;
            }
        }

		private static void PrintHelp()
		{
			Console.WriteLine(@"Usage: 
-generateblackboxes	Creates BlackBoxes for user login.
-cleanup			Cleans old sessions, blackboxes, and polls, captchas, and keys.
-cleanupSessions	Cleans up just the sessions for debugging.
-cleanupBlackboxes	Cleans up just the black boxes for debugging.
-cleanupPolls		Cleans up just the polls for debugging.
-generateTransforms	Creates transform methods to make the blackboxes unique.

See the config file for parameters to each option.
");
		}

        #endregion
    }
}