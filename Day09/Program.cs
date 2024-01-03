using Day09;

OasisReportAnalyzer analyzer = new("input.txt");

int sum = analyzer.GetSumOfPredictions();

Console.WriteLine($"Part 1: {sum}");

int sumPast = analyzer.GetSumOfPredictions(true);

Console.WriteLine($"Part 2: {sumPast}");