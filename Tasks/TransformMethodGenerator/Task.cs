using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Allegiance.CommunitySecuritySystem.DataAccess;

namespace Allegiance.CommunitySecuritySystem.TransformMethodGenerator
{
	public class Task
	{
		public delegate string OperationDelegate(int currentDepth, int maxDepth);

		private static Random _random = new Random();
		private static List<OperationDelegate> _operations = InitializeOperations();

		public List<OperationDelegate> Operations
		{
			get { return _operations; }
		}

		private static List<OperationDelegate> InitializeOperations()
		{
			List<OperationDelegate> operations = new List<OperationDelegate>();

			operations.Add(new OperationDelegate(RandomForRecursiveLoop1Operation));
			operations.Add(new OperationDelegate(RandomForRecursiveLoop2Operation));
			operations.Add(new OperationDelegate(RandomForRecursiveLoop3Operation));
			operations.Add(new OperationDelegate(RandomForRecursiveLoop4Operation));
			
			operations.Add(new OperationDelegate(WhileRecursive1LoopOperation));
			operations.Add(new OperationDelegate(WhileRecursive2LoopOperation));
			operations.Add(new OperationDelegate(WhileRecursive3LoopOperation));

			operations.Add(new OperationDelegate(RandomizeStringBuilder1Operation));
			operations.Add(new OperationDelegate(RandomizeStringBuilder2Operation));
			operations.Add(new OperationDelegate(RandomizeStringBuilder3Operation));
			operations.Add(new OperationDelegate(RandomizeStringBuilder4Operation));
			
			return operations;
		}

		public static void Execute(int numberOfTransformMethodsToGenerate, int complexityLevel)
		{
			using (var db = new CSSDataContext())
			{
				for (int i = 0; i < numberOfTransformMethodsToGenerate; i++)
				{
					string codeFragment = GenerateCode(complexityLevel);

					db.TransformMethods.InsertOnSubmit(new TransformMethod()
					{
						Text = codeFragment
					});
				}

				db.SubmitChanges();
			}
		}

		public static String GenerateCode(int complexityLevel)
		{
			StringBuilder seedValue = new StringBuilder();

			for (int i = 0; i < 10; i++)
				seedValue.Append(_random.Next(10, 99).ToString());

			string output = String.Format(" sb.Append(\"{0}\"); ", seedValue.ToString());

			output += GetRandomOperation(0, complexityLevel);

			return output;
		}

		private static String GetRandomOperation(int currentDepth, int maxDepth)
		{
			if (currentDepth >= maxDepth)
				return String.Empty;

			int operationIndex = _random.Next(0, _operations.Count);

			return _operations[operationIndex](currentDepth + 1, maxDepth);
		}

		/// <summary>
		/// Do not add this to the operations list. This operation is the inner most operation 
		/// that all the other ones will work against.
		/// </summary>
		/// <param name="currentDepth"></param>
		/// <param name="maxDepth"></param>
		/// <returns></returns>
		//private static string SeedOperation(int currentDepth, int maxDepth)
		//{
		//    Random random = new Random();
		//    StringBuilder output = new StringBuilder();

		//    for (int i = 0; i < 10; i++)
		//        output.Append(i.ToString());

		//    return String.Format("b.Append(\"{0}\")", output.ToString());
		//}

