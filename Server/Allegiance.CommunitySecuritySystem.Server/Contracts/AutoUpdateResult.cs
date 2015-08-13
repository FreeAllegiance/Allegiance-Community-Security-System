using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using Allegiance.CommunitySecuritySystem.DataAccess;

namespace Allegiance.CommunitySecuritySystem.Server.Contracts
{
    [DataContract]
    public class AutoUpdateResult
    {
        #region Properties

        [DataMember]
        public string AutoUpdateBaseAddress { get; set; }

        [DataMember]
        public List<FindAutoUpdateFilesResult> Files { get; set; }

        #endregion

        #region Methods

        public static AutoUpdateResult RetrieveFileList(int lobbyId)
        {
            using (var db = new CSSDataContext())
            {
                return new AutoUpdateResult()
                {
                    AutoUpdateBaseAddress   = ConfigurationManager.AppSettings["autoupdateBaseAddress"],
                    Files                   = db.FindAutoUpdateFiles(lobbyId).ToList()
                };
            }
        }

        #endregion
    }
}