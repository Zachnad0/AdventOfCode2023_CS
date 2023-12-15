using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace CSharp
{
	public class Day11 : IDailyProgram
	{
		private const char SPACE_CHAR = '.', GALAXY_CHAR = '#';
		private const int UNIVERSE_EXPAND_AMOUNT = 1000000;

		public static void RunP1(string[] args)
		{
			// Get unexpanded universe
			string[] lines = File.ReadAllLines("../input11.txt");
			char[][] universeImageInput = new char[lines[0].Length][];
			for (int cN = 0; cN < universeImageInput.Length; cN++)
				universeImageInput[cN] = new char[lines.Length];
			for (int y = 0; y < lines.Length; y++)
				for (int x = 0; x < lines[0].Length; x++)
					universeImageInput[x][y] = lines[y][x];

			// Expand universe
			List<List<char>> expandedUniverse = universeImageInput.Select(a => a.ToList()).ToList();
			for (int col = 0; col < expandedUniverse.Count; col++)
			{
				// Insert empty col after empty cols, and after doing so jump forward additionally
				if (!expandedUniverse[col].Contains(GALAXY_CHAR))
				{
					expandedUniverse.Insert(col + 1, new(expandedUniverse[col]));
					col++;
				}
			}
			for (int row = 0; row < expandedUniverse[0].Count; row++)
			{
				if (!expandedUniverse.Any(col => col[row] == GALAXY_CHAR))
				{
					foreach (List<char> col in expandedUniverse)
						col.Insert(row + 1, SPACE_CHAR);
					row++;
				}
			}
			int expUnivWidth = expandedUniverse.Count, expUnivHeight = expandedUniverse[0].Count;

			// Get galaxy coords
			List<Vector2> galaxies = new();
			for (int x = 0; x < expUnivWidth; x++)
				for (int y = 0; y < expUnivHeight; y++)
					if (expandedUniverse[x][y] == GALAXY_CHAR)
						galaxies.Add(new(x, y));

			// For each galaxy, for each other galaxy besides this, find number of steps required to go to other galaxy
			long sumOfSteps = 0;
			List<Vector2> alreadyDoneGalaxies = new();
			foreach (Vector2 startGalaxy in galaxies)
			{
				foreach (Vector2 endGalaxy in galaxies.Where(g => g != startGalaxy && !alreadyDoneGalaxies.Contains(g)))
				{
					sumOfSteps += (long)(Math.Abs(endGalaxy.X - startGalaxy.X) + Math.Abs(endGalaxy.Y - startGalaxy.Y));
				}
				alreadyDoneGalaxies.Add(startGalaxy);
			}

			Console.WriteLine($"Total Paths' Steps Count:\t{sumOfSteps}");
		}

		public static void RunP2(string[] args)
		{
			// Get unexpanded universe
			string[] lines = File.ReadAllLines("../input11.txt");
			char[][] universeImageInput = new char[lines[0].Length][];
			for (int cN = 0; cN < universeImageInput.Length; cN++)
				universeImageInput[cN] = new char[lines.Length];
			for (int y = 0; y < lines.Length; y++)
				for (int x = 0; x < lines[0].Length; x++)
					universeImageInput[x][y] = lines[y][x];
			int univImgWidth = universeImageInput.Length, univImgHeight = universeImageInput[0].Length;

			// Universe is no longer physically expanded, rather virtually accounted for later
			List<int> expandedCols = new(), expandedRows = new();
			for (int col = 0; col < univImgWidth; col++)
				if (!universeImageInput[col].Any(GALAXY_CHAR.Equals))
					expandedCols.Add(col);
			for (int row = 0; row < univImgHeight; row++)
				if (!universeImageInput.Any(col => col[row] == GALAXY_CHAR))
					expandedRows.Add(row);

			// Get galaxy coords
			List<Vector2> galaxies = new();
			for (int x = 0; x < univImgWidth; x++)
				for (int y = 0; y < univImgHeight; y++)
					if (universeImageInput[x][y] == GALAXY_CHAR)
						galaxies.Add(new(x, y));

			// For each galaxy, for each other galaxy besides this, find number of steps required to go to other galaxy
			long sumOfSteps = 0;
			List<Vector2> alreadyDoneGalaxies = new();
			foreach (Vector2 startGalaxy in galaxies)
			{
				foreach (Vector2 endGalaxy in galaxies.Where(g => g != startGalaxy && !alreadyDoneGalaxies.Contains(g)))
				{
					// Find distance horizontally, taking into account "expanded" rows and cols based on provided positions
					bool xForwards = startGalaxy.X <= endGalaxy.X, yUp = startGalaxy.Y <= endGalaxy.Y;
					for (int x = (int)startGalaxy.X; (xForwards && x < endGalaxy.X) || (!xForwards && x > endGalaxy.X); x += xForwards ? 1 : -1)
					{
						sumOfSteps += expandedCols.Contains(x) ? UNIVERSE_EXPAND_AMOUNT : 1;
					}
					for (int y = (int)startGalaxy.Y; (yUp && y < endGalaxy.Y) || (!yUp && y > endGalaxy.Y); y += yUp ? 1 : -1)
					{
						sumOfSteps += expandedRows.Contains(y) ? UNIVERSE_EXPAND_AMOUNT : 1;
					}
				}
				alreadyDoneGalaxies.Add(startGalaxy);
			}

			Console.WriteLine($"Total Paths' Steps Count:\t{sumOfSteps}");
		}
	}
}
