using Day14;

PlatformLoadAnalyzer analyzer = new("input.txt");

int load = analyzer.CalculateLoadOnNorthSupportBeams();

Console.WriteLine($"Part 1: {load}");

load = analyzer.CalculateLoadOnNorthSupportBeams(1000000000);

Console.WriteLine($"Part 2: {load}");
