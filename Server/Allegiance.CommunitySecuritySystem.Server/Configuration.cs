using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Configuration;
using System.Xml;

namespace Allegiance.CommunitySecuritySystem.Server
{
	public class Configuration
	{
		private static Configuration _instance = new Configuration();
		public static Configuration Instance
		{
			get { return _instance; }
		}

		public CssServerConfiguration CssServer { get; set; }

		private Configuration()
		{
			CssServer = (CssServerConfiguration) ConfigurationManager.GetSection("cssServer");
		}
	}

	public class CssServerConfiguration : IConfigurationSectionHandler
	{
		public readonly NameValueCollection UserTokens = new NameValueCollection();

		public object Create(object parent, object configContext, System.Xml.XmlNode section)
		{
			CssServerConfiguration returnValue = new CssServerConfiguration();

			foreach (XmlNode node in section.SelectNodes("userTokens/userToken"))
			{
				returnValue.UserTokens.Add(node.Attributes["name"].Value, node.Attributes["value"].Value);
			}

			return returnValue;
		}
	}
}