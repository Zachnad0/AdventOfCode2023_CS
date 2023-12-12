using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSharp
{
	public class Day9 : IDailyProgram
	{
		public static void RunP1(string[] args)
		{
			string[] lines = File.ReadAllLines("../input9.txt");

			// For each history/line, predict next extrapolated value
			long sumOfExtrVals = 0;
			for (int l = 0; l < lines.Length; l++)
			{
				long[] baseHistory = lines[l].Split(' ').Select(long.Parse).ToArray();
				long[][] historyAndDiffs;
				// Determine layers needed
				long diffLayers = 0, diffSum = 1;
				long[] prevHistory = baseHistory;
				while (diffSum != 0)
				{
					long[] newHistory = new long[prevHistory.Length - 1];
					for (int i = 1; i < prevHistory.Length; i++)
						newHistory[i - 1] = prevHistory[i] - prevHistory[i - 1];

					prevHistory = newHistory;
					diffSum = newHistory.Sum();
					diffLayers++;
				}
				historyAndDiffs = new long[diffLayers + 1][];
				historyAndDiffs[0] = baseHistory;
				for (int i = 1; i < historyAndDiffs.Length; i++)
				{
					historyAndDiffs[i] = new long[historyAndDiffs[i - 1].Length - 1];
					for (int j = 0; j < historyAndDiffs[i].Length; j++)
						historyAndDiffs[i][j] = historyAndDiffs[i - 1][j + 1] - historyAndDiffs[i - 1][j];
				}

				// Extrapolate
				long[][] extrapolatedHist = new long[historyAndDiffs.Length][]; // Same height
																				// No need to extend zero-layer
				extrapolatedHist[^1] = [0];
				for (int i = extrapolatedHist.Length - 2; i >= 0; i--)
				{
					extrapolatedHist[i] = (long[])historyAndDiffs[i].Clone();
					Array.Resize(ref extrapolatedHist[i], extrapolatedHist[i].Length + 1);
					extrapolatedHist[i][^1] = extrapolatedHist[i][^2] + extrapolatedHist[i + 1][^1];
				}

				sumOfExtrVals += extrapolatedHist[0][^1];
			}

			Console.WriteLine($"Sum: {sumOfExtrVals}");
		}

		public static void RunP2(string[] args)
		{
			string[] lines = File.ReadAllLines("../input9.txt");

			// For each history/line, predict next extrapolated value
			long sumOfExtrVals = 0;
			for (int l = 0; l < lines.Length; l++)
			{
				long[] baseHistory = lines[l].Split(' ').Select(long.Parse).ToArray();
				long[][] historyAndDiffs;
				// Determine layers needed+
				long diffLayers = 0, diffSum = 1;
				long[] prevHistory = baseHistory;
				while (diffSum != 0)
				{
					long[] newHistory = new long[prevHistory.Length - 1];
					for (int i = 1; i < prevHistory.Length; i++)
						newHistory[i - 1] = prevHistory[i] - prevHistory[i - 1];

					prevHistory = newHistory;
					diffSum = newHistory.Sum();
					diffLayers++;
				}
				historyAndDiffs = new long[diffLayers + 1][];
				historyAndDiffs[0] = baseHistory;
				for (int i = 1; i < historyAndDiffs.Length; i++)
				{
					historyAndDiffs[i] = new long[historyAndDiffs[i - 1].Length - 1];
					for (int j = 0; j < historyAndDiffs[i].Length; j++)
						historyAndDiffs[i][j] = historyAndDiffs[i - 1][j + 1] - historyAndDiffs[i - 1][j];
				}

				// Extrapolate
				List<long>[] extrapolatedHist = new List<long>[historyAndDiffs.Length]; // Same height

				extrapolatedHist[^1] = [0];
				for (int i = extrapolatedHist.Length - 2; i >= 0; i--)
				{
					extrapolatedHist[i] = historyAndDiffs[i].ToList();
					extrapolatedHist[i].Insert(0, extrapolatedHist[i][0] - extrapolatedHist[i + 1][0]);
				}

				sumOfExtrVals += extrapolatedHist[0][0];
			}

			Console.WriteLine($"Sum: {sumOfExtrVals}");
		}
	}
}
