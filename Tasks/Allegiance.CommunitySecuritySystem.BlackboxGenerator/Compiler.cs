using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using Allegiance.CommunitySecuritySystem.DataAccess;
using Allegiance.CommunitySecuritySystem.DataAccess.Enumerations;

namespace Allegiance.CommunitySecuritySystem.BlackboxGenerator
{
    internal static class Compiler
    {
        #region Methods

		public static ActiveKey Build(bool debugMode)
        {
            using (var provider = CodeDomProvider.CreateProvider("CSharp"))
            {
                //Compile blackbox assembly, save to file
                var parameters  = new CompilerParameters();
                var id          = Guid.NewGuid();

                parameters.GenerateInMemory = false;
                parameters.OutputAssembly   = AllocateFilename(id);

                parameters.ReferencedAssemblies.Add("System.dll");
                parameters.ReferencedAssemblies.Add("System.Security.dll");
                parameters.ReferencedAssemblies.Add("System.Xml.dll");

                string token;
                byte[] rsaBlob;
                var transformMethod = PickTransformMethod();
                var source          = RetrieveSource(transformMethod, debugMode, out rsaBlob, out token);
                var results         = provider.CompileAssemblyFromSource(parameters, source.ToArray());

                if (results.Errors.HasErrors)
                {
                    var sb = new StringBuilder()
                        .AppendLine("Failed to generate blackbox:");
                    foreach (CompilerError error in results.Errors)
                        sb.AppendLine(string.Format("Line {0}: {1}", error.Line, error.ErrorText));

                    Log.Write(LogType.BlackBoxGenerator, sb.ToString());

                    throw new Exception("Failed to generate blackbox.");
                }

                return new ActiveKey()
                {
                    Filename            = Path.GetFileName(parameters.OutputAssembly),
                    DateCreated         = DateTime.Now,
                    TransformMethodId   = transformMethod.Id,
                    Token               = token,
                    RSACspBlob          = rsaBlob,
					IsValid				= true
                };
            }
        }

        private static string AllocateFilename(Guid guid)
        {
            var root        = ConfigurationManager.AppSettings["OutputRoot"];

			if (Directory.Exists(root) == false)
				Directory.CreateDirectory(root);

            var filename    = string.Format("{0}.dll", guid);

            return Path.Combine(root, filename);
        }

        private static TransformMethod PickTransformMethod()
        {
            using (var db = new CSSDataContext())
            {
                var random  = new Random();
                var methods = db.TransformMethods;
                var index   = random.Next(0, methods.Count() - 1);

                return methods.Skip(index).Take(1).FirstOrDefault();
            }
        }

        private static List<string> RetrieveSource(TransformMethod method, bool debugMode, out byte[] rsaBlob, out string token)
        {
            return new List<string>()
            {
                RetrieveFileText("DeviceInfo.txt"),
                RetrieveFileText("DeviceType.txt"),
                RetrieveFileText("MachineInformation.txt"),

                //Replace values in validator text
                method.Replace(RetrieveFileText("Validator.txt"), 
                    RetrieveFileText("TokenGeneration.txt"),
                    RetrieveFileBytes("KnownHash.txt"),
					debugMode,
                    out rsaBlob, out token)
            };
        }

        private static string RetrieveFileText(string path)
        {
            var root = ConfigurationManager.AppSettings["SourceRoot"];
            return File.ReadAllText(Path.Combine(root, path));
        }

        private static byte[] RetrieveFileBytes(string path)
        {
            var root = ConfigurationManager.AppSettings["SourceRoot"];
            return File.ReadAllBytes(Path.Combine(root, path));
        }

        #endregion
    }
}