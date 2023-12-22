using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace CSharp
{
	public class Day16 : IDailyProgram
	{
		private const char EMPTY_SPACE = '.', MIRROR_A = '/', MIRROR_B = '\\', SPLIT_VERT = '-', SPLIT_HORIZ = '|', ENERGIZED_CHAR = '#';
		private class BeamAgent
		{
			public static char[][] EnergizedMap, ContraptionMatrix;
			public static List<BeamAgent> Agents { get; private set; } = new();
			private static List<(Vector2 Pos, Vector2 Dir)> SharedHistory = new();
			public Vector2 CurrPos { get; private set; }
			public Vector2 CurrDir { get; set; }
			public BeamAgent(Vector2 startPos, Vector2 startDir)
			{
				CurrPos = startPos;
				CurrDir = startDir;
				Agents.Add(this);
			}
			/// <returns>True if the agent was destroyed this step</returns>
			public bool Step()
			{
				// Consider the tile being entered, not current tile
				Vector2 enteringPos = CurrPos + CurrDir;
				if (enteringPos != Vector2.Clamp(enteringPos, new(0, 0), new(ContraptionMatrix.Length - 1, ContraptionMatrix[0].Length - 1)) || SharedHistory.Contains((enteringPos, CurrDir))) // If going out of bounds, or repeating history, cleanse
				{
					if (Agents.Count == 1)
						SharedHistory = new(); // BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBRRRRRRRRRRRRRRRRRRRRRRRRUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH
					Agents.Remove(this);
					return true;
				}
				SharedHistory.Add((enteringPos, CurrDir));

				CurrPos = enteringPos;
				EnergizedMap[(int)CurrPos.X][(int)CurrPos.Y] = ENERGIZED_CHAR;
				char tileChar;
				switch (tileChar = ContraptionMatrix[(int)CurrPos.X][(int)CurrPos.Y])
				{
					case SPLIT_HORIZ or SPLIT_VERT:
						TrySplitAgent(tileChar);
						break;

					case MIRROR_A: // y becomes -x, x becomes -y.
						CurrDir = new(-Math.Sign(CurrDir.Y), -Math.Sign(CurrDir.X));
						break;

					case MIRROR_B: // y becomes x, x becomes y.
						CurrDir = new(Math.Sign(CurrDir.Y), Math.Sign(CurrDir.X));
						break;

					default: // Otherwise carry on through
						break;
				}

				return false;
			}
			public void TrySplitAgent(char splitterChar)
			{
				// Logic for splitting goes here
				if (splitterChar == SPLIT_HORIZ && CurrDir.X != 0)
				{
					CurrDir = new(0, 1);
					_ = new BeamAgent(CurrPos, new(0, -1));
				}
				else if (splitterChar == SPLIT_VERT && CurrDir.Y != 0)
				{
					CurrDir = new(1, 0);
					_ = new BeamAgent(CurrPos, new(-1, 0));
				}
			}
		}

		public static void RunP1(string[] args)
		{
			char[][] contraptionMatrix = File.ReadAllLines("../input16.txt").ToMatrix();
			BeamAgent.EnergizedMap = new char[contraptionMatrix.Length][];
			BeamAgent.ContraptionMatrix = new char[contraptionMatrix.Length][];
			for (int x = 0; x < contraptionMatrix.Length; x++) // Clone properly
			{
				BeamAgent.EnergizedMap[x] = (char[])contraptionMatrix[x].Clone();
				BeamAgent.ContraptionMatrix[x] = (char[])contraptionMatrix[x].Clone();
			}

			_ = new BeamAgent(new(-1, 0), new(1, 0)); // Starting agent
													  //long prevEnTileCount = -1, sameInARow = 0;
			while (BeamAgent.Agents.Count > 0)
			{
				for (int a = 0; a < BeamAgent.Agents.Count; a++)
					if (BeamAgent.Agents[a].Step())
						a--;

				//long currEnTileCount = BeamAgent.EnergizedMap.Sum(col => col.Count(ENERGIZED_CHAR.Equals));
				//if (currEnTileCount == prevEnTileCount)
				//{
				//	sameInARow++;
				//	if (sameInARow > Math.Pow(contraptionMatrix.Length, 2))
				//		break;
				//	continue;
				//}
				//sameInARow = 0;
				//prevEnTileCount = currEnTileCount;
			}

			Console.WriteLine($"No. of energized positions: {BeamAgent.EnergizedMap.Sum(col => col.Count(ENERGIZED_CHAR.Equals))}");
		}

		public static void RunP2(string[] args)
		{
			char[][] contraptionMatrix = File.ReadAllLines("../input16.txt").ToMatrix();
			List<long> energizedAmounts = new();

			for (int side = 0; side < 4; side++)
			{
				for (int i = 0; i < contraptionMatrix.Length; i++)
				{
					// Generate e-map and c-matrix
					BeamAgent.EnergizedMap = new char[contraptionMatrix.Length][];
					BeamAgent.ContraptionMatrix = new char[contraptionMatrix.Length][];
					for (int x = 0; x < contraptionMatrix.Length; x++) // Clone properly
					{
						BeamAgent.EnergizedMap[x] = (char[])contraptionMatrix[x].Clone();
						BeamAgent.ContraptionMatrix[x] = (char[])contraptionMatrix[x].Clone();
					}

					// Get starting pos and starting dir based on side
					Vector2 startPos = new(), startDir = new();
					switch (side)
					{
						case 0: // Left to right
							startPos = new(-1, i);
							startDir = new(1, 0);
							break;
						case 1: // Right to left
							startPos = new(contraptionMatrix.Length, i);
							startDir = new(-1, 0);
							break;
						case 2: // Bottom to top
							startPos = new(i, -1);
							startDir = new(0, 1);
							break;
						case 3: // Top to bottom
							startPos = new(i, contraptionMatrix[0].Length);
							startDir = new(0, -1);
							break;
					}
					//Console.WriteLine(startPos);

					// Run calculations until no more repeated history
					_ = new BeamAgent(startPos, startDir); // Starting agent
					while (BeamAgent.Agents.Count > 0)
					{
						for (int a = 0; a < BeamAgent.Agents.Count; a++)
							if (BeamAgent.Agents[a].Step())
								a--;
					}
					energizedAmounts.Add(BeamAgent.EnergizedMap.Sum(col => col.Count(ENERGIZED_CHAR.Equals)));
					Console.WriteLine($"E-pos # @ {side}::{i}: {energizedAmounts[^1]}");
				}
			}
			Console.WriteLine($"\n\nHighest is: {energizedAmounts.Max()}");
		}
	}
}
