using System;
using System.Text.RegularExpressions;
using System.Web;

namespace Allegiance.CommunitySecuritySystem.AutoUpdate
{
    public class AutoUpdateHandler : IHttpHandler
    {
        #region IHttpHandler Members

        /// <summary>
        /// You will need to configure this handler in the web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            //write your handler implementation here.
            try
            {
                HandleRequest(context);
            }
            catch (Exception error)
            {
				DataAccess.Error.Write(error);

                context.Response.Write(string.Format("Failure: {0}", error.Message));
            }
        }

        #endregion

        #region Methods

        public void HandleRequest(HttpContext context)
        {
            var url = context.Request.Url.PathAndQuery;

            //Handle File pattern requests
            if (HandleMatch(url, FileRequestHandler.Pattern,
                p => FileRequestHandler.Handle(context, int.Parse(p.Groups[1].Value), p.Groups[2].Value)))
                return;
        }

        private bool HandleMatch(string input, string pattern, Func<Match, bool> handler)
        {
            var match = Regex.Match(input, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            if (!match.Success)
                return false;

            return handler(match);
        }

        #endregion
    }
}