using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSharp
{
	public class Day7 : IDailyProgram
	{
		private delegate bool HandIsType(string hand);
		private static readonly HandIsType[] HandTypeChecks = new HandIsType[] {
			check5 => check5.Any(l => check5.Count(ol => l == ol) == 5),
			check4 => check4.Any(l => check4.Count(ol => l == ol) == 4),
			fullHouse => fullHouse.Any(l => fullHouse.Count(ol => l == ol) == 3) && fullHouse.Any(l => fullHouse.Count(ol => l == ol) == 2),
			check3 => check3.Any(l => check3.Count(ol => l == ol) == 3),
			pair2 => pair2.Count(l => pair2.Count(ol => l == ol) == 2) == 4,
			pair1 => pair1.Any(l => pair1.Count(ol => l == ol) == 2),
			highCard => true,
		};
		private static readonly char[] CardTypes = { 'A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2' };
		private class ArrCompare<T> : IComparer<T[]> where T : IComparable
		{
			public int Compare(T[]? x, T[]? y)
			{
				int result = 0, i = 0;
				while (result == 0)
				{
					if (i >= x.Length || i >= y.Length)
						return result;
					result = x[i].CompareTo(y[i]);
					i++;
				}
				return result;

			}
		}

		public static void RunP1(string[] args)
		{
			string[] lines = File.ReadAllLines("../input7.txt");
			const int HAND_LENGTH = 5;

			// Determine strengths and bid amounts
			int lineCount = lines.Length;
			List<(string Hand, int Bid, int[] Strength)> handSpecs = new();

			for (int l = 0; l < lineCount; l++)
			{
				string hand = new(lines[l].Take(HAND_LENGTH).ToArray());
				int bid = int.Parse(lines[l].Skip(HAND_LENGTH + 1).ToArray());
				int[] strength = new int[HAND_LENGTH + 1];

				// Test hand with conditions to determine strength
				for (int s = HandTypeChecks.Length - 1; s >= 0; s--)
				{
					if (HandTypeChecks[s](hand))
					{
						// Generate strength score array
						strength[0] = s;
						for (int cardN = 0; cardN < HAND_LENGTH; cardN++)
						{
							strength[cardN + 1] = CardTypes.Length - Array.IndexOf(CardTypes, hand[cardN]);
						}
						break;
					}
				}

				handSpecs.Add((hand, bid, strength));
			}

			int totalWinnings = 0;
			handSpecs = handSpecs.OrderBy<(string, int, int[] Strength), int[]>(s => s.Strength, new ArrCompare<int>()).ToList();
			handSpecs.Reverse();
			for (int i = 0; i < handSpecs.Count; i++)
				totalWinnings += handSpecs[i].Bid * (i + 1);

			Console.WriteLine($"Total winnings: {totalWinnings}");
		}

		public static void RunP2(string[] args)
		{
		}
	}
}
