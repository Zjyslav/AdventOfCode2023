using Day03;

EngineSchematicAnalyzer analyzer = new("input.txt");

int sumOfPartNumbers = analyzer.GetSumOfPartNumbers();

Console.WriteLine($"Part 1: {sumOfPartNumbers}");