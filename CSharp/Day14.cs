using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace CSharp
{
	public class Day14 : IDailyProgram
	{
		private const char MVBL_CHAR = 'O', STTC_CHAR = '#', GRND_CHAR = '.';
		private const int CYCLE_COUNT = 1000000000;

		public static void RunP1(string[] args)
		{
			string[] lines = File.ReadAllLines("../input14.txt");
			// Remove if not matrix:
			char[][] rocksMatrix = lines.ToMatrix();

			// Move all rocks as far negative Y as possible.
			int mWidth = rocksMatrix.Length, mHeight = rocksMatrix[0].Length;

			for (int y = 1; y < mHeight; y++) // Start at bottom, and pull down except first
			{
				for (int x = 0; x < mWidth; x++)
				{
					if (rocksMatrix[x][y] == MVBL_CHAR) // If a movable rock, move as far forward as possible
					{
						// Check positions below, relocate to next available pos
						for (int yOffsetPos = y - 1; yOffsetPos >= 0; yOffsetPos--)
						{
							if (rocksMatrix[x][yOffsetPos] is MVBL_CHAR or STTC_CHAR) // If cannot go further, put it just one above
							{
								rocksMatrix[x][y] = GRND_CHAR;
								rocksMatrix[x][yOffsetPos + 1] = MVBL_CHAR;
								break;
							}
							else if (yOffsetPos == 0)
							{
								rocksMatrix[x][y] = GRND_CHAR;
								rocksMatrix[x][yOffsetPos] = MVBL_CHAR;
								break;
							}
						}
					}
				}
			}

			// Calculate load
			long totalLoad = 0;
			for (int invY = 1; invY <= mHeight; invY++)
				totalLoad += rocksMatrix.Count(col => col[^invY] == MVBL_CHAR) * invY;

			Console.WriteLine(rocksMatrix.ToString(""));
			Console.WriteLine($"\n\nTotal Load: {totalLoad}");
		}

		public static void RunP2(string[] args)
		{
			string[] lines = File.ReadAllLines("../input14.txt");
			// Remove if not matrix:
			char[][] rocksMatrix = lines.ToMatrix();
			//DateTime startTime = DateTime.Now;

			List<char[][]> prevMatrices = new();
			for (int cycleNum = 0; cycleNum < CYCLE_COUNT; cycleNum++)
			{
				if (cycleNum % 1000000 == 0)
					Console.WriteLine($"Beginning {cycleNum / 1000000}-millionth cycle.");
				else if (cycleNum % 10000 == 0)
					Console.WriteLine($"Beginning {cycleNum / 1000}-thousandth cycle.");

				for (int spinCycleProgress = 0; spinCycleProgress < 4; spinCycleProgress++)
				{
					// Move all rocks as far in current dir as possible
					int mWidth = rocksMatrix.Length, mHeight = rocksMatrix[0].Length;
					for (int y = 1; y < mHeight; y++) // Start at bottom, and pull down except first
					{
						Parallel.For(0, mWidth, x =>
						{
							if (rocksMatrix[x][y] == MVBL_CHAR) // If a movable rock, move as far forward as possible
							{
								// Check positions below, relocate to next available pos
								for (int yOffsetPos = y - 1; yOffsetPos >= 0; yOffsetPos--)
								{
									if (rocksMatrix[x][yOffsetPos] is MVBL_CHAR or STTC_CHAR) // If cannot go further, put it just one above
									{
										rocksMatrix[x][y] = GRND_CHAR;
										rocksMatrix[x][yOffsetPos + 1] = MVBL_CHAR;
										break;
									}
									else if (yOffsetPos == 0)
									{
										rocksMatrix[x][y] = GRND_CHAR;
										rocksMatrix[x][yOffsetPos] = MVBL_CHAR;
										break;
									}
								}
							}
						});
					}

					// Rotate matrix 90 degrees clockwise
					rocksMatrix = rocksMatrix.RotateMatrix();
				}

				bool isAlreadyIncl = false;
				Parallel.ForEach(prevMatrices, (m, ls) => // TODO Day 14 Part 2 maybe LCM of each rock's mini cycle???
				{
					if (rocksMatrix.Identical(m))
					{
						isAlreadyIncl = true;
						ls.Stop();
					}
				});

				if (isAlreadyIncl)
					break;
				prevMatrices.Add(rocksMatrix);
			}

			//long ts = (long)(DateTime.Now - startTime).TotalSeconds;
			//Console.WriteLine($"{CYCLE_COUNT} cycles took {ts} seconds, therefore it would take {1000000000 / CYCLE_COUNT * ts} seconds to do full.");

			// Calculate load
			long totalLoad = 0;
			for (int invY = 1; invY <= rocksMatrix[0].Length; invY++)
				totalLoad += rocksMatrix.Count(col => col[^invY] == MVBL_CHAR) * invY;

			Console.WriteLine(rocksMatrix.ToString<char>(""));
			Console.WriteLine($"\n\nTotal Load: {totalLoad}");
		}
	}
}
