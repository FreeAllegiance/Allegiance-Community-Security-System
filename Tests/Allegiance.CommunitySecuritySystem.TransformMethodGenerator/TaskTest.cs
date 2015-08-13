using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.IO;
using Allegiance.CommunitySecuritySystem.TransformMethodGenerator;
using Allegiance.CommunitySecuritySystem.DataAccess;
using System.Diagnostics;

namespace Allegiance.CommunitySecuritySystem.TransformMethodGeneratorTest
{
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestClass]
	public class TaskTest
	{
		public TaskTest()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}


		[ClassInitialize()]
		public static void MyClassInitialize(TestContext testContext)
		{
			using (var db = new CSSDataContext())
			{
				if (db.DatabaseExists() == false)
				{
					db.CreateDatabase();
				}
			}
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		 

		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion


		[TestMethod]
		public void Execute()
		{
			using (var db = new CSSDataContext())
			{
				db.Sessions.DeleteAllOnSubmit(db.Sessions);
				db.UsedKeys.DeleteAllOnSubmit(db.UsedKeys);
				db.ActiveKeys.DeleteAllOnSubmit(db.ActiveKeys);
				db.TransformMethods.DeleteAllOnSubmit(db.TransformMethods);
				db.SubmitChanges();

				Allegiance.CommunitySecuritySystem.TransformMethodGenerator.Task.Execute(10, 10);

				Assert.AreEqual(10, db.TransformMethods.Count());
			}
		}

		[TestMethod]
		public void GenerateCode()
		{
			string lastGenerated = String.Empty;

			for (int i = 0; i < 10; i++)
			{
				string output = Allegiance.CommunitySecuritySystem.TransformMethodGenerator.Task.GenerateCode(10);
				//Console.Write(output);

				string dynamicResults = CompileAndInvoke(output);
				
				Assert.IsTrue(dynamicResults.Length > 0);

				Console.WriteLine("results: " + dynamicResults);

				lastGenerated = output;
			}

			Console.WriteLine(lastGenerated);
			
		}

		[TestMethod]
		public void TestAllOperations()
		{
			var task = new TransformMethodGenerator.Task();

			foreach (TransformMethodGenerator.Task.OperationDelegate operationDelegate in task.Operations)
			{
				string codeFragment = operationDelegate(0, 0);

				string result = CompileAndInvoke(codeFragment);

				Assert.IsTrue(result.Length > 0);

				Console.WriteLine("results: " + result);
			}
		}

		private string CompileAndInvoke(string codeFragment)
		{
			string dynamicCode = string.Format(@"
			
using System;
using System.IO;
using System.Text;

namespace Dynamic 
{{

	public class DynamicClass 
	{{
		
		public string DynamicCode(int seed)
		{{
			StringBuilder sb = new StringBuilder(""123456789"");
			Random rand = new Random(seed); 
			
			{0}

			return sb.ToString();
		}}
	}}
}}
", codeFragment);

			var compilerParamters = new CompilerParameters();
			compilerParamters.ReferencedAssemblies.Add("System.dll");
			compilerParamters.GenerateInMemory = false;

			var results = CodeDomProvider.CreateProvider("cs").CompileAssemblyFromSource(compilerParamters, dynamicCode);

			if (results.Errors.Count > 0)
			{
				StringReader sr = new StringReader(dynamicCode);
				string nextLine = sr.ReadLine();
				for (int i = 0; nextLine != null; nextLine = sr.ReadLine())
					Console.WriteLine(i + ": " + nextLine);

				foreach (CompilerError error in results.Errors)
					Console.WriteLine(error.ToString());
			}

			Assert.AreEqual(0, results.Errors.Count);

			var instance = results.CompiledAssembly.CreateInstance("Dynamic.DynamicClass");

			var dynamicResults = instance.GetType().InvokeMember("DynamicCode", System.Reflection.BindingFlags.InvokeMethod, null, instance, new object[] { 1234 });

			Assert.IsTrue(dynamicResults is String);

			return (string)dynamicResults;
		}


		private void Tester()
		{
			StringBuilder sb = new StringBuilder();
			Random rand = new Random();


			//////////////////////////////////////////////////////////////////////
			// Paste code from a generated transform method here to reformat 
			// or to check it's syntax.
			//////////////////////////////////////////////////////////////////////

			sb.Append("32362083391727629292");
			for (int i1 = 0; i1 < 5; i1++)
			{
				char[] sbValue = sb.ToString().ToCharArray();
				int index1 = rand.Next(0, sbValue.Length - 1);
				int index2 = rand.Next(0, sbValue.Length - 1);

				char char1 = sbValue[index1];
				char char2 = sbValue[index2];
				sbValue[index1] = ((Convert.ToInt32(char1) ^ Convert.ToInt32(char2)) % 10).ToString().ToCharArray()[0];
				sbValue[index2] = ((Convert.ToInt32(char1) * Convert.ToInt32(char2)) % 10).ToString().ToCharArray()[0];

				sb = new StringBuilder(new String(sbValue));
			}

			int j2 = 0;
			for (; j2 < 4; j2++)
			{
				int i3 = 0;
				do
				{
					for (int a4 = 0; a4 < 2; a4++)
					{

						char[] sbValue = sb.ToString().ToCharArray();

						for (int b4 = 0; b4 < sbValue.Length; b4++)
						{
							char currChar = sbValue[b4];
							int nextValue = (int)currChar + rand.Next(0, 10);

							sbValue[b4] = (nextValue % 10).ToString()[0];

							sb = new StringBuilder(new String(sbValue));
						}
					}

					int i5 = 0;
					do
					{
						int i6 = 0;
						do
						{
							for (int q7 = 0; q7 < 2; q7++)
							{

								int midPoint = (int)Math.Floor((double)2 / q7);

								char[] sbValue = sb.ToString().ToCharArray();
								for (int j = 0; j < midPoint; j++)
								{
									if (j >= sbValue.Length)
										continue;

									char char1 = sbValue[j];
									char char2 = sbValue[sbValue.Length - 1 - j];

									sbValue[sbValue.Length - 1 - j] = char1;
									sbValue[j] = char2;
								}

								sb = new StringBuilder(new String(sbValue));
							}

							for (int i8 = 0; i8 < 4; i8++)
							{
								char[] sbValue = sb.ToString().ToCharArray();
								int index1 = rand.Next(0, sbValue.Length - 1);
								int index2 = rand.Next(0, sbValue.Length - 1);

								char tempChar = sbValue[index1];
								sbValue[index1] = sbValue[index2];
								sbValue[index2] = tempChar;

								sb = new StringBuilder(new String(sbValue));
							}

							for (int q9 = 0; q9 < 5; q9++)
							{

								int midPoint = (int)Math.Floor((double)5 / q9);

								char[] sbValue = sb.ToString().ToCharArray();
								for (int j = 0; j < midPoint; j++)
								{
									if (j >= sbValue.Length)
										continue;

									char char1 = sbValue[j];
									char char2 = sbValue[sbValue.Length - 1 - j];

									sbValue[sbValue.Length - 1 - j] = char1;
									sbValue[j] = char2;
								}

								sb = new StringBuilder(new String(sbValue));
							}

							for (int i10 = 0; i10 < 1; i10++)
							{
							}
						} while (++i6 < 3);
					} while (++i5 < 3);
				} while (++i3 < 2);
			}










		}
	}
}
