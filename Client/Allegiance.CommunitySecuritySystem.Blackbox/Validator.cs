using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Allegiance.CommunitySecuritySystem.Blackbox
{
    public class Validator
    {
        #region Fields

        private const int _dwSize = 2048;

        private const int _chunkSize = 2048 / 8 - 42;

        private static int _seed = -51810617;

        private static string _rsaKey = "BgIAAACkAABSU0ExAAgAAAEAAQA9elnF7C3HJGr551LAYoeSrrCT6x3q1a98L7Z5H5y9FXsFts4YFElKARmqWGkkY1/7qsNZ6GxCk53Vub+FqfXbPhakkYQjTkNqb0JQH+1q9cMpfCSMAns4rea1P73KD4m1XWY7xb5j3LIgH8tWmdfWRnyKnw1Xlnvj2zFx6nf82BsKtH7bOW5jGRNEHtCoTzVYF7uz+51RYptdDac6WwLl0oGxQu2vWtyrNOhOQdoME7k/LmEo6bfcbVMpo1XNc7zg+dWeW2csfGRQX1Z01p1hOsGdOnAvIru8uYfLiRwqdBfXn1Qb1C0Nq5DfyDMC9bZB8iEC5Mv/faviIcDnBUDg";

        private static SecureString _token = null;

        #endregion

        #region Methods

        public static byte[] Check(MachineInformation information)
        {
            if(_token == null)
                _token = GenerateToken();

            lock (information)
            {
                try
                {
                    IntPtr ptr = Marshal.SecureStringToBSTR(_token);
                    information.Token = Marshal.PtrToStringBSTR(ptr);
                    return EncryptRSA(information.Serialize());
                }
                finally
                {
                    information.Token   = string.Empty;
                }
            }
        }

        private static SecureString GenerateToken()
        {
            byte[] hash;
            Assembly asm = Assembly.GetEntryAssembly();

            using (SHA256Managed sha = new SHA256Managed())
            using (FileStream fs = File.OpenRead(asm.Location))
                hash = sha.ComputeHash(fs);

            StringBuilder sb    = new StringBuilder();
            Random rand         = new Random(_seed);

            /***Random Portion********/

            //Algorithm 1
            //int len = rand.Next(189, 350);
            //for (int i = rand.Next(15, 35); i < len; i += 2)
            //    sb.Append((char)rand.Next(48, 122));
            
            //Algorithm 2
            //int len = rand.Next(53, 281);
            //int i   = 0;
            //do { sb.Append((char)rand.Next(26, 114)); }
            //while (++i < len);

            //Algorithm 3
            //int len = rand.Next(66, 252);
            //for (int i = 0; i < len; i += 3)
            //    sb.Append(Convert.ToChar(Math.Floor(128 * rand.NextDouble())));

            //Algorithm 4
            //int len = rand.Next(21, 125);
            //for (int i = 0; i < len; i++)
            //{
            //    sb.Append((char)rand.Next(21, 125))
            //        .Replace((char)rand.Next(31, 120), 
            //            (char)rand.Next(24, 108));
            //}

            //Algorithm 5
            int len = rand.Next(54, 255);
            for (int i = 0; i < len; i += 2)
            {
                int jlen = rand.Next(12, 63);
                for (int j = 0; j < jlen; i += 3)
                    sb.Append((char)rand.Next(15, 82));
            }
            
            /*************************/

            string value    = sb.ToString();
            byte[] data     = Encoding.UTF8.GetBytes(value);
            string iv       = GeneratePassword(0xa19c62);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Encoding.UTF8.GetBytes(value), Encoding.UTF8.GetBytes(value));

            using (Rijndael alg = Rijndael.Create())
            {
                alg.Key = pdb.GetBytes(32);
                alg.IV  = pdb.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(hash, 0, hash.Length);
                    cs.Close();

                    data = ms.ToArray();
                }
            }

            using (SHA256Managed sha = new SHA256Managed())
            {
                SecureString result = new SecureString();
                foreach(char s in BitConverter.ToString(sha.ComputeHash(data)))
                    result.AppendChar(s);

                result.MakeReadOnly();
                return result;
            }
        }

        private static byte[] EncryptRSA(byte[] data)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(_dwSize))
            using (MemoryStream ms = new MemoryStream())
            {
                rsa.ImportCspBlob(Convert.FromBase64String(_rsaKey));

                for(int i = 0; i < data.Length; i += _chunkSize)
                {
                    int amount      = Math.Min(_chunkSize, data.Length - i);
                    byte[] buffer   = new byte[amount];

                    Buffer.BlockCopy(data, i, buffer, 0, amount);

                    byte[] encrypted = rsa.Encrypt(buffer, false);
                    ms.Write(encrypted, 0, encrypted.Length);
                }

                return ms.ToArray();
            }
        }

        private static string GeneratePassword(int seed)
        {
            StringBuilder sb = new StringBuilder();
            Random rand = new Random(seed);
            int len = rand.Next(16, 31);

            for (int i = 0; i < len; i++)
                sb.Append((char)rand.Next(97, 122));

            return sb.ToString();
        }

        #endregion
    }
}