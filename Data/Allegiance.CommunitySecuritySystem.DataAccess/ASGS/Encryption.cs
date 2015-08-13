using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Allegiance.CommunitySecuritySystem.DataAccess.ASGS
{
	public class Encryption
	{
		public static string EncryptAsgsPassword(string password, string callsign)
		{
			byte[] bytes = Encoding.Unicode.GetBytes(password);
			return Convert.ToBase64String(EncryptBytes(bytes, callsign));
		}

		private static byte[] EncryptBytes(byte[] SecureBytes, string PrivateKey)
		{
			byte[] array = new byte[]
				{
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0
				};

			PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(PrivateKey, array);

			byte[] array2 = new byte[]
				{
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0
				};

			byte[] key = passwordDeriveBytes.CryptDeriveKey("TripleDES", "MD5", 0, array2);

			TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
			tripleDESCryptoServiceProvider.Key = key;
			tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;

			return tripleDESCryptoServiceProvider.CreateEncryptor().TransformFinalBlock(SecureBytes, 0, SecureBytes.Length);
		}
	}
}
