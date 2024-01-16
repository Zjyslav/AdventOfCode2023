using Day12;

ConditionRecordsAnalyzer analyzer = new("input.txt");

int sum = analyzer.GetSumOfPossibleArrangements();

Console.WriteLine($"Part 1: {sum}");