using System;
using System.Collections.Generic;
using System.Globalization;

class Program
{
	static void Main()
	{
		string input =
			"9191896883-9191940271,457499-518693,4952-6512,960-1219," +
			"882220-1039699,2694-3465,3818-4790,166124487-166225167," +
			"759713819-759869448,4821434-4881387,7271-9983,1182154-1266413," +
			"810784-881078,802-958,1288-1491,45169-59445,25035-29864," +
			"379542-433637,287-398,75872077-75913335,653953-689335," +
			"168872-217692,91-113,475-590,592-770,310876-346156," +
			"2214325-2229214,85977-112721,51466993-51620441," +
			"8838997-8982991,534003-610353,32397-42770,17-27," +
			"68666227-68701396,1826294188-1826476065,1649-2195," +
			"141065204-141208529,7437352-7611438,10216-13989," +
			"33-44,1-16,49-74,60646-73921,701379-808878";

		long partOneSum = 0;
		long partTwoSum = 0;

		foreach (var range in input.Split(','))
		{
			var parts = range.Split('-');
			long start = long.Parse(parts[0], CultureInfo.InvariantCulture);
			long end = long.Parse(parts[1], CultureInfo.InvariantCulture);

			var partOneNumbers = GetPartOneNumbersInRange(start, end);
			long sumPartOne = 0;
			foreach (var n in partOneNumbers) sumPartOne += n;
			partOneSum += sumPartOne;

			var partTwoNumbers = GetPartTwoNumbersInRange(start, end);

			partTwoNumbers.ExceptWith(partOneNumbers);

			long sumPartTwo = sumPartOne;
			foreach (var n in partTwoNumbers) sumPartTwo += n;
			partTwoSum += sumPartTwo;
		}

		Console.WriteLine("Part One: " + partOneSum);
		Console.WriteLine("Part Two: " + partTwoSum);
	}

	static HashSet<long> GetPartOneNumbersInRange(long start, long end)
	{
		var numbers = new HashSet<long>();
		int minDigits = start.ToString().Length;
		int maxDigits = end.ToString().Length;

		for (int digits = minDigits; digits <= maxDigits; digits++)
		{
			if (digits % 2 != 0) continue;
			int halfLength = digits / 2;
			long minHalf = Pow10(halfLength - 1);
			long maxHalf = Pow10(halfLength) - 1;

			for (long half = minHalf; half <= maxHalf; half++)
			{
				long n = MakeRepeatedNumber(half, halfLength);
				if (n >= start && n <= end)
					numbers.Add(n);
			}
		}

		return numbers;
	}

	static HashSet<long> GetPartTwoNumbersInRange(long start, long end)
	{
		var numbers = new HashSet<long>();
		int minDigits = start.ToString().Length;
		int maxDigits = end.ToString().Length;

		for (int digits = minDigits; digits <= maxDigits; digits++)
		{
			for (int repeat = 2; repeat <= digits; repeat++)
			{
				if (digits % repeat != 0) continue;

				int blockLen = digits / repeat;
				long minBlock = Pow10(blockLen - 1);
				long maxBlock = Pow10(blockLen) - 1;

				for (long block = minBlock; block <= maxBlock; block++)
				{
					long value = RepeatBlock(block, blockLen, repeat);
					if (value >= start && value <= end)
						numbers.Add(value);
				}
			}
		}

		return numbers;
	}

	static long MakeRepeatedNumber(long half, int halfLength)
	{
		long multiplier = Pow10(halfLength);
		return half * multiplier + half;
	}

	static long RepeatBlock(long block, int blockLen, int count)
	{
		long result = 0;
		long multiplier = Pow10(blockLen);
		for (int i = 0; i < count; i++)
			result = result * multiplier + block;
		return result;
	}

	static long Pow10(int exp)
	{
		long result = 1;
		for (int i = 0; i < exp; i++) result *= 10;
		return result;
	}
}
