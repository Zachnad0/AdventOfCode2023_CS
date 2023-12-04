using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp
{
	public class Day4 : IDailyProgram
	{
		public static void RunP1(string[] args)
		{
			string[] lines = File.ReadAllLines("../input4.txt");

			int totalPoints = 0;
			for (int l = 0; l < lines.Length; l++)
			{
				// Each line is a card
				int cardValue = 0;
				string[] clauses = lines[l].Split(':', '|').Select(c => Day1.FindAndReplace(new() { { "  ", " " } }, c.Trim())).ToArray(); // card, winning #s, your #s
				int[] wNumbers = clauses[1].Split(' ', StringSplitOptions.TrimEntries).Select(int.Parse).ToArray();
				int[] currNumbers = clauses[2].Split(' ', StringSplitOptions.TrimEntries).Select(int.Parse).ToArray();
				int[] inCommon = wNumbers.Intersect(currNumbers).ToArray();
				for (int i = 0; i < inCommon.Length; i++)
					cardValue += i == 0 ? 1 : cardValue;

				totalPoints += cardValue;
			}

			Console.WriteLine(totalPoints);
		}

		public static void RunP2(string[] args)
		{
			string[] lines = File.ReadAllLines("../input4.txt");
			Dictionary<int, int> cardLineAndAmount = new();
			for (int l = 0; l < lines.Length; l++)
				cardLineAndAmount.Add(l, 1);

			for (int l = 0; l < lines.Length; l++)
			{
				// Each line is a card
				string[] clauses = lines[l].Split(':', '|').Select(c => Day1.FindAndReplace(new() { { "  ", " " } }, c.Trim())).ToArray(); // card, winning #s, your #s
				int[] wNumbers = clauses[1].Split(' ', StringSplitOptions.TrimEntries).Select(int.Parse).ToArray();
				int[] currNumbers = clauses[2].Split(' ', StringSplitOptions.TrimEntries).Select(int.Parse).ToArray();
				int[] inCommon = wNumbers.Intersect(currNumbers).ToArray();

				int currCardCount = cardLineAndAmount.TryGetValue(l, out int c) ? c : 0;
				for (int i = 1; i <= inCommon.Length; i++)
				{
					if (!cardLineAndAmount.TryAdd(l + i, currCardCount))
						cardLineAndAmount[l + i] += currCardCount;
				}
			}

			Console.WriteLine(cardLineAndAmount.Sum(pair => pair.Value));
		}
	}
}
