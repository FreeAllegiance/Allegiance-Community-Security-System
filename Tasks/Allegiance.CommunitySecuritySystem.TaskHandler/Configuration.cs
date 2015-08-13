using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Allegiance.CommunitySecuritySystem.TaskHandler
{
    static class Configuration
    {
        public static int NumBlackboxes
        {
            get
            {
                if (ConfigurationManager.AppSettings["NumBlackboxes"] != null)
                    return int.Parse(ConfigurationManager.AppSettings["NumBlackboxes"]);
                return 100;
            }
        }

		public static bool DebugMode
		{
			get
			{
				if (ConfigurationManager.AppSettings["DebugMode"] != null)
					return Boolean.Parse(ConfigurationManager.AppSettings["DebugMode"]);

				return false;
			}
		}

		public static int NumberOfTransformMethods
		{
			get
			{
				if (ConfigurationManager.AppSettings["NumberOfTransformMethods"] != null)
					return int.Parse(ConfigurationManager.AppSettings["NumberOfTransformMethods"]);
				return 10;
			}
		}

		public static int TransformMethodComplexityLevel
		{
			get
			{
				if (ConfigurationManager.AppSettings["TransformMethodComplexityLevel"] != null)
					return int.Parse(ConfigurationManager.AppSettings["TransformMethodComplexityLevel"]);
				return 3;
			}
		}
    }
}
