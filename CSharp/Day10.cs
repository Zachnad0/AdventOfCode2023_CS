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
			{UD_PIPE,[new(0,1),new(0,-1)]}, {LR_PIPE,[new(1,0),new(-1,0)]}, {UR_PIPE,[new(0,1),new(1,0)]}, {UL_PIPE,[new(0,1),new(-1,0)]}, {DR_PIPE,[new(0,-1),new(1,0)]}, {DL_PIPE,[new(0,-1),new(-1,0)]}, {START_CHAR,[new(0,1),new(0,-1),new(-1,0),new(1,0)]}, {GRND_CHAR,[]}
		};

		/// <summary>
		/// Check if this pipe connects to other, and other connects to this.
		/// </summary>
		private static bool PipeConnectsTo((Vector2 Pos, char PipeChar) startPipe, (Vector2 Pos, char PipeChar) nextPipe)
		{
			if (Vector2.Distance(startPipe.Pos, nextPipe.Pos) > 1)
				return false; // If not adjacent, then does not connect

			Vector2 relPos = Vector2.Normalize(nextPipe.Pos - startPipe.Pos);
			bool isStartPValid = ValidAttatchments.TryGetValue(startPipe.PipeChar, out Vector2[]? attchDirs1) && attchDirs1.Contains(relPos);
			bool isNextValid = ValidAttatchments.TryGetValue(nextPipe.PipeChar, out Vector2[]? attchDirs2) && attchDirs2.Contains(-relPos);

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

			// Retrieve coords of start, then find all coordinates of pipe pieces part of loop
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

			List<Vector2> pipeCoords = new();
			(Vector2, char) firstAdjPipe;
			for (int x = -1; x < 2; x++)
			{
				for (int y = -1; y < 2; y++)
				{
					int xPos = Math.Clamp((int)startPos.X + x, 0, mWidth - 1), yPos = Math.Clamp((int)startPos.Y + y, 0, mHeight - 1);
					var coord = (new Vector2(xPos, yPos), pipeMatrix[xPos][yPos]);
					if (PipeConnectsTo(coord, (startPos, START_CHAR)))
					{
						firstAdjPipe = coord;
						goto ContL1;
					}
				}
			}
		ContL1:;

			// Get furthest coord by using trig.
			Vector2 furthestSegment = pipeCoords.OrderByDescending(coord => Vector2.Distance(coord, startPos)).First();
			Console.WriteLine($"Furthest segment:\n\t{furthestSegment}");

			// Find steps to furthest segment
		}

		public static void RunP2(string[] args)
		{

		}
	}
}
