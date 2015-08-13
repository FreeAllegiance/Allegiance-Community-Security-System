using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Web;

namespace Allegiance.CommunitySecuritySystem.Common.Utility
{
	public enum CacheSeconds
	{
		TenSeconds = 10
	}

	public static class CacheManager<T>
	{
		private static object _syncObject = new object();
		private static Cache _cache = null;

		public delegate T LoadCacheDelegate();

		private static Cache Cache
		{
			get
			{
				if (_cache == null)
				{
					if ( HttpContext.Current == null )
					{
						HttpRuntime httpRuntime = new HttpRuntime();
					}

					_cache = HttpRuntime.Cache;
				}

				return _cache;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cacheKey">The string key for the cache item</param>
		/// <param name="cacheSeconds">The number of seconds to keep the item in cache</param>
		/// <param name="loadCacheDelegate">The delegate to run to load the object into cache if the object was not present in the cache.</param>
		/// <returns></returns>
		public static T Get(string cacheKey, CacheSeconds cacheSeconds, LoadCacheDelegate loadCacheDelegate)
		{
			T returnValue = (T) Cache.Get(cacheKey);

			if (returnValue == null)
			{
				lock (_syncObject)
				{
					// Check it again to ensure that another thread didn't load it.
					returnValue = (T) Cache.Get(cacheKey);

					if (returnValue != null)
						return returnValue;

					returnValue = loadCacheDelegate();

					Cache.Add(cacheKey, returnValue, null, DateTime.Now.AddSeconds((int) cacheSeconds), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
				}
			}

			return returnValue;
		}

	}
}
