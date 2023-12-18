using Day02;
using Day02.Data;

try
{
    CubeSet cubesInBag = new(12, 13, 14);
    int sumOfPossible = CubesGame.GetSumOfPossibleGames("input.txt", cubesInBag);
    Console.WriteLine($"Part 1: {sumOfPossible}");

    int sumOfPowersOfMin = CubesGame.GetSumOfPowersOfMinimumSets("input.txt");
    Console.WriteLine($"Part 2: {sumOfPowersOfMin}");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}