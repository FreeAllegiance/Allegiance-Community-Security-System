using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Allegiance.CommunitySecuritySystem.GenerateHash
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var filename in args)
            {
                using (SHA256Managed sha = new SHA256Managed())
                using (FileStream fs = File.Open(filename, FileMode.Open, FileAccess.Read))
                {
                    var hash = sha.ComputeHash(fs);
                    File.WriteAllBytes("KnownHash.txt", hash);
                }
            }
        }
    }
}