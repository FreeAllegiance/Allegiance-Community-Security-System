using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Allegiance.CommunitySecuritySystem.Lobby
{
    public class QueryString
    {
        #region Properties

        public Dictionary<string, object> Collection { get; set; }

        #endregion

        #region Methods

        public QueryString()
        {
            Collection = new Dictionary<string, object>();
        }

        public QueryString Redirect()
        {
            HttpContext.Current.Response.Redirect(
                string.Concat(HttpContext.Current.Request.Url.LocalPath, this), true);

            return this;
        }

        public QueryString Add(string key, object value)
        {
            if (value is string)
            {
                if (string.IsNullOrEmpty((string)value))
                    return this;
            }

            return Add(key, value, p => true);
        }

        public QueryString Add<T>(string key, T value, Func<T, bool> AllowedValue)
        {
            if (AllowedValue(value))
                Collection.Add(key, value);

            return this;
        }

        public static string Get(string key)
        {
            var value = HttpContext.Current.Request.QueryString[key];
            if (!string.IsNullOrEmpty(value))
                return value;
            return null;
        }

        public static T Get<T>(string key, Func<string, T> Parse)
        {
            return Get(key, Parse, default(T));
        }
        
        public static T Get<T>(string key, Func<string, T> Parse, T defaultValue)
        {
            var value = HttpContext.Current.Request.QueryString[key];
            if (string.IsNullOrEmpty(value))
                return defaultValue;
            try
            {
                return Parse(value);
            }
            catch { }

            return defaultValue;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var c in Collection.Keys)
            {
                if(sb.Length > 0)
                    sb.Append("&");
                sb.AppendFormat("{0}={1}", c, Collection[c]);
            }

            if (sb.Length == 0)
                return string.Empty;

            return sb.Insert(0, "?").ToString();
        }

        #endregion
    }
}