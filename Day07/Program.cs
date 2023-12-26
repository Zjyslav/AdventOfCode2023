using Day07;

CamelCards camelCards = new("input.txt");

int totalWinnings = camelCards.GetTotalWinnings();

Console.WriteLine($"Part 1: {totalWinnings}");