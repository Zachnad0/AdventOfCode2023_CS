using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZUtilLib;

namespace CSharp
{
	public class Day2 : IDailyProgram
	{
		enum Colors { red, green, blue }
		public static void RunP1(string[] args)
		{
			string[] lines = File.ReadAllText("../input2.txt").Split('\n');
			Dictionary<Colors, int> colorAndMax = new() { { Colors.red, 12 }, { Colors.green, 13 }, { Colors.blue, 14 } };

			int sumOfGameNums = 0;
			for (int l = 0; l < lines.Length; l++)
			{
				// For each line is a game, check if game is possible
				string[] clauses = lines[l].Split(':', ',', ';'); // G-No. is first clause
				bool isGameValid = true;
				for (int i = 1; i < clauses.Length; i++)
				{
					int count = int.Parse(clauses[i].FilterNumbers(false));
					Colors color = Enum.Parse<Colors>(clauses[i].FilterNumbers(true).Trim());
					// If NOT valid number for color, set lineValid is false and break
					isGameValid = colorAndMax.TryGetValue(color, out int max) && count <= max;
					if (!isGameValid) break;
				}
				if (isGameValid)
					sumOfGameNums += int.Parse(clauses[0].FilterNumbers(false));
			}

			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"\n\n{sumOfGameNums}");
			Console.ForegroundColor = ConsoleColor.Gray;
		}

		public static void RunP2(string[] args)
		{
			string[] lines = File.ReadAllText("../input2.txt").Split('\n');

			int sumOfSetPowers = 0;
			for (int l = 0; l < lines.Length; l++)
			{
				// For each line is a game, find highest count of each color, then multiply the three values together and add it to sum
				Dictionary<Colors, int> colorAndHighest = new() { { Colors.red, 0 }, { Colors.green, 0 }, { Colors.blue, 0 } };
				string[] clauses = lines[l].Split(':', ',', ';'); // G-No. is first clause

				for (int i = 1; i < clauses.Length; i++)
				{
					int count = int.Parse(clauses[i].FilterNumbers(false));
					Colors color = Enum.Parse<Colors>(clauses[i].FilterNumbers(true).Trim());
					if (colorAndHighest.TryGetValue(color, out int currMax) && count > currMax)
						colorAndHighest[color] = count;
				}

				sumOfSetPowers += colorAndHighest[Colors.red] * colorAndHighest[Colors.green] * colorAndHighest[Colors.blue];
			}

			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"\n\n{sumOfSetPowers}");
			Console.ForegroundColor = ConsoleColor.Gray;
		}
	}
}
