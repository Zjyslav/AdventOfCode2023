using Day14;

PlatformLoadAnalyzer analyzer = new("input.txt");

int load = analyzer.CalculateLoadOnNorthSupportBeams();

Console.WriteLine($"Part 1: {load}");
