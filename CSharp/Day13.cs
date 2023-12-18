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

			// Get all of the matrices of the sections
			List<char[][]> sectionMatrices = new();
			{
				List<int> sectionSplitIndices = new();
				for (int i = 0; i < lines.Length; i++)
					if (lines[i] == string.Empty)
						sectionSplitIndices.Add(i);
				sectionSplitIndices.Add(lines.Length);

				// Map characters per section
				int prevIndex = 0;
				foreach (int index in sectionSplitIndices)
				{
					char[][] thisMatrix = new char[lines[index - 1].Length][];
					for (int x = 0; x < thisMatrix.Length; x++)
					{
						thisMatrix[x] = new char[index - prevIndex];
						for (int y = 0; y < thisMatrix[x].Length; y++)
						{
							thisMatrix[x][y] = lines[prevIndex + y][x];
						}
					}

					sectionMatrices.Add(thisMatrix);
					prevIndex = index + 1;
				}
			}

			// Find the mirror col and row coords for each section, by comparing the left-side array with a reversed right side. Crop to suit smaller side.
			long totalSum = 0;
			foreach (char[][] groundMatrix in sectionMatrices)
			{
				int nOfColsLeftOfMirror = 0, nOfRowsAboveMirror = 0;

				int colCount = groundMatrix.Length - 1;
				for (int col = 0; col < colCount; col++) // Cols
				{
					char[][] leftSide, rightSide;
					if (col > colCount / 2) // Left is bigger
					{
						leftSide = groundMatrix[(2 * col - colCount + 1)..(col + 1)];
						rightSide = groundMatrix[(col + 1)..^0];
					}
					else if (col < colCount / 2) // Right is bigger
					{
						leftSide = groundMatrix[0..(col + 1)];
						rightSide = groundMatrix[(col + 1)..(2 * (col + 1))];
					}
					else // Same
					{
						leftSide = groundMatrix[(colCount - 2 * col + 1)..(col + 1)];
						rightSide = groundMatrix[(col + 1)..^0];
					}

					int nOfDifferences = 0;
					for (int x = 0; x < leftSide.Length && nOfDifferences <= 0; x++)
					{
						for (int y = 0; y < leftSide[0].Length; y++)
						{
							if (leftSide[x][y] != rightSide[^(x + 1)][y]) // Read horizontally inverse
							{
								nOfDifferences++;
							}
						}
					}

					if (nOfDifferences == 0)
					{
						nOfColsLeftOfMirror += col + 1;
						goto EndSection;
					}
				}

				int rowCount = groundMatrix[0].Length - 1;
				for (int row = 0; row < rowCount; row++) // Rows
				{
					char[][] topSide = new char[groundMatrix.Length][], bottomSide = new char[groundMatrix.Length][];
					for (int c = 0; c < groundMatrix.Length; c++)
					{
						if (row > rowCount / 2) // Top is bigger
						{
							topSide[c] = groundMatrix[c][(2 * row - rowCount + 1)..(row + 1)];
							bottomSide[c] = groundMatrix[c][(row + 1)..^0];
						}
						else if (row < rowCount / 2) // Bottom is bigger
						{
							topSide[c] = groundMatrix[c][0..(row + 1)];
							bottomSide[c] = groundMatrix[c][(row + 1)..(2 * (row + 1))];
						}
						else // Same
						{
							topSide[c] = groundMatrix[c][(rowCount - 2 * row + 1)..(row + 1)];
							bottomSide[c] = groundMatrix[c][(row + 1)..^0];
						}
					}

					int nOfDifferences = 0;
					for (int x = 0; x < topSide.Length && nOfDifferences <= 0; x++)
					{
						for (int y = 0; y < topSide[0].Length; y++)
						{
							if (topSide[x][y] != bottomSide[x][^(y + 1)]) // Read vertically inverse
							{
								nOfDifferences++;
							}
						}
					}

					if (nOfDifferences == 0)
					{
						nOfRowsAboveMirror += row + 1;
						goto EndSection;
					}
				}

			EndSection:
				totalSum += nOfColsLeftOfMirror + 100 * nOfRowsAboveMirror;
			}

			Console.WriteLine($"Final answer is: {totalSum}");
		}

		public static void RunP2(string[] args)
		{
			string[] lines = File.ReadAllLines("../input13.txt");

			// Get all of the matrices of the sections
			List<char[][]> sectionMatrices = new();
			{
				List<int> sectionSplitIndices = new();
				for (int i = 0; i < lines.Length; i++)
					if (lines[i] == string.Empty)
						sectionSplitIndices.Add(i);
				sectionSplitIndices.Add(lines.Length);

				// Map characters per section
				int prevIndex = 0;
				foreach (int index in sectionSplitIndices)
				{
					char[][] thisMatrix = new char[lines[index - 1].Length][];
					for (int x = 0; x < thisMatrix.Length; x++)
					{
						thisMatrix[x] = new char[index - prevIndex];
						for (int y = 0; y < thisMatrix[x].Length; y++)
						{
							thisMatrix[x][y] = lines[prevIndex + y][x];
						}
					}

					sectionMatrices.Add(thisMatrix);
					prevIndex = index + 1;
				}
			}

			// Find the mirror col and row coords for each section, by comparing the left-side array with a reversed right side. Crop to suit smaller side.
			long totalSum = 0;
			foreach (char[][] groundMatrix in sectionMatrices)
			{
				int nOfColsLeftOfMirror = 0, nOfRowsAboveMirror = 0;

				int colCount = groundMatrix.Length - 1;
				for (int col = 0; col < colCount; col++) // Cols
				{
					char[][] leftSide, rightSide;
					if (col > colCount / 2) // Left is bigger
					{
						leftSide = groundMatrix[(2 * col - colCount + 1)..(col + 1)];
						rightSide = groundMatrix[(col + 1)..^0];
					}
					else if (col < colCount / 2) // Right is bigger
					{
						leftSide = groundMatrix[0..(col + 1)];
						rightSide = groundMatrix[(col + 1)..(2 * (col + 1))];
					}
					else // Same
					{
						leftSide = groundMatrix[(colCount - 2 * col + 1)..(col + 1)];
						rightSide = groundMatrix[(col + 1)..^0];
					}

					int nOfDifferences = 0;
					for (int x = 0; x < leftSide.Length && nOfDifferences <= 1; x++)
					{
						for (int y = 0; y < leftSide[0].Length; y++)
						{
							if (leftSide[x][y] != rightSide[^(x + 1)][y]) // Read horizontally inverse
							{
								nOfDifferences++;
							}
						}
					}

					if (nOfDifferences == 1)
					{
						nOfColsLeftOfMirror += col + 1;
						goto EndSection;
					}
				}

				int rowCount = groundMatrix[0].Length - 1;
				for (int row = 0; row < rowCount; row++) // Rows
				{
					char[][] topSide = new char[groundMatrix.Length][], bottomSide = new char[groundMatrix.Length][];
					for (int c = 0; c < groundMatrix.Length; c++)
					{
						if (row > rowCount / 2) // Top is bigger
						{
							topSide[c] = groundMatrix[c][(2 * row - rowCount + 1)..(row + 1)];
							bottomSide[c] = groundMatrix[c][(row + 1)..^0];
						}
						else if (row < rowCount / 2) // Bottom is bigger
						{
							topSide[c] = groundMatrix[c][0..(row + 1)];
							bottomSide[c] = groundMatrix[c][(row + 1)..(2 * (row + 1))];
						}
						else // Same
						{
							topSide[c] = groundMatrix[c][(rowCount - 2 * row + 1)..(row + 1)];
							bottomSide[c] = groundMatrix[c][(row + 1)..^0];
						}
					}

					int nOfDifferences = 0;
					for (int x = 0; x < topSide.Length && nOfDifferences <= 1; x++)
					{
						for (int y = 0; y < topSide[0].Length; y++)
						{
							if (topSide[x][y] != bottomSide[x][^(y + 1)]) // Read vertically inverse
							{
								nOfDifferences++;
							}
						}
					}

					if (nOfDifferences == 1)
					{
						nOfRowsAboveMirror += row + 1;
						goto EndSection;
					}
				}

			EndSection:
				totalSum += nOfColsLeftOfMirror + 100 * nOfRowsAboveMirror;
			}

			Console.WriteLine($"Final answer is: {totalSum}");
		}
	}
}
