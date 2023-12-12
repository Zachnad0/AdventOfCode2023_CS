using System;
using System.Collections.Generic;
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

		//public static void RunP2(string[] args)
		//{
		//	string[] lines = File.ReadAllLines("../input8.txt");
		//	string dirInstructSeq = lines[0];

		//	//int currLine = Array.FindIndex(lines, l => new string(l.Take(3).ToArray()) == "AAA"), pathCount = 0;
		//	//string currNode = new(lines[currLine].Take(3).ToArray());
		//	// Starting nodes
		//	List<(string NodeID, int Line)> currNodes = lines.Skip(2).Where(l => l[2] == 'A').Select(l => (new string(l.Take(3).ToArray()), Array.IndexOf(lines, l))).ToList();

		//	int pathCount = 0;
		//	while (!currNodes.All(n => n.NodeID[2] == 'Z'))
		//	{
		//		char dirToGo = dirInstructSeq[pathCount % dirInstructSeq.Length];
		//		int idStartIndex = dirToGo switch { 'L' => 7, 'R' => 12, _ => throw new IndexOutOfRangeException("Like bruh") };

		//		// Iterate through and find all destination nodes for current nodes dependent on direction
		//		List<(string NodeID, int Line)> nextNodes = new();
		//		foreach (var node in currNodes)
		//		{
		//			string nextID = new(lines[node.Line].Skip(idStartIndex).Take(3).ToArray());
		//			int nextLine = Array.FindIndex(lines, l => new string(l.Take(3).ToArray()) == nextID);
		//			if (nextLine == -1)
		//				Console.WriteLine("ID Node not found???");

		//			nextNodes.Add((nextID, nextLine));
		//		}
		//		currNodes = nextNodes;

		//		pathCount++;
		//		if (pathCount % 1000 == 0)
		//			Console.WriteLine($"Pathcount is {pathCount}");
		//	}
		//	Console.WriteLine($"Path Count: {pathCount}");
		//}

		public static void RunP2(string[] args)
		{
			string[] lines = File.ReadAllLines("../input8.txt");
			string dirInstructSeq = lines[0];

			// Starting nodes
			List<(string NodeID, int Line)> startNodes = lines.Skip(2).Where(l => l[2] == 'A').Select(l => (new string(l.Take(3).ToArray()), Array.IndexOf(lines, l))).ToList();

			List<int> pathCounts = new();
			foreach (var startingNode in startNodes)
			{
				int pathCount = 0;
				int currLine = startingNode.Line;
				string currNode = startingNode.NodeID;
				while (currNode[2] != 'Z')
				{
					char dirToGo = dirInstructSeq[pathCount % dirInstructSeq.Length];
					int idStartIndex = dirToGo switch { 'L' => 7, 'R' => 12, _ => throw new IndexOutOfRangeException("Like bruh") };
					currNode = new(lines[currLine].Remove(0, idStartIndex).Take(3).ToArray());
					currLine = Array.FindIndex(lines, l => new string(l.Take(3).ToArray()) == currNode);
					if (currLine == -1)
						Console.WriteLine("INVALID CURRNODE ID DETECTED!!!");
					pathCount++;
				}
				pathCounts.Add(pathCount);
			}

			double lCM = LCM(pathCounts.Select<int, double>(l => l).ToArray());
			Console.WriteLine($"Path Count: {lCM:F}");
		}

		private static double LCM(double[] values) // TODO add to ZUtilLibs
		{
			var sortedVals = values.OrderByDescending(v => v).ToArray();

			double lCM = sortedVals[0];
			for (int i = 1; i < sortedVals.Length; i++)
			{
				double preLCM = lCM;
				while (lCM % sortedVals[i] != 0)
					lCM += preLCM;
			}

			return lCM;
		}
	}
}