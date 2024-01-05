namespace Day10;
public class PipeNavigator
{
    List<Tile> tiles = new();
    public PipeNavigator(string filePath)
    {
        if (File.Exists(filePath) == false)
            throw new FileNotFoundException($"File {filePath} not found.");

        var inputChars = File.ReadAllLines(filePath)
            .Select(l => l.ToCharArray())
            .ToArray();

        for (int y = 0; y < inputChars.Length; y++)
        {
            for (int x = 0; x < inputChars[y].Length; x++)
            {
                tiles.Add(new Tile(inputChars[y][x], x, y));
            }
        }

        Tile start = tiles
            .Where(t => t.Symbol == 'S')
            .First();
        var connectedToStart = tiles
            .Where(t => t.Connections.Contains((start.X, start.Y)))
            .ToList();

        foreach (Tile tile in connectedToStart)
        {
            start.Connections.Add((tile.X, tile.Y));
        }
    }

    public int CountStepsToFarthest()
    {
        List<(Tile tile, int steps)> loop = new();
        Tile start = tiles
            .Where(t => t.Symbol == 'S')
            .First();

        bool loopClosed = false;

        int i = 0;
        loop.Add((start, i));
        i++;

        Tile[] previousStep = [start, start];

        var currentStep = FindConnected(start);
        loop.Add((currentStep[0], i));
        loop.Add((currentStep[1], i));
        i++;

        while (loopClosed == false)
        {
            Tile[] nextStep = new Tile[2];

            nextStep[0] = FindConnected(currentStep[0]).
                Where(t => t != previousStep[0])
                .First();

            nextStep[1] = FindConnected(currentStep[1]).
                Where(t => t != previousStep[1])
                .First();

            foreach (Tile tile in nextStep)
            {
                if (loop.Any(s => s.tile == tile) == false)
                    loop.Add((tile, i));
                else
                {
                    loopClosed = true;
                }
            }

            previousStep[0] = currentStep[0];
            previousStep[1] = currentStep[1];
            currentStep[0] = nextStep[0];
            currentStep[1] = nextStep[1];

            i++;
        }

        return loop.Select(s => s.steps).Max();
    }

    public List<Tile> FindConnected(Tile tile)
    {
        List<Tile> output = new List<Tile>();

        foreach (var connection in tile.Connections)
        {
            Tile? connectedTile = tiles
                .Where(t => t.X == connection.x && t.Y == connection.y)
                .FirstOrDefault();

            if (connectedTile is not null)
                output.Add(connectedTile);
        }
        return output;
    }
}
