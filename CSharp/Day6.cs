using System;
using System.IO;
using System.Linq;
using ZUtilLib;

namespace CSharp
{
	public class Day6 : IDailyProgram
	{
		public static void RunP1(string[] args)
		{
			string[] lines = File.ReadAllLines("../input6.txt");
			int[] times = lines[0].Split(new[] { ':', ' ' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToArray();
			int[] distances = lines[1].Split(new[] { ':', ' ' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToArray();

			int resultProduct = 1;
			for (int r = 0; r < times.Length; r++)
			{
				// Test every hold time for race
				int nOfSuccessCases = 0;
				for (int holdTime = 1; holdTime < times[r]; holdTime++)
				{
					if ((times[r] - holdTime) * holdTime > distances[r])
						nOfSuccessCases++;
				}
				resultProduct *= nOfSuccessCases;
			}

			Console.WriteLine(resultProduct);
		}

		public static void RunP2(string[] args)
		{
			string[] lines = File.ReadAllLines("../input6.txt");
			long raceTime = long.Parse(lines[0].FilterNumbers(false));
			long distToBeat = long.Parse(lines[1].FilterNumbers(false));

			// Test every hold time for race
			int nOfSuccessCases = 0;
			for (long holdTime = 1; holdTime < raceTime; holdTime++)
			{
				if ((raceTime - holdTime) * holdTime > distToBeat)
					nOfSuccessCases++;
			}

			Console.WriteLine(nOfSuccessCases);
		}
	}
}
