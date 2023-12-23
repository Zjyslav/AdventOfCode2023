using Day05;

Almanach almanach = new("input.txt");
long location = almanach.GetLowestLocationForSeeds();

Console.WriteLine($"Part 1: {location}");

long locationForRanges = almanach.GetLowestLocationForSeedRanges();

Console.WriteLine($"Part 2: {locationForRanges}");
