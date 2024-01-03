using Day09;

OasisReportAnalyzer analyzer = new("input.txt");

int sum = analyzer.GetSumOfPredictions();

Console.WriteLine($"Part 1: {sum}");