using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CSharp
{
	public class Day3 : IDailyProgram
	{
		private static readonly char[] Digits = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
		private static readonly char Dot = '.';

		public static void RunP1(string[] args)
		{
			char[][] engineMatrix = File.ReadAllLines("../input3.txt").Select(l => l.ToArray()).ToArray(); // [y][x]
			int width = engineMatrix[0].Length, height = engineMatrix.Length;
			// A symbol is anything that is not a Digit or Dot

			int partSum = 0;
			for (int y = 0; y < height; y++)
			{
				List<char> currNumber = new();
				bool currNumValid = false;
				for (int x = 0; x < width; x++)
				{
					// If it's a digit, add to number and scan all around for anything but a dot to validate current number
					if (Digits.Contains(engineMatrix[y][x]))
					{
						currNumber.Add(engineMatrix[y][x]);

						if (!currNumValid)
						{
							char[][] adjRows = engineMatrix[Math.Max(y - 1, 0)..(Math.Min(y + 1, height - 1) + 1)];
							for (int row = 0; row < adjRows.Length; row++)
							{
								char[] adjCols = adjRows[row][Math.Max(x - 1, 0)..(Math.Min(x + 1, width - 1) + 1)];

								if (adjCols.Any(c => !Digits.Contains(c) && c != Dot))
								{
									currNumValid = true;
									break;
								}
							}
						}

					}
					else // Otherwise end number, and maybe add to sum, regardless of symbol or dot
					{
						if (currNumValid)
						{
							partSum += int.Parse(currNumber.ToArray());
							Console.WriteLine($"ValidNum: {new string(currNumber.ToArray())}");
						}
						currNumValid = false;
						currNumber = new();
					}
				}
				if (currNumValid)
				{
					partSum += int.Parse(currNumber.ToArray());
					Console.WriteLine($"ValidNum: {new string(currNumber.ToArray())}");
				}
			}

			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"\n{partSum}");
			Console.ForegroundColor = ConsoleColor.Gray;
		}

		private static readonly char Gear = '*';

		public static void RunP2(string[] args)
		{
			char[][] engineMatrix = File.ReadAllLines("../input3.txt").Select(l => l.ToArray()).ToArray(); // [y][x]
			int width = engineMatrix[0].Length, height = engineMatrix.Length;
			Dictionary<(int X, int Y), List<int>> gearNumbers = new();
			// A symbol is anything that is not a Digit or Dot

			for (int y = 0; y < height; y++)
			{
				(int x, int y) gearPos = default;
				List<char> currNumber = new();
				bool currNumValid = false;
				for (int x = 0; x < width; x++)
				{
					// If it's a digit, add to number and scan all around for anything but a dot to validate current number
					if (Digits.Contains(engineMatrix[y][x]))
					{
						currNumber.Add(engineMatrix[y][x]);

						if (!currNumValid)
						{
							char[][] adjRows = engineMatrix[Math.Max(y - 1, 0)..(Math.Min(y + 1, height - 1) + 1)];
							for (int row = 0; row < adjRows.Length; row++)
							{
								char[] adjCols = adjRows[row][Math.Max(x - 1, 0)..(Math.Min(x + 1, width - 1) + 1)];
								for (int col = 0; col < adjCols.Length; col++)
								{
									if (adjCols[col] == Gear)
									{
										currNumValid = true;
										gearPos = (Math.Max(x + (col - 1), 0), y + row + (y > 0 ? -1 : 0));
										break;
									}
								}
							}
						}

					}
					else // Otherwise end number, and maybe add to sum, regardless of symbol or dot
					{
						if (currNumValid)
						{
							int numVal = int.Parse(currNumber.ToArray());
							if (!gearNumbers.TryAdd(gearPos, new() { numVal }))
								gearNumbers[gearPos].Add(numVal);
							Console.WriteLine($"ValidNum: {new string(currNumber.ToArray())}");
						}
						currNumValid = false;
						currNumber = new();
					}
				}
				if (currNumValid)
				{
					int numVal = int.Parse(currNumber.ToArray());
					if (!gearNumbers.TryAdd(gearPos, new() { numVal }))
						gearNumbers[gearPos].Add(numVal);
					Console.WriteLine($"ValidNum: {new string(currNumber.ToArray())}");
				}
			}

			int gearRatioSum = gearNumbers.Sum(kp => kp.Value.Count == 2 ? kp.Value[0] * kp.Value[1] : 0);
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"\n{gearRatioSum}");
			Console.ForegroundColor = ConsoleColor.Gray;
		}
	}
}
