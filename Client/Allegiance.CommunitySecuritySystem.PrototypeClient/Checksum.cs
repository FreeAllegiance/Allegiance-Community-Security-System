using System;
using System.IO;
using System.Security.Cryptography;

namespace Allegiance.CommunitySecuritySystem.PrototypeClient
{
    public static class Checksum
    {
        #region Fields

        private static HashAlgorithm algo = null;

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the Checksum class with the specified hash algorithm
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void Initialize<T>() where T: HashAlgorithm
        {
            if (algo != null)
                algo.Clear();

            algo = HashAlgorithm.Create(typeof(T).Name);

            if (algo == null)
                algo = Activator.CreateInstance<T>();
        }

        /// <summary>
        /// Calculates a checksum for specified file
        /// </summary>
        public static string Calculate(string path)
        {
            using (var fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var hash = algo.ComputeHash(fs);
                return Convert.ToBase64String(hash);
            }
        }

        #endregion
    }
}