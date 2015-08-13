using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace logCleanup
{
    class Program
    {
        static int Main(string[] args)
        {
            string[] sFiles = { };
            string sFileName = "";
            int iCount = 0;

			if (args.Length != 4)
			{
				Console.WriteLine("Usage: logCleanup.exe <directory> <days> <searchString> <recursion>");
				Console.WriteLine("directory: directory to start processing at");
				Console.WriteLine("days: delete files older than this value");
				Console.WriteLine("searchString: *.txt or *.log or similar");
				Console.WriteLine("resursion: one of top or all");
				return -1;
			}

			Console.WriteLine("Processing: " + args[0]);

            if (Directory.Exists(args[0].ToString()))
                if ("top" == args[3].ToString())
                    sFiles = Directory.GetFiles(args[0].ToString(), args[2], SearchOption.TopDirectoryOnly);
                else if ("all" == args[3].ToString())
                    sFiles = Directory.GetFiles(args[0].ToString(), args[2], SearchOption.AllDirectories);

            Console.WriteLine("Total Files Parsed: " + sFiles.Length);
            for (iCount = 0; iCount < sFiles.Length; iCount++)
            {
                sFileName = sFiles.GetValue(iCount).ToString();
                if (1 == DateTime.Compare(DateTime.Now, File.GetLastWriteTime(sFileName).AddDays(Convert.ToDouble(args[1]))))
                {
					try
					{
						File.Delete(sFileName);
						Console.WriteLine("File Deleted: " + sFileName);
					}
					catch (Exception ex)
					{
						Console.WriteLine("Couldn't delete: " + sFileName);
						Console.WriteLine(ex.ToString());
					}
                }
            }

			return 0;
        }
    }
}
