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
			//Day13.RunP2(args); // TODO Days 14 up to and including 18

			Day19.RunP1(args);

			Console.ReadKey();
		}
	}

	public static class UsefulAoCUtils
	{
		// TODO add to ZUtilLib
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
	}
}
