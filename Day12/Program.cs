using Day12;

ConditionRecordsAnalyzer analyzer = new("input.txt");

int sum = analyzer.GetSumOfPossibleArrangements();

Console.WriteLine($"Part 1: {sum}");

analyzer = new("input.txt", 5);

int sumWithCopies = analyzer.GetSumOfPossibleArrangements();

Console.WriteLine($"Part 2: {sumWithCopies}");