using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace Allegiance.CommunitySecuritySystem.Common.Utility
{
    public static class Encryption
    {
        private const int _dwSize = 2048;
        private const int _chunkSize = _dwSize / 8;

        public static byte[] DecryptRSA(byte[] data, byte[] key)
        {
            var csp     = new CspParameters();
            csp.Flags   = CspProviderFlags.UseMachineKeyStore;

            using (var rsa  = new RSACryptoServiceProvider(_dwSize, csp))
            using (var ms   = new MemoryStream())
            {
                //Create seed, create RSA blob, replace logic
                rsa.ImportCspBlob(key);

                for (int i = 0; i < data.Length; i += _chunkSize)
                {
                    int amount = Math.Min(_chunkSize, data.Length - i);
                    byte[] buffer = new byte[amount];

                    Buffer.BlockCopy(data, i, buffer, 0, amount);

                    byte[] decrypted = rsa.Decrypt(buffer, false);
                    ms.Write(decrypted, 0, decrypted.Length);
                }

                return ms.ToArray();
            }
        }

		/// <summary>
		/// Computes a SHA256 hash of input string
		/// </summary>
		public static string SHA256Hash(string value)
		{
			using (var sha = new SHA256Managed())
				return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(value)));
		}
    }
}
