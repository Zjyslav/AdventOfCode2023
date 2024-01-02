using Day08;

MapReader reader = new("input.txt");

long steps = reader.CountSteps("AAA", "ZZZ");

Console.WriteLine($"Part 1: {steps}");