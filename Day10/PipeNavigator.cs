namespace Day10;
public class PipeNavigator
{
    List<Tile> tiles = new();
    char[][] inputChars;
    char startSymbol;
    public PipeNavigator(string filePath)
    {
        if (File.Exists(filePath) == false)
            throw new FileNotFoundException($"File {filePath} not found.");

        inputChars = File.ReadAllLines(filePath)
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

        startSymbol = DetermineSymbolUsingConnections(start);
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

    public int CountTilesWithinLoop()
    {
        int tileCount = 0;
        List<Tile> loop = FindLoop();

        for (int y = 0; y < inputChars.Length; y++)
        {
            bool insideLoop = false;
            char lastBend = ' ';
            for (int x = 0; x < inputChars[y].Length; x++)
            {
                if (loop.Any(t => t.X == x && t.Y == y))
                {
                    char symbol = inputChars[y][x];
                    if (symbol == 'S')
                        symbol = startSymbol;

                    switch (symbol)
                    {
                        case '|':
                            insideLoop = !insideLoop;
                            break;
                        case 'L':
                            lastBend = 'L';
                            break;
                        case 'F':
                            lastBend = 'F';
                            break;
                        case 'J':
                            if (lastBend == 'F')
                                insideLoop = !insideLoop;
                            break;
                        case '7':
                            if (lastBend == 'L')
                                insideLoop = !insideLoop;
                            break;
                        default:
                            break;
                    }
                }
                else if (insideLoop)
                    tileCount++;
            }
        }

        return tileCount;
    }

    private List<Tile> FindLoop()
    {
        List<Tile> loop = new();
        Tile start = tiles
            .Where(t => t.Symbol == 'S')
            .First();
        loop.Add(start);
        Tile previous = start;
        Tile current = FindConnected(previous).First();
        loop.Add(current);

        bool loopClosed = false;
        while (loopClosed == false)
        {
            Tile next = FindConnected(current)
                .Where(t => t != previous)
                .First();

            if (next.Symbol == 'S')
                loopClosed = true;
            else
            {
                loop.Add(next);
                previous = current;
                current = next;
            }
        }
        return loop;
    }
    private List<Tile> FindConnected(Tile tile)
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

    private char DetermineSymbolUsingConnections(Tile tile)
    {
        if (tile.Connections.Count == 0)
        {
            return '.';
        }
        else if (tile.Connections.Count == 2)
        {
            List<(int x, int y)> differences = new();
            foreach (var connection in tile.Connections)
            {
                differences.Add((connection.x - tile.X, connection.y - tile.Y));
            }

            if (differences.Contains((0, -1))
                && differences.Contains((0, 1)))
                return '|';
            if (differences.Contains((-1, 0))
                && differences.Contains((1, 0)))
                return '-';
            if (differences.Contains((0, -1))
                && differences.Contains((1, 0)))
                return 'L';
            if (differences.Contains((0, -1))
                && differences.Contains((-1, 0)))
                return 'J';
            if (differences.Contains((0, 1))
                && differences.Contains((-1, 0)))
                return '7';
            if (differences.Contains((0, 1))
                && differences.Contains((1, 0)))
                return 'F';
        }
        return tile.Symbol;
    }
}
