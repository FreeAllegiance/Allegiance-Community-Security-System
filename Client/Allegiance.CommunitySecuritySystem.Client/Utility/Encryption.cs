using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace Allegiance.CommunitySecuritySystem.Client.Utility
{
    public class Encryption<T> : IDisposable where T : HashAlgorithm, IDisposable
    {
        #region Fields

        private HashAlgorithm algo = null;

        #endregion

        #region Constructors

        public Encryption()
        {
            algo = HashAlgorithm.Create(typeof(T).Name);

            if (algo == null)
                algo = Activator.CreateInstance<T>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates a checksum for specified file
        /// </summary>
        public string Calculate(string path)
        {
            using (var fs = File.OpenRead(path))
            {
                var hash = algo.ComputeHash(fs);
                return Convert.ToBase64String(hash);
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (algo != null)
                algo.Clear();
        }

        #endregion
    }
}
