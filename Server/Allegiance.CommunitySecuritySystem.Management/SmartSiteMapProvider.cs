using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;

namespace Allegiance.CommunitySecuritySystem.Management
{
	// Uses a sample provided by: http://www.csharper.net/blog/sitemapprovider_doesn_t_take_querystring_into_consideration.aspx#custom_sitemapprovider_incorporates_querystring_reliance.aspx
	public class SmartSiteMapProvider : XmlSiteMapProvider
	{
		public override void Initialize(string name, NameValueCollection attributes)
		{
			base.Initialize(name, attributes);
			this.SiteMapResolve += new SiteMapResolveEventHandler(SmartSiteMapProvider_SiteMapResolve);
		}

		SiteMapNode SmartSiteMapProvider_SiteMapResolve(object sender, SiteMapResolveEventArgs e)
		{
			if (SiteMap.CurrentNode == null)
				return null;

			SiteMapNode temp;
			temp = SiteMap.CurrentNode.Clone(true);
			Uri u = new Uri(e.Context.Request.Url.ToString());

			SiteMapNode tempNode = temp;
			while (tempNode != null)
			{
				string qs = GetReliance(tempNode, e.Context);
				if (qs != null)
					if (tempNode != null)
						tempNode.Url += qs;

				tempNode = tempNode.ParentNode;
			}

			return temp;
		}

		private string GetReliance(SiteMapNode node, HttpContext context)
		{
			//Check to see if the node supports reliance
			if (node["reliantOn"] == null)
				return null;

			NameValueCollection values = new NameValueCollection();
			string[] vars = node["reliantOn"].Split(",".ToCharArray());

			foreach (string s in vars)
			{
				string var = s.Trim();
				//Make sure the var exists in the querystring
				if (context.Request.QueryString[var] == null)
					continue;

				values.Add(var, context.Request.QueryString[var]);
			}

			if (values.Count == 0)
				return null;

			return NameValueCollectionToString(values);
		}

		private string NameValueCollectionToString(NameValueCollection col)
		{
			string[] parts = new string[col.Count];
			string[] keys = col.AllKeys;

			for (int i = 0; i < keys.Length; i++)
				parts[i] = keys[i] + "=" + col[keys[i]];

			string url = "?" + String.Join("&", parts);
			return url;
		}

	}
}
