using System.IO;

namespace CSharp
{
	public class Day19 : IDailyProgram
	{
		public static void RunP1(string[] args)
		{
			string[] lines = File.ReadAllLines("../input19.txt");
			// Remove if not matrix:
			char[][] matrix = lines.ToMatrix();
		}

		public static void RunP2(string[] args)
		{

		}
	}
}
