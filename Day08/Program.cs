using Day08;

MapReader reader = new("input.txt");

long steps = reader.CountSteps("ZZZ");

Console.WriteLine($"Part 1: {steps}");