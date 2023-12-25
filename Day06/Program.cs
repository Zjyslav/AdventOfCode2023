using Day06;

BoatRaces races = new("input.txt");

long multipliedMargins = races.GetMultipliedMargins();

Console.WriteLine($"Part 1: {multipliedMargins}");

races = new BoatRaces("input.txt", true);

long margin = races.GetMultipliedMargins();

Console.WriteLine($"Part 2: {margin}");