		private static string RandomForRecursiveLoop1Operation(int currentDepth, int maxDepth)
		{
			string output = string.Format(@"
					int t{0} = 0;
					for(; t{0} < {1};) 
					{{
						t{0}++;",
				currentDepth,
				_random.Next(1, 10));

			output += GetRandomOperation(currentDepth, maxDepth);

			output += @" 
					} ";

			return output;
		}

		private static string RandomForRecursiveLoop2Operation(int currentDepth, int maxDepth)
		{
			string output = string.Format(@"
					int j{0} = 0;
					for(; j{0} < {1}; j{0}++) 
					{{ ",
				currentDepth,
				_random.Next(1, 10));

			output += GetRandomOperation(currentDepth, maxDepth);

			output += @" 
					} ";

			return output;
		}

		private static string RandomForRecursiveLoop3Operation(int currentDepth, int maxDepth)
		{
			string output = string.Format(@"
					int w{0} = 0;
					for(; true;) 
					{{
						w{0}++;
						if(w{0} > {1})
							break;",
				currentDepth,
				_random.Next(1, 10));

			output += GetRandomOperation(currentDepth, maxDepth);

			output += @" 
					} ";

			return output;
		}

		private static string RandomForRecursiveLoop4Operation(int currentDepth, int maxDepth)
		{
			string output = string.Format(@"
					for(int i{0} = 0; i{0} < {1}; i{0}++) 
					{{ ",
				currentDepth,
				_random.Next(1, 10));

			output += GetRandomOperation(currentDepth, maxDepth);

			output += @" 
					} ";

			return output;
		}

		private static string WhileRecursive1LoopOperation(int currentDepth, int maxDepth)
		{
			string output = string.Format(@"
					int i{0} = 0;
					do
					{{ ",
				currentDepth);

			output += GetRandomOperation(currentDepth, maxDepth);

			output += String.Format(@" 
					}} while(++i{0} < {1});",
				currentDepth,
				_random.Next(1, 10));

			return output;
		}

		private static string WhileRecursive2LoopOperation(int currentDepth, int maxDepth)
		{
			string output = string.Format(@"
					int z{0} = 0;
					do
					{{ ",
				currentDepth);

			output += GetRandomOperation(currentDepth, maxDepth);

			output += String.Format(@" 
					}} while(z{0}++ < {1});",
				currentDepth,
				_random.Next(1, 10));

			return output;
		}

		private static string WhileRecursive3LoopOperation(int currentDepth, int maxDepth)
		{
			string output = string.Format(@"
					int x{0} = 0;
					while(true)
					{{ ",
				currentDepth);

			output += GetRandomOperation(currentDepth, maxDepth);

			output += String.Format(@" 
						if(x{0}++ >= {1})
							break;
					}}",
				currentDepth,
				_random.Next(1, 10));

			return output;
		}

		private static string RandomizeStringBuilder1Operation(int currentDepth, int maxDepth)
		{
			string output = string.Format(@"
					for(int i{0} = 0; i{0} < {1}; i{0}++) {{ 
						char[] sbValue = sb.ToString().ToCharArray();
						int index1 = rand.Next(0, sbValue.Length - 1);
						int index2 = rand.Next(0, sbValue.Length - 1);

						char tempChar = sbValue[index1];
						sbValue[index1] = sbValue[index2];
						sbValue[index2] = tempChar;

						sb = new StringBuilder(new String(sbValue));
					}}
				", currentDepth, _random.Next(1, 10));

			return output + GetRandomOperation(currentDepth, maxDepth);
		}

		private static string RandomizeStringBuilder2Operation(int currentDepth, int maxDepth)
		{
			string output = string.Format(@"
					for(int i{0} = 0; i{0} < {1}; i{0}++) {{ 
						char[] sbValue = sb.ToString().ToCharArray();
						int index1 = rand.Next(0, sbValue.Length - 1);
						int index2 = rand.Next(0, sbValue.Length - 1);
						
						char char1 = sbValue[index1];
						char char2 = sbValue[index2];
						sbValue[index1] = ((Convert.ToInt32(char1) ^ Convert.ToInt32(char2)) % 10).ToString().ToCharArray()[0];
						sbValue[index2] = ((Convert.ToInt32(char1) * Convert.ToInt32(char2)) % 10).ToString().ToCharArray()[0];

						sb = new StringBuilder(new String(sbValue));
					}}
				", currentDepth, _random.Next(1, 10));

			return output + GetRandomOperation(currentDepth, maxDepth);
		}

		private static string RandomizeStringBuilder3Operation(int currentDepth, int maxDepth)
		{
			string output = string.Format(@"
					for(int q{0} = 0; q{0} < {1}; q{0}++) {{ 

						int midPoint = (int) Math.Floor((double) {1} / q{0});

						char[] sbValue = sb.ToString().ToCharArray();
						for(int j = 0; j < midPoint; j++)
						{{
							if(j >= sbValue.Length)
								continue;

							char char1 = sbValue[j];
							char char2 = sbValue[sbValue.Length - 1 - j];
							
							sbValue[sbValue.Length - 1 - j] = char1;
							sbValue[j] = char2;
						}}

						sb = new StringBuilder(new String(sbValue));
					}}
				", currentDepth, _random.Next(1, 10));

			return output + GetRandomOperation(currentDepth, maxDepth);
		}

		private static string RandomizeStringBuilder4Operation(int currentDepth, int maxDepth)
		{	
			string output = string.Format(@"
					for(int a{0} = 0; a{0} < {1}; a{0}++) {{ 

						char[] sbValue = sb.ToString().ToCharArray();

						for(int b{0} = 0; b{0} < sbValue.Length; b{0}++)
						{{
							char currChar = sbValue[b{0}];
							int nextValue = (int) currChar + rand.Next(0, 10);
			
							sbValue[b{0}] = (nextValue % 10).ToString()[0];

							sb = new StringBuilder(new String(sbValue));
						}}
					}}
				", currentDepth, _random.Next(1, 10));

			return output + GetRandomOperation(currentDepth, maxDepth);
		}

	}
}
