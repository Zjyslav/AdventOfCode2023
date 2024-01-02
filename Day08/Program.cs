using Day08;

MapReader reader = new("input.txt");

long steps = reader.CountSteps("AAA", "ZZZ");

Console.WriteLine($"Part 1: {steps}");

long stepsToZ = reader.CountStepsToOnlyZ();

Console.WriteLine($"Part 2: {stepsToZ}");