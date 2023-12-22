using Day04;

ScratchcardsAnalyzer analyzer = new("input.txt");

int sumOfPoints = analyzer.GetSumOfScratchcardsPoints();

Console.WriteLine($"Part 1: {sumOfPoints}");

int numberOfScratchcards = analyzer.GetNumberOfScratchcards();

Console.WriteLine($"Part 2: {numberOfScratchcards}");
