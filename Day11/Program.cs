using Day11;

ObservationAnalyzer analyzer = new("input.txt");

long sum = analyzer.GetSumOfShortestPaths();

Console.WriteLine($"Part 1: {sum}");

long sumNewRules = analyzer.GetSumOfShortestPaths(1000000);

Console.WriteLine($"Part 2: {sumNewRules}");