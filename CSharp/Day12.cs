using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp
{
	public class Day12 : IDailyProgram
	{
		private const char OPRTNL_CHAR = '.', BRKN_CHAR = '#', UNKNWN_CHAR = '?';
		private const int N_OF_COPIES = 5;

		public static void RunP1(string[] args)
		{
			string[] lines = File.ReadAllLines("../input12.txt");

			long sumOfPossibleArrangements = 0;
			for (int lN = 0; lN < lines.Length; lN++)
			{
				char[] springGroup;
				int[] brokenSpringSetLengths;
				// Damn bruh scoping be like???
				{
					// First part is spring group, other is numbers
					string[] sections = lines[lN].Split(' ');
					springGroup = sections[0].ToArray();
					brokenSpringSetLengths = sections[1].Split(',').Select(int.Parse).ToArray();
				}

				// Cycle through every damn combination until something fits
				int twoToUnknownCount = (int)Math.Pow(2, springGroup.Count(UNKNWN_CHAR.Equals));
				List<int> unknownIndices = new();
				for (int i = 0; i < springGroup.Length; i++)
					if (springGroup[i] == UNKNWN_CHAR)
						unknownIndices.Add(i);

				for (int cN = 0; cN < twoToUnknownCount; cN++)
				{
					// Generate based on binary of unknownCountSqrd
					char[] currSpringGroup = springGroup.ToArray();
					bool[] binarySeed = Convert.ToString(cN, 2).PadLeft(unknownIndices.Count, '0').Select('1'.Equals).ToArray();

					for (int e = 0; e < unknownIndices.Count; e++)
						currSpringGroup[unknownIndices[e]] = binarySeed[e] ? BRKN_CHAR : OPRTNL_CHAR;

					// Test, if successful, increase sum
					List<string> brokenChunks = new string(currSpringGroup).Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
					if (brokenChunks.Count == brokenSpringSetLengths.Length)
					{
						bool fail = false;
						for (int i = 0; i < brokenSpringSetLengths.Length; i++)
						{
							if (brokenSpringSetLengths[i] != brokenChunks[i].Length)
							{
								fail = true;
								break;
							}
						}

						// If it passes the test above, then it's a possible config
						if (!fail)
							sumOfPossibleArrangements++;
					}
				}

				Console.WriteLine($"Line {lN} complete.");
			}

			Console.WriteLine($"\n\t\tNumber of total possible arrangements: {sumOfPossibleArrangements}");
		}

		public static void RunP2(string[] args)
		{
			string[] lines = File.ReadAllLines("../input12.txt");

			long sumOfPossibleArrangements = 0;
			object sumLock = new();

			Parallel.For(0, lines.Length, lN =>
			{
				long localSum = 0;
				char[] springGroup;
				int[] brokenSpringSetLengths;
				// Damn bruh scoping be like???
				{
					// First part is spring group, other is numbers
					string[] sections = lines[lN].Split(' ');
					string originalSec0 = sections[0];
					string originalSec1 = sections[1];
					for (int i = 1; i < N_OF_COPIES; i++)
					{
						sections[0] = string.Join(UNKNWN_CHAR, sections[0], originalSec0);
						sections[1] = string.Join(',', sections[1], originalSec1);
					}

					springGroup = sections[0].ToArray();
					brokenSpringSetLengths = sections[1].Split(',').Select(int.Parse).ToArray();
				}

				// Cycle through every damn combination until something fits
				long twoToUnknownCount = (long)Math.Pow(2, springGroup.Count(UNKNWN_CHAR.Equals));
				List<int> unknownIndices = new();
				for (int i = 0; i < springGroup.Length; i++)
					if (springGroup[i] == UNKNWN_CHAR)
						unknownIndices.Add(i);

				Parallel.For(0, twoToUnknownCount, cN =>
				{
					// Generate based on binary of unknownCountSqrd
					char[] currSpringGroup = springGroup.ToArray();
					bool[] binarySeed = Convert.ToString(cN, 2).PadLeft(unknownIndices.Count, '0').Select('1'.Equals).ToArray();

					for (int e = 0; e < unknownIndices.Count; e++)
						currSpringGroup[unknownIndices[e]] = binarySeed[e] ? BRKN_CHAR : OPRTNL_CHAR;

					// Test, if successful, increase sum
					List<string> brokenChunks = new string(currSpringGroup).Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
					if (brokenChunks.Count == brokenSpringSetLengths.Length)
					{
						bool fail = false;
						for (int i = 0; i < brokenSpringSetLengths.Length; i++)
						{
							if (brokenSpringSetLengths[i] != brokenChunks[i].Length)
							{
								fail = true;
								break;
							}
						}

						// If it passes the test above, then it's a possible config
						if (!fail)
							Interlocked.Increment(ref localSum);
					}
				});

				// Add onto overall sum
				Interlocked.Add(ref sumOfPossibleArrangements, localSum);

				Console.WriteLine($"Line {lN} complete with {localSum} possible arrangements.");
			});

			Console.WriteLine($"\n\t\tNumber of total possible arrangements: {sumOfPossibleArrangements}");
		}
	}
}
