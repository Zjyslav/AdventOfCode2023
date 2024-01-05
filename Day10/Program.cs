using Day10;

PipeNavigator navigator = new("input.txt");

int steps = navigator.CountStepsToFarthest();

Console.WriteLine($"Part 1: {steps}");