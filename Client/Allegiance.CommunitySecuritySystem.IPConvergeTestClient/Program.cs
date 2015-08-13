using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;

namespace Allegiance.CommunitySecuritySystem.IPConvergeTestClient
{
	class Program
	{

		public enum ConvergeMethod
		{
			HandshakeStart = 1,
			HandshakeEnd = 2,
			HandshakeRemove = 3
		}


		private string HandShakeStartPayload = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<methodCall>
	<methodName>ipConverge.handshakeStart</methodName>
	<params>
		<param>
			<value>
			<struct>
				<member>
					<name>reg_id</name>
					<value><int>5</int></value>
				</member>
				<member>
					<name>reg_code</name>
					<value><string>c353930b3066d870e3d31e8ecb6f02a7</string></value>
				</member>
				<member>
					<name>reg_date</name>
					<value><int>1304113036</int></value>
				</member>
				<member>
					<name>reg_product_id</name>
					<value><int>5</int></value>
				</member>
				<member>
					<name>converge_url</name>
					<value><string>http://auth.alleg.net</string></value>
				</member>
				<member>
					<name>acp_email</name>
					<value><string>nick@chi-town.com</string></value>
				</member>
				<member>
					<name>acp_md5_password</name>
					<value><string>a46cf985e7ac57468b104e565854564c</string></value>
				</member>
				<member>
					<name>http_user</name>
					<value><string></string></value>
				</member>
				<member>
					<name>http_pass</name>
					<value><string></string></value>
				</member>
			</struct>
			</value>
		</param>
	</params>
</methodCall>
";


		private string HandShakeEndPayload = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<methodCall>
	<methodName>ipConverge.handshakeEnd</methodName>
	<params>
		<param>
			<value>
			<struct>
				<member>
					<name>reg_id</name>
					<value><int>5</int></value>
				</member>
				<member>
					<name>reg_code</name>
					<value><string>c353930b3066d870e3d31e8ecb6f02a7</string></value>
				</member>
				<member>
					<name>reg_date</name>
					<value><int>1304113036</int></value>
				</member>
				<member>
					<name>reg_product_id</name>
					<value><int>5</int></value>
				</member>
				<member>
					<name>converge_url</name>
					<value><string>http://auth.alleg.net</string></value>
				</member>
				<member>
					<name>acp_email</name>
					<value><string>nick@chi-town.com</string></value>
				</member>
				<member>
					<name>acp_md5_password</name>
					<value><string>a46cf985e7ac57468b104e565854564c</string></value>
				</member>
				<member>
					<name>http_user</name>
					<value><string></string></value>
				</member>
				<member>
					<name>http_pass</name>
					<value><string></string></value>
				</member>
				<member>
					<name>handshake_completed</name>
					<value><int>1</int></value>
				</member>
			</struct>
			</value>
		</param>
	</params>
</methodCall>
";

		private string HandshakeRemovePayload = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<methodCall>
	<methodName>ipConverge.handshakeRemove</methodName>
	<params>
		<param>
			<value>
			<struct>
				<member>
					<name>reg_code</name>
					<value><string>c353930b3066d870e3d31e8ecb6f02a7</string></value>
				</member>
				<member>
					<name>reg_product_id</name>
					<value><int>5</int></value>
				</member>
			</struct>
			</value>
		</param>
	</params>
</methodCall>
";

		static int Main(string[] args)
		{

			//<value><string>a46cf985e7ac57468b104e565854564c</string></value>

			//var md5 = MD5.Create();
			//byte [] hash = md5.ComputeHash(Encoding.Default.GetBytes("xxxxxx"));

			//// and create a string.
			//StringBuilder sBuilder = new StringBuilder();

			//// Loop through each byte of the hashed data 
			//// and format each one as a hexadecimal string.
			//for (int i = 0; i < hash.Length; i++)
			//{
			//    sBuilder.Append(hash[i].ToString("x2"));
			//}

			//// Return the hexadecimal string.
			//Console.WriteLine(sBuilder.ToString());



			if (args.Length != 2)
			{
				Console.WriteLine("Usage: IPConvergeTestClient <url> <option>");
				Console.WriteLine("\tOptions");
				Console.WriteLine("\t==========================");
				Console.WriteLine("\t1: Handshake Start");

				return -1;
			}

			var program = new Program();
			
			return program.Run(args);
		}

		private int Run(string[] args)
		{
			string url = args[0];
			ConvergeMethod convergeMethod = (ConvergeMethod)Enum.Parse(typeof(ConvergeMethod), args[1]);

			string payload = String.Empty;

			payload = GetPayload(convergeMethod);

			var webRequest = WebRequest.Create(url);
			webRequest.Method = "Post";
			StreamWriter sw = new StreamWriter(webRequest.GetRequestStream());

			sw.Write(payload);
			sw.Flush();
			sw.Close();

			var webResponse = webRequest.GetResponse();
			StreamReader sr = new StreamReader(webResponse.GetResponseStream());

			Console.Write(sr.ReadToEnd());

			return 0;
		}

		private string GetPayload(ConvergeMethod convergeMethod)
		{
			switch (convergeMethod)
			{
				case ConvergeMethod.HandshakeStart:
					return HandShakeStartPayload;

				case ConvergeMethod.HandshakeEnd:
					return HandShakeEndPayload;

				case ConvergeMethod.HandshakeRemove:
					return HandshakeRemovePayload;

				default:
					throw new NotSupportedException(convergeMethod.ToString());
			}
		}
	}
}
