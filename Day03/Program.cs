using Day03;

EngineSchematicAnalyzer analyzer = new("input.txt");

int sumOfPartNumbers = analyzer.GetSumOfPartNumbers();

Console.WriteLine($"Part 1: {sumOfPartNumbers}");

int sumOfGearRatios =  analyzer.GetSumOfGearRatios();

Console.WriteLine($"Part 2: {sumOfGearRatios}");