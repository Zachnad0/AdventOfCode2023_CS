using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZUtilLib;

namespace CSharp
{
	public class Day15 : IDailyProgram
	{
		public static void RunP1(string[] args)
		{
			string[] clauses = File.ReadAllText("../input15.txt").Split(',');

			long resultSum = 0;
			for (int i = 0; i < clauses.Length; i++)
			{
				int currentVal = 0;
				for (int cN = 0; cN < clauses[i].Length; cN++)
				{
					currentVal += Convert.ToByte(clauses[i][cN]);
					currentVal *= 17;
					currentVal %= 256;
				}

				resultSum += currentVal;

			}

			Console.WriteLine($"Sum of results: {resultSum}");
		}

		public static void RunP2(string[] args)
		{
			string[] clauses = File.ReadAllText("../input15.txt").Split(',');

			List<(string ID, int Value)>[] boxes = new List<(string ID, int Value)>[256];
			for (int b = 0; b < boxes.Length; b++)
				boxes[b] = new();

			for (int i = 0; i < clauses.Length; i++)
			{
				string id = clauses[i].FilterNumbers(true).Replace("-", "").Replace("=", "");

				int currentHash = 0;
				for (int cN = 0; cN < id.Length; cN++)
				{
					currentHash += Convert.ToByte(id[cN]);
					currentHash *= 17;
					currentHash %= 256;
				}

				if (clauses[i][^1] == '-') // Remove lens
				{
					if (boxes.Any(b => b.Any(c => c.ID == id)))
					{
						//int boxNum = Array.IndexOf(boxes, boxes.First(b => b.Any(c => c.ID == id)));
						boxes[currentHash].RemoveAll(c => c.ID == id);
					}
				}
				else // Add lens
				{
					int value = int.Parse(clauses[i].FilterNumbers(false));

					if (boxes.Any(b => b.Any(c => c.ID == id))) // Insert and remove
					{
						//int boxNum = Array.IndexOf(boxes, boxes.First(b => b.Any(c => c.ID == id)));
						int ind = boxes[currentHash].FindIndex(c => c.ID == id);
						boxes[currentHash].RemoveAt(ind);
						boxes[currentHash].Insert(ind, (id, value));
					}
					else // Otherwise add in
					{
						boxes[currentHash].Add((id, value));
					}
				}
			}

			// Calculate focus power
			long sumOfFocalPowers = 0;
			for (int b = 0; b < boxes.Length; b++)
			{
				for (int e = 0; e < boxes[b].Count; e++)
				{
					sumOfFocalPowers += (b + 1) * (e + 1) * boxes[b][e].Value;
				}
			}

			Console.WriteLine($"Sum is {sumOfFocalPowers}");
		}
	}
}
