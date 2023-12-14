using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace CSharp
{
	public class Day10 : IDailyProgram
	{
		private const char
			UR_PIPE = 'L',
			UL_PIPE = 'J',
			LR_PIPE = '-',
			DR_PIPE = 'F',
			DL_PIPE = '7',
			UD_PIPE = '|',
			START_CHAR = 'S',
			GRND_CHAR = '.';

		private static readonly Dictionary<char, Vector2[]> ValidAttatchments = new()
		{
			{UD_PIPE,[new(0,1),new(0,-1)]}, {LR_PIPE,[new(1,0),new(-1,0)]}, {UR_PIPE,[new(0,-1),new(1,0)]}, {UL_PIPE,[new(0,-1),new(-1,0)]}, {DR_PIPE,[new(0,1),new(1,0)]}, {DL_PIPE,[new(0,1),new(-1,0)]}, {START_CHAR,[new(0,-1),new(0,1),new(-1,0),new(1,0)]}, {GRND_CHAR,[]}
		};

		/// <summary>
		/// Check if this pipe connects to other, and other connects to this.
		/// </summary>
		private static bool PipeConnectsTo((Vector2 Pos, char PipeChar) startPipe, (Vector2 Pos, char PipeChar) nextPipe)
		{
			if (Vector2.Distance(startPipe.Pos, nextPipe.Pos) is > 1 or <= 0)
				return false; // If not adjacent, then does not connect

			Vector2 relPosOfOtherToStart = Vector2.Normalize(nextPipe.Pos - startPipe.Pos);
			bool isStartPValid = ValidAttatchments.TryGetValue(startPipe.PipeChar, out Vector2[]? attchDirs1) && attchDirs1.Contains(relPosOfOtherToStart);
			bool isNextValid = ValidAttatchments.TryGetValue(nextPipe.PipeChar, out Vector2[]? attchDirs2) && attchDirs2.Contains(-relPosOfOtherToStart);

			return isStartPValid && isNextValid;
		}

		public static void RunP1(string[] args)
		{
			string[] lines = File.ReadAllLines("../input10.txt");
			int mWidth = lines[0].Length, mHeight = lines.Length;
			char[][] pipeMatrix = new char[mWidth][];
			for (int i = 0; i < mWidth; i++)
				pipeMatrix[i] = new char[mHeight];

			// Assign pipeMatrix values
			for (int x = 0; x < mWidth; x++)
				for (int y = 0; y < mHeight; y++)
					pipeMatrix[x][y] = lines[y][x];

			// Retrieve coords of start, and first connecting pipe
			int yCoord = -1;
			Vector2 startPos = new(Array.FindIndex(pipeMatrix, col =>
			{
				int yC = Array.IndexOf(col, START_CHAR);
				if (yC != -1)
				{
					yCoord = yC;
					return true;
				}
				return false;
			}), yCoord);

			List<(Vector2 Pos, long StepsTo)> pipeCoordAndSteps = new();
			(Vector2 Pos, char PipeChar, Vector2 DirEntered) firstAdjPipe = default;
			for (int x = -1; x < 2; x++)
			{
				for (int y = -1; y < 2; y++)
				{
					int xPos = Math.Clamp((int)startPos.X + x, 0, mWidth - 1), yPos = Math.Clamp((int)startPos.Y + y, 0, mHeight - 1);
					var coord = (new Vector2(xPos, yPos), pipeMatrix[xPos][yPos]);
					if (PipeConnectsTo(coord, (startPos, START_CHAR)))
					{
						firstAdjPipe = (coord.Item1, coord.Item2, startPos - new Vector2(xPos, yPos));
						goto ContL1;
					}
				}
			}
		ContL1:;

			// Follow and log pipes until start is reached again
			(Vector2 Pos, char PipeChar, Vector2 DirEntered) currPipe = firstAdjPipe;
			pipeCoordAndSteps.Add((currPipe.Pos, 1));
			int currSteps = 2;
			while (currPipe.Item2 != START_CHAR)
			{
				// Look at remaining directions for next pipe
				var remainingDirs = ValidAttatchments[currPipe.PipeChar].Where(d => currPipe.DirEntered != d);
				foreach (Vector2 dir in remainingDirs)
				{
					Vector2 pos = currPipe.Pos + dir;
					char pipeChar = pipeMatrix[(int)pos.X][(int)pos.Y];
					if (PipeConnectsTo((currPipe.Pos, currPipe.PipeChar), (pos, pipeChar)))
					{
						currPipe = (pos, pipeChar, -dir);
						pipeCoordAndSteps.Add((pos, currSteps));
						break;
					}
				}

				currSteps++;
			}

			// Get furthest coord by using trig.
			Vector2 furthestSegment = pipeCoordAndSteps.OrderByDescending(c => Math.Abs(c.StepsTo - (currSteps / 2))).Last().Pos;
			Console.WriteLine($"Furthest segment:\t{furthestSegment}\nSegment distance:\t{currSteps / 2}");
		}

		public static void RunP2(string[] args)
		{
			string[] lines = File.ReadAllLines("../input10.txt");
			int mWidth = lines[0].Length, mHeight = lines.Length;
			char[][] pipeMatrix = new char[mWidth][];
			for (int i = 0; i < mWidth; i++)
				pipeMatrix[i] = new char[mHeight];

			// Assign pipeMatrix values
			for (int x = 0; x < mWidth; x++)
				for (int y = 0; y < mHeight; y++)
					pipeMatrix[x][y] = lines[y][x];

			// Retrieve coords of start, and first connecting pipe
			int yCoord = -1;
			Vector2 startPos = new(Array.FindIndex(pipeMatrix, col =>
			{
				int yC = Array.IndexOf(col, START_CHAR);
				if (yC != -1)
				{
					yCoord = yC;
					return true;
				}
				return false;
			}), yCoord);

			List<Vector2> pipeLoopCoords = new() { startPos };
			(Vector2 Pos, char PipeChar, Vector2 DirEntered) firstAdjPipe = default;
			for (int x = -1; x < 2; x++)
			{
				for (int y = -1; y < 2; y++)
				{
					int xPos = Math.Clamp((int)startPos.X + x, 0, mWidth - 1), yPos = Math.Clamp((int)startPos.Y + y, 0, mHeight - 1);
					var coord = (new Vector2(xPos, yPos), pipeMatrix[xPos][yPos]);
					if (PipeConnectsTo(coord, (startPos, START_CHAR)))
					{
						firstAdjPipe = (coord.Item1, coord.Item2, startPos - new Vector2(xPos, yPos));
						goto ContL1;
					}
				}
			}
		ContL1:;

			// Follow and log pipes until start is reached again
			(Vector2 Pos, char PipeChar, Vector2 DirEntered) currPipe = firstAdjPipe;
			pipeLoopCoords.Add(currPipe.Pos);
			while (currPipe.Item2 != START_CHAR)
			{
				// Look at remaining directions for next pipe
				var remainingDirs = ValidAttatchments[currPipe.PipeChar].Where(d => currPipe.DirEntered != d);
				foreach (Vector2 dir in remainingDirs)
				{
					Vector2 pos = currPipe.Pos + dir;
					char pipeChar = pipeMatrix[(int)pos.X][(int)pos.Y];
					if (PipeConnectsTo((currPipe.Pos, currPipe.PipeChar), (pos, pipeChar)))
					{
						currPipe = (pos, pipeChar, -dir);
						pipeLoopCoords.Add(pos);
						break;
					}
				}
			}

			// For each row, switch between out and in and determine total ground tiles enclosed horizontally
			List<Vector2> horizontallyEnclosed = new();
			for (int y = 0; y < mHeight; y++)
			{
				int? openingX = null, closingX = null;
				for (int x = 0; x < mWidth; x++)
				{
					if (pipeLoopCoords.Contains(new(x, y)))
					{
						if (openingX == null)
							openingX = x;
						else if (closingX == null)
						{
							closingX = x;
							for (int i = openingX.Value + 1; i < closingX.Value; i++)
								horizontallyEnclosed.Add(new(i, y));
							openingX = closingX = null;
						}
					}
				}
			}

			// For each column, determine what of the horizontally enclosed is also enclosed vertically
			long totalInsideArea = 0;
			for (int x = 0; x < mWidth; x++)
			{
				int? openingY = null, closingY = null;
				for (int y = 0; y < mHeight; y++)
				{
					if (pipeLoopCoords.Contains(new(x, y)))
					{
						if (openingY == null)
							openingY = y;
						else if (closingY == null)
						{
							closingY = y;
							for (int i = openingY.Value + 1; i < closingY.Value; i++)
							{
								if (horizontallyEnclosed.Contains(new(x, i)))
									totalInsideArea++;
							}
							openingY = closingY = null;
						}
					}
				}
			}

			Console.WriteLine($"Total Inner Area:\t{totalInsideArea}");
		}
	}
}
