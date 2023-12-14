using Day02;
using Day02.Data;

try
{
    CubeSet cubesInBag = new(12, 13, 14);
    int sum = CubesGame.GetSumOfPossibleGames("input.txt", cubesInBag);
    Console.WriteLine(sum);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}