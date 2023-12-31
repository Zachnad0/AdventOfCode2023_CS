﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CSharp
{
	public class Day5 : IDailyProgram
	{
		public static void RunP1(string[] args)
		{
			string[] lines = File.ReadAllLines("../input5.txt");
			Array.Resize(ref lines, lines.Length + 1);
			lines[lines.Length - 1] = "";

			// Seeds
			List<long> previousNumbers = new();
			var cls1 = lines[0].Split(':', ' ').ToList();
			cls1.RemoveRange(0, 2);
			previousNumbers = cls1.Select(long.Parse).ToList();

			int secCount = lines.Where(l => l == "").Count() - 1;
			int prevIndex = 1;
			for (int section = 0; section < secCount; section++)
			{
				int sectionLength = lines.ToList().IndexOf("", prevIndex + 1) - prevIndex - 2;
				(long DestRS, long SrcRS, long RangeL)[] mapSpecs = new (long DestRS, long SrcRS, long RangeL)[sectionLength];

				// Get maps
				int nextIndex = lines.ToList().IndexOf("", prevIndex + 1);
				for (int l = prevIndex + 2, iter = 0; l < nextIndex; l++, iter++)
				{
					long[] vals = lines[l].Split(' ').Select(long.Parse).ToArray();
					mapSpecs[iter] = (vals[0], vals[1], vals[2]);

				}
				//Console.WriteLine($"Maps retrieved");

				// For each previous number, map them to new numbers. For each number in the range conv also.
				List<long> currentNumbers = new();
				for (int prevN = 0; prevN < previousNumbers.Count; prevN++)
				{
					long currNum = previousNumbers[prevN];
					bool numIsMapped = false;
					for (int mapN = 0; mapN < mapSpecs.Length; mapN++)
					{
						long diff = currNum - mapSpecs[mapN].SrcRS;
						if (diff < mapSpecs[mapN].RangeL && diff >= 0)
						{
							currentNumbers.Add(mapSpecs[mapN].DestRS + diff);
							numIsMapped = true;
							break;
						}
					}
					if (!numIsMapped) // If not on map, add straight up
						currentNumbers.Add(currNum);
					Console.WriteLine($"Number #{currentNumbers[prevN]} Complete");
				}

				// prevIndex is now next
				previousNumbers = new(currentNumbers);
				prevIndex = nextIndex;
				Console.WriteLine($"Section {section} Complete");
			}

			// Order from highest to lowest the location numbers
			Console.WriteLine(previousNumbers.OrderDescending().Last());
		}

		public static void RunP2(string[] args)
		{
			string[] lines = File.ReadAllLines("../input5.txt");
			Array.Resize(ref lines, lines.Length + 1);
			lines[lines.Length - 1] = "";

			// Seeds
			// Expand initial seeds to be that of ranges, via enumeration(?)
			List<string> seedClauses = lines[0].Split(':', ' ').ToList();
			seedClauses.RemoveRange(0, 2);
			List<long> finalNumbers = new(), seedStartNums = seedClauses.Select(long.Parse).ToList();
			for (int sPair = 0; sPair < seedClauses.Count; sPair += 2)
			{
				//	List<long> seedStartNums = new();
				List<long> previousNumbers = new();
				long startR = seedStartNums[sPair], range = seedStartNums[sPair + 1];
				for (int n = 0; n < range; n++)
					lock (previousNumbers)
						previousNumbers.Add(startR + n);
				//for (int i = 0; i < previousNumbers.Count; i += 2)
				//{
				//	long startR = previousNumbers[i], range = previousNumbers[i + 1];
				//	for (int n = 0; n < range; n++)
				//		seedNums.Add(startR + n);
				//}
				Console.WriteLine($"Initial phase success! Pair: #{sPair}");

				int secCount = lines.Where(l => l == "").Count() - 1;
				int prevIndex = 1;
				for (int section = 0; section < secCount; section++)
				{
					int sectionLength = lines.ToList().IndexOf("", prevIndex + 1) - prevIndex - 2;
					(long DestRS, long SrcRS, long RangeL)[] mapSpecs = new (long DestRS, long SrcRS, long RangeL)[sectionLength];

					// Get maps
					int nextIndex = lines.ToList().IndexOf("", prevIndex + 1);
					for (int l = prevIndex + 2, iter = 0; l < nextIndex; l++, iter++)
					{
						long[] vals = lines[l].Split(' ').Select(long.Parse).ToArray();
						mapSpecs[iter] = (vals[0], vals[1], vals[2]);

					}
					Console.WriteLine($"Maps retrieved! Pair: {sPair}, Sec: {section}");

					// For each previous number, map them to new numbers. For each number in the range conv also.
					//previousNumbers.RemoveAll(val => previousNumbers.Count(num => num == val) > 1); // Remove duplicate values
					List<long> currentNumbers = new();
					Console.WriteLine($"Beginning map passing for {previousNumbers.Count} numbers... Pair: {sPair}, Sec: {section}\nNumbers mapped:");
					Parallel.For(0, previousNumbers.Count, (prevN) =>
					{
						long currNum = previousNumbers[prevN];
						bool numIsMapped = false;
						for (int mapN = 0; mapN < mapSpecs.Length; mapN++)
						{
							long diff = currNum - mapSpecs[mapN].SrcRS;
							if (diff < mapSpecs[mapN].RangeL && diff >= 0)
							{
								lock (currentNumbers)
									currentNumbers.Add(mapSpecs[mapN].DestRS + diff);
								numIsMapped = true;
								break;
							}
						}
						if (!numIsMapped) // If not on map, add straight up
							lock (currentNumbers)
								currentNumbers.Add(currNum);
						//Console.Write($"#{currNum}, ");
					});

					//for (int prevN = 0; prevN < previousNumbers.Count; prevN++)
					//{
					//	long currNum = previousNumbers[prevN];
					//	bool numIsMapped = false;
					//	for (int mapN = 0; mapN < mapSpecs.Length; mapN++)
					//	{
					//		long diff = currNum - mapSpecs[mapN].SrcRS;
					//		if (diff < mapSpecs[mapN].RangeL && diff >= 0)
					//		{
					//			currentNumbers.Add(mapSpecs[mapN].DestRS + diff);
					//			numIsMapped = true;
					//			break;
					//		}
					//	}
					//	if (!numIsMapped) // If not on map, add straight up
					//			currentNumbers.Add(currNum);
					//	Console.WriteLine($"Number #{currentNumbers[prevN]} Complete");
					//}

					// prevIndex is now next
					previousNumbers = new(currentNumbers);
					prevIndex = nextIndex;
					Console.WriteLine($"\nSection #{section} Complete. Pair: {sPair}");
				}
				//lock (finalNumbers)
				finalNumbers.Add(previousNumbers.OrderDescending().Last());
			}

			// Order from highest to lowest the location numbers
			Console.WriteLine(finalNumbers.OrderDescending().Last());
		}
	}
}
