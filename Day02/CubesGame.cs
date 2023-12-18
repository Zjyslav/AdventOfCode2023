using Day02.Data;

namespace Day02;
public static class CubesGame
{
    public static int GetSumOfPossibleGames(string filePath, CubeSet cubesInBag)
    {
        int sum = 0;
        List<Game> possibleGames = new();

        if (File.Exists(filePath) == false)
            throw new FileNotFoundException($"File {filePath} not found.");

        string[] inputLines = File.ReadAllLines(filePath);

        foreach (string line in inputLines)
        {
            Game game = ParseGame(line);
            bool isPossible = EvaluateIfPossible(game, cubesInBag);

            if (isPossible)
                possibleGames.Add(game);
        }

        foreach (Game game in possibleGames)
        {
            sum += game.Id;
        }

        return sum;
    }

    public static int GetSumOfPowersOfMinimumSets(string filePath)
    {
        int sum = 0;

        if (File.Exists(filePath) == false)
            throw new FileNotFoundException($"File {filePath} not found.");

        string[] inputLines = File.ReadAllLines(filePath);

        foreach (string line in inputLines)
        {
            Game game = ParseGame(line);
            CubeSet cubeSet = GetMinimumCubeSet(game);
            int power = GetCubeSetPower(cubeSet);
            sum += power;
        }

        return sum;
    }

    private static Game ParseGame(string gameRecord)
    {
        Game output = new();

        int indexOfColon = gameRecord.IndexOf(':');
        if (indexOfColon == -1)
            throw new FormatException("Record of a game must include a colon (:).");

        int trimLength = "Game ".Length;

        int gameId = int.Parse(gameRecord.Substring(trimLength, indexOfColon - trimLength));

        string[] sets = gameRecord.Substring(indexOfColon + 1).Split(";");

        output.Id = gameId;

        foreach (string set in sets)
        {
            output.CubeSets.Add(PaseCubeSet(set));
        }

        return output;
    }

    private static CubeSet PaseCubeSet(string set)
    {
        int red = 0;
        int green = 0;
        int blue = 0;

        string[] cubes = set.Split(',');

        foreach (string cube in cubes)
        {
            red += GetNumberOfCubes(cube, "red");
            green += GetNumberOfCubes(cube, "green");
            blue += GetNumberOfCubes(cube, "blue");
        }

        return new CubeSet(red, green, blue);
    }

    private static int GetNumberOfCubes(string cube, string color)
    {
        if (cube.EndsWith(color))
        {
            string number = cube.Replace(color, string.Empty).Trim();
            return int.Parse(number);
        }
        else
            return 0;
    }

    private static bool EvaluateIfPossible(Game game, CubeSet cubesInBag)
    {
        foreach (CubeSet set in game.CubeSets)
        {
            if (set.Red > cubesInBag.Red)
                return false;
            if (set.Green > cubesInBag.Green)
                return false;
            if (set.Blue > cubesInBag.Blue)
                return false;
        }

        return true;
    }

    private static CubeSet GetMinimumCubeSet(Game game)
    {
        int red = 0;
        int green = 0;
        int blue = 0;

        foreach (var set in game.CubeSets)
        {
            if (set.Red > red)
                red = set.Red;
            if (set.Green > green)
                green = set.Green;
            if (set.Blue > blue)
                blue = set.Blue;
        }

        return new CubeSet(red, green, blue);
    }

    private static int GetCubeSetPower(CubeSet cubeSet)
    {
        return cubeSet.Red * cubeSet.Green * cubeSet.Blue;
    }
}
