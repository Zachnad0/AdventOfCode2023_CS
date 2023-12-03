using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZUtilLib;

namespace CSharp
{
	public class Day1 : IDailyProgram
	{
		public static void RunP1(string[] args)
		{
			int sum = 0;
			string text = File.ReadAllText("../input.txt");
			string[] lines = text.Split('\n');
			for (int l = 0; l < lines.Length; l++)
			{
				Console.WriteLine(lines[l]);
				string numbers = lines[l].FilterNumbers(false);
				string value = string.Join("", numbers.First(), numbers.Last());
				sum += int.Parse(value);
				Console.WriteLine(value);
			}
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(sum);
		}

		public static void RunP2(string[] args)
		{
			int sum = 0;
			string text = File.ReadAllText("../input.txt");
			string[] lines = text.Split('\n').Select(l => l.ToLower()).ToArray();
			Dictionary<string, string> digits = new() { { "zero", "0" }, { "one", "1" }, { "two", "2" }, { "three", "3" }, { "four", "4" }, { "five", "5" }, { "six", "6" }, { "seven", "7" }, { "eight", "8" }, { "nine", "9" } };

			for (int l = 0; l < lines.Length; l++)
			{
				string line = lines[l];
				string numbers = FindAndReplace(digits, line).FilterNumbers(false);
				string value = string.Join("", numbers.First(), numbers.Last());
				sum += int.Parse(value);

				Console.WriteLine($"LN: {line}\t\tVal: {value}");
			}
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(sum);
			Console.ForegroundColor = ConsoleColor.Gray;
		}

		// TODO implement this into ZUtilLib! Very useful!!!
		private static string FindAndReplace(Dictionary<string, string> map, string text)
		{
			// Iterate through line, replace "one" with "1", "five" with "5" and etcetera
			string output = new(text);
			for (int i = 0; i < output.Length; i++)
			{
				foreach (KeyValuePair<string, string> keyPair in map)
				{
					int keyLength = keyPair.Key.Length;
					if (i + keyLength > output.Length) continue;
					string extract = output[i..(i + keyLength)];
					// If same, replace
					if (extract == keyPair.Key)
					{
						output = output
							.Remove(i, keyLength)
							.Insert(i, keyPair.Value);
						break;
					}
				}
			}

			return output;
		}
	}
}
