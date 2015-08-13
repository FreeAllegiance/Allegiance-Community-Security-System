using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Allegiance.CommunitySecuritySystem.DataAccess.Enumerations;

namespace Allegiance.CommunitySecuritySystem.DataAccess
{
    public partial class TransformMethod
    {
        #region Fields

        //private static int _instance = 0;

        #endregion

        #region Methods

        public string Replace(string input, string fragmentInput, 
            byte[] knownHash, bool debugMode, out byte[] rsaBlob, out string token)
        {
            //Create seed, create RSA blob, replace logic
            var csp     = new CspParameters();
            csp.Flags   = CspProviderFlags.UseMachineKeyStore;

            using (var rsa = new RSACryptoServiceProvider(2048, csp))
            {
                var random          = new Random();
                var seed            = random.Next(int.MinValue, int.MaxValue);
                rsaBlob             = rsa.ExportCspBlob(true);

                var publicBlob      = rsa.ExportCspBlob(false);
                var publicKey       = Convert.ToBase64String(publicBlob);

                var fragmentLogic   = SwapCode(fragmentInput, seed, string.Empty, debugMode);
                token               = GenerateToken(fragmentLogic, knownHash);

				return SwapCode(input, seed, publicKey, debugMode);
            }
        }

        private string SwapCode(string classLogic, int seed, string publicKey, bool debugMode)
        {
            classLogic = Regex.Replace(classLogic, @"_seed \= (.+?);", string.Format("_seed = {0};", seed), RegexOptions.Multiline);
            classLogic = Regex.Replace(classLogic, @"_rsaKey \= (.+?);", string.Format("_rsaKey = \"{0}\";", publicKey), RegexOptions.Multiline);
            classLogic = Regex.Replace(classLogic, @"_randomMethod", this.Text, RegexOptions.Multiline);
			classLogic = Regex.Replace(classLogic, @"_debugMode \= (.+?);", string.Format("_debugMode = {0};", debugMode.ToString().ToLower()), RegexOptions.Multiline);

            return classLogic;
        }

        private static string GenerateToken(string classLogic, byte[] hash)
        {
            using (var provider = CodeDomProvider.CreateProvider("CSharp"))
            {
                //Compile blackbox fragment and execute to retrieve token
                var parameters = new CompilerParameters();

                parameters.GenerateInMemory = true;

                parameters.ReferencedAssemblies.Add("System.dll");
                parameters.ReferencedAssemblies.Add("System.Security.dll");
                parameters.ReferencedAssemblies.Add("System.Xml.dll");

                var results = provider.CompileAssemblyFromSource(parameters, classLogic);
                if (results.Errors.HasErrors)
                {
                    var sb = new StringBuilder()
                        .AppendLine("Failed to generate blackbox:");
                    foreach (CompilerError error in results.Errors)
                        sb.AppendLine(string.Format("Line {0}: {1}", error.Line, error.ErrorText));

                    Log.Write(LogType.BlackBoxGenerator, sb.ToString());

                    throw new Exception("Failed to generate token.");
                }

                var assembly = results.CompiledAssembly;
                var tokenGeneration = assembly.GetType("Allegiance.CommunitySecuritySystem.BlackboxGenerator.Resources.TokenGeneration");
                var method = tokenGeneration.GetMethod("Generate", BindingFlags.Static | BindingFlags.Public);

                return method.Invoke(null, new object[] { hash }) as string;
            }
        }

        #endregion
    }
}