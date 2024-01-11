using Day11;

ObservationAnalyzer analyzer = new("input.txt");

int sum = analyzer.GetSumOfShortestPaths();

Console.WriteLine($"Part 1: {sum}");