using Day10;

PipeNavigator navigator = new("input.txt");

int steps = navigator.CountStepsToFarthest();

Console.WriteLine($"Part 1: {steps}");

int tiles = navigator.CountTilesWithinLoop();

Console.WriteLine($"Part 2: {tiles}");