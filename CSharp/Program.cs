using System;
using System.Reflection;

namespace CSharp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			//Day1.RunP1(args);
			//Day1.RunP2(args);
			//Day2.RunP1(args);
			//Day2.RunP2(args);
			//Day3.RunP1(args);
			//Day3.RunP2(args);
			//Day4.RunP1(args);
			//Day4.RunP2(args);
			//Day5.RunP1(args);
			//Day5.RunP2(args);
			//Day6.RunP1(args);
			//Day6.RunP2(args);
			//Day7.RunP1(args);
			//Day7.RunP2(args);
			//Day8.RunP1(args);
			//Day8.RunP2(args);
			//Day9.RunP1(args);
			//Day9.RunP2(args);
			//Day10.RunP1(args);
			//Day10.RunP2(args);
			//Day11.RunP1(args);
			//Day11.RunP2(args);
			//Day12.RunP1(args);
			//Day12.RunP2(args);
			//Day13.RunP1(args);
			//Day13.RunP2(args);
			//Day14.RunP1(args);
			//Day14.RunP2(args);
			//Day15.RunP1(args);
			//Day15.RunP2(args);
			//Day16.RunP1(args);
			Day16.RunP2(args);

			Console.ReadKey();
		}
	}

	public static class UsefulAoCUtils // TODO add these to ZUtilLib
	{
		public static char[][] ToMatrix(this string[] lines)
		{
			int mWidth = lines[0].Length, mHeight = lines.Length;
			char[][] outMatrix = new char[mWidth][];
			for (int x = 0; x < mWidth; x++)
			{
				outMatrix[x] = new char[mHeight];
				for (int y = 0; y < mHeight; y++)
				{
					outMatrix[x][y] = lines[y][x];
				}
			}

			return outMatrix;
		}

		public static string ToString<T>(this T[][] matrix, string horizSeperator = "\t", string vertSeperator = "\n")
		{
			string outputString = "";
			int mWidth = matrix.Length, mHeight = matrix[0].Length; ;
			for (int y = 0; y < mHeight; y++)
			{
				for (int x = 0; x < mWidth; x++)
					outputString += $"{(x != 0 ? horizSeperator : "")}{matrix[x][y]}";
				outputString += y != mWidth - 1 ? vertSeperator : "";
			}

			return outputString;
		}

		/// <summary>
		/// Rotates the matrix 90 degrees if <paramref name="clockwise"/>, otherwise counter-clockwise.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="clockwise"></param>
		/// <returns>A new array containing the elements in rotated position</returns>
		public static T[][] RotateMatrix<T>(this T[][] matrix, bool clockwise = true)
		{
			int origMHeight = matrix[0].Length, origMWidth = matrix.Length;
			T[][] outMatrix = new T[origMHeight][];
			for (int i = 0; i < origMHeight; i++)
				outMatrix[i] = new T[origMWidth];

			for (int origMY = 0; origMY < origMHeight; origMY++)
			{
				for (int origMX = 0; origMX < origMWidth; origMX++)
				{
					if (clockwise)
						outMatrix[origMHeight - 1 - origMY][origMX] = matrix[origMX][origMY];
					else
						outMatrix[origMY][origMWidth - 1 - origMX] = matrix[origMX][origMY];
				}
			}

			return outMatrix;
		}

		public static bool Identical<T>(this T[][] matrix, T[][] otherMatrix)
		{
			if (matrix is null || otherMatrix is null || matrix.Length != otherMatrix.Length || matrix[0].Length != otherMatrix[0].Length)
				return false;

			bool equivelant = true; // If the previous matrix is the same as this matrix, then we must be in a perpetually looping loop.
			for (int y = 0; y < matrix[0].Length && equivelant; y++)
				for (int x = 0; x < matrix.Length && equivelant; x++)
					if (!matrix[x][y].Equals(otherMatrix[x][y]))
						equivelant = false;

			return equivelant;
		}
	}
}
