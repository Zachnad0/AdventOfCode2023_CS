using System;
using System.IO;
using System.Linq;

namespace CSharp
{
	public class Day8 : IDailyProgram
	{
		public static void RunP1(string[] args)
		{
			string[] lines = File.ReadAllLines("../input8.txt");
			string dirInstructSeq = lines[0];

			int currLine = Array.FindIndex(lines, l => new string(l.Take(3).ToArray()) == "AAA"), pathCount = 0;
			string currNode = new(lines[currLine].Take(3).ToArray());
			while (currNode != "ZZZ")
			{
				char dirToGo = dirInstructSeq[pathCount % dirInstructSeq.Length];
				int idStartIndex = dirToGo switch { 'L' => 7, 'R' => 12, _ => throw new IndexOutOfRangeException("Like bruh") };
				currNode = new(lines[currLine].Remove(0, idStartIndex).Take(3).ToArray());
				currLine = Array.FindIndex(lines, l => new string(l.Take(3).ToArray()) == currNode);
				if (currLine == -1)
					Console.WriteLine("INVALID CURRNODE ID DETECTED!!!");
				pathCount++;
			}

			Console.WriteLine($"Path Count: {pathCount}");
		}

		public static void RunP2(string[] args)
		{
			string[] lines = File.ReadAllLines("../input8.txt");
			string dirInstructSeq = lines[0];

			int currLine = Array.FindIndex(lines, l => new string(l.Take(3).ToArray()) == "AAA"), pathCount = 0;
			string currNode = new(lines[currLine].Take(3).ToArray());
			while (currNode != "ZZZ")
			{
				char dirToGo = dirInstructSeq[pathCount % dirInstructSeq.Length];
				int idStartIndex = dirToGo switch { 'L' => 7, 'R' => 12, _ => throw new IndexOutOfRangeException("Like bruh") };
				currNode = new(lines[currLine].Remove(0, idStartIndex).Take(3).ToArray());
				currLine = Array.FindIndex(lines, l => new string(l.Take(3).ToArray()) == currNode);
				if (currLine == -1)
					Console.WriteLine("INVALID CURRNODE ID DETECTED!!!");
				pathCount++;
			}

			Console.WriteLine($"Path Count: {pathCount}");
		}
	}
}
