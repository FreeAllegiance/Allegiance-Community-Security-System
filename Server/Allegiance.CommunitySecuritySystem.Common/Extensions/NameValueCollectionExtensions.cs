using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace Allegiance.CommunitySecuritySystem.Common.Extensions
{
    public static class NameValueCollectionExtensions
    {
        public static T GetValue<T>(this NameValueCollection collection, string key, Func<string, T> parse)
        {
            return collection.GetValue(key, parse, default(T));
        }

        public static T GetValue<T>(this NameValueCollection collection, string key, Func<string, T> parse, T defaultValue)
        {
            try
            {
                return parse(collection[key]);
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
