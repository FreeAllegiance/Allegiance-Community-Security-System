using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using Allegiance.CommunitySecuritySystem.DataAccess;

namespace Allegiance.CommunitySecuritySystem.AutoUpdate
{
    public class FileRequestHandler
    {
        #region Fields

        public const string Pattern = @"\/Files\/(\d+?)\/(.+)";

        #endregion

        #region Methods

        public static bool Handle(HttpContext context, int lobbyId, string filename)
        {
            using (var db = new CSSDataContext())
            {
                if (filename.Contains(".."))
                    throw new Exception("Could not load path.");

                var lobby       = db.Lobbies.Single(p => p.IsEnabled && p.Id == lobbyId);
                var filepath    = Path.Combine(lobby.BasePath, filename);

                //If file is not found, attempt to send file from the default path
                var info = new FileInfo(filepath);

                if (!info.Exists)
                {
                    filepath    = Path.Combine(ConfigurationManager.AppSettings["DefaultLobbyPath"], filename);
                    info        = new FileInfo(filepath);

                    if (!info.Exists)
                        throw new FileNotFoundException();
                }
                
                //Transmit the file
                context.Response.Clear();
                context.Response.ContentType = "application/octet-stream";
                context.Response.AddHeader("Content-Length", info.Length.ToString());
                context.Response.AddHeader("Content-Disposition", 
                    string.Format("attachment; filename=\"{0}\"", 
                    Path.GetFileName(filename)));
                context.Response.TransmitFile(filepath);
            }

            return true;
        }

        #endregion
    }
}