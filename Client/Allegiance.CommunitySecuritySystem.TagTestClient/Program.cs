using System;
using System.Collections.Generic;
using System.Text;
using Allegiance.CommunitySecuritySystem.ServerTest;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Allegiance.CommunitySecuritySystem.TagTestClient.Properties;
using System.IO;
using System.Configuration;

namespace Allegiance.CommunitySecuritySystem.TagTestClient
{
	class Program
	{
		static void Main(string[] args)
		{
			TagTest tagTest = new TagTest();

			// To test with the local database, uncomment the below lines.
			tagTest.TestSaveGameDataWithGameXmlFiles();
			return;

			//string compressedXml = tagTest.GetCompressedGameDataXml();

			//For testing, we're accepting untrusted server certificates
			//ServicePointManager.ServerCertificateValidationCallback
			//    = new RemoteCertificateValidationCallback(delegate(object sender,
			//        X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
			//    {
			//        return true;
			//    });


			//CssServer.Tag.Tag tag = new Allegiance.CommunitySecuritySystem.TagTestClient.CssServer.Tag.Tag();

			////tag.Url = Settings.Default.Allegiance_CommunitySecuritySystem_TagTestClient_CssServer_Tag_Tag;

			//foreach (string file in Directory.GetFiles(ConfigurationManager.AppSettings["gameTestDataDirectory"], "*game *.xml"))
			//{
			//    Console.Write("Loading: " + file);


			//    string compressedXml = tagTest.Compress(File.ReadAllText(file));

			//    string message;
			//    int result;
			//    bool resultSpecified;

			//    try
			//    {
			//        tag.SaveGameData(compressedXml, out result, out resultSpecified, out message);

			//        Console.WriteLine("Complete - result: " + result + ", message: " + message);
			//    }
			//    catch (Exception ex)
			//    {
			//        Console.WriteLine("Error: " + ex.ToString());
			//    }

			//}
		}
	}
}
