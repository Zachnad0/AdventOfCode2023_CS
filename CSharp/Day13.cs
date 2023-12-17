using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSharp
{
	public class Day13 : IDailyProgram
	{
		public static void RunP1(string[] args)
		{
			string[] lines = File.ReadAllLines("../input13.txt");
			List<char[][]> sectionMatrices = new();
			{
				int[] sectionSplitIndices = lines.Where(string.Empty.Equals).Select(l => Array.IndexOf(lines, l)).ToArray();
				int prevIndex = 0;
				foreach (int index in sectionSplitIndices)
				{
					char[][] thisMatrix = new char[lines[index - 1].Length][];
					for (int x = 0; x < thisMatrix.Length; x++)
					{
						thisMatrix[x] = new char[index - prevIndex];
						for (int y = 0; y < thisMatrix[x].Length; y++)
						{
							thisMatrix[x][y] = lines[prevIndex + y][x]; // CONTINUE HERE with D13P1
						}
					}
					sectionMatrices.Add(thisMatrix);
				}
			}
		}

		public static void RunP2(string[] args)
		{

		}
	}
}
