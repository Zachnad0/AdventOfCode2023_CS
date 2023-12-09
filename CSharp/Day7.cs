using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSharp
{
	public class Day7 : IDailyProgram
	{
		private delegate bool HandIsType(string hand);
		//private static readonly HandIsType[] HandTypeChecks = new HandIsType[] {
		//	check5 => check5.Any(l => check5.Count(ol => l == ol) == 5),
		//	check4 => check4.Any(l => check4.Count(ol => l == ol) == 4),
		//	fullHouse => fullHouse.Any(l => fullHouse.Count(ol => l == ol) == 3) && fullHouse.Any(l => fullHouse.Count(ol => l == ol) == 2),
		//	check3 => check3.Any(l => check3.Count(ol => l == ol) == 3),
		//	pair2 => pair2.Count(l => pair2.Count(ol => l == ol) == 2) == 4,
		//	pair1 => pair1.Any(l => pair1.Count(ol => l == ol) == 2),
		//	highCard => true,
		//};
		private static readonly HandIsType[] HandTypeChecks = new HandIsType[] {
			check5 => check5.Any(l => l != JCard && check5.Count(l.Equals) + check5.Count(JCard.Equals) == 5) || check5.All(JCard.Equals),
			check4 => check4.Any(l => l != JCard && check4.Count(l.Equals) + check4.Count(JCard.Equals) == 4),
			fullHouse =>
			{
				char firstPairChar = fullHouse.FirstOrDefault(l => fullHouse.Count(l.Equals) + fullHouse.Count(JCard.Equals) == 3);
				bool c2 = fullHouse.Any(l => l != firstPairChar && l != JCard && fullHouse.Count(l.Equals) == 2);
				return c2 && firstPairChar != default;
			},
			check3 => check3.Any(l => check3.Count(l.Equals) + check3.Count(JCard.Equals) == 3),
			pair2 => pair2.Count(l => pair2.Count(l.Equals) == 2) == 4,
			pair1 => pair1.Any(l => l != JCard && pair1.Count(l.Equals) + pair1.Count(JCard.Equals) == 2),
			highCard => true,
		};
		private static readonly char[] CardTypes = { 'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2' };
		private const char JCard = 'J';

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
				for (int s = HandTypeChecks.Length; s > 0; s--)
				{
					if (HandTypeChecks[^s](hand))
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
			for (int i = 0; i < handSpecs.Count; i++)
				totalWinnings += handSpecs[i].Bid * (i + 1);

			Console.WriteLine($"Total winnings:\n{totalWinnings}");
		}

		public static void RunP2(string[] args)
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
				for (int s = HandTypeChecks.Length; s > 0; s--)
				{
					if (HandTypeChecks[^s](hand))
					{
						// Generate strength score array
						strength[0] = s;
						for (int cardN = 0; cardN < HAND_LENGTH; cardN++)
						{
							if (hand[cardN] != JCard)
								strength[cardN + 1] = CardTypes.Length - Array.IndexOf(CardTypes, hand[cardN]);
							else
								strength[cardN + 1] = -1;
						}

						if (hand.Any(JCard.Equals))
							Console.WriteLine($"[{hand}] is {s switch
							{
								7 => "5-OAK",
								6 => "4-OAK",
								5 => "Full House",
								4 => "3-OAK",
								3 => "2-Pairs",
								2 => "1-Pair",
								1 => "Highest Card"
							}}");
						break;
					}
				}

				handSpecs.Add((hand, bid, strength));
			}

			int totalWinnings = 0;
			handSpecs = handSpecs.OrderBy<(string, int, int[] Strength), int[]>(s => s.Strength, new ArrCompare<int>()).ToList();
			for (int i = 0; i < handSpecs.Count; i++)
				totalWinnings += handSpecs[i].Bid * (i + 1);

			Console.WriteLine($"Total winnings:\n{totalWinnings}");
		}
	}
}
