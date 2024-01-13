namespace Day11;
public class ObservationAnalyzer
{
    List<List<char>> universe;
    List<List<char>> expandedUniverse;
    List<int> emptyRows = new();
    List<int> emptyColumns = new();
    public ObservationAnalyzer(string filePath)
    {
        if (File.Exists(filePath) == false)
            throw new FileNotFoundException($"File {filePath} not found.");

        universe = File.ReadAllLines(filePath)
            .Select(l => l.ToList())
            .ToList();

        expandedUniverse = new(universe);
        FindEmptyRowsAndColumns();
    }

    public long GetSumOfShortestPaths(int expansionRate = 2)
    {
        List<long> ShortestPaths = new();
        List<Galaxy> galaxies = FindGalaxies(universe);

        for (int i = 0; i < galaxies.Count - 1; i++)
        {
            for (int j = i + 1; j < galaxies.Count; j++)
            {
                long path = CalculateShortestPath(galaxies[i], galaxies[j], expansionRate);
                ShortestPaths.Add(path);
            }
        }

        return ShortestPaths.Sum();
    }

    private List<Galaxy> FindGalaxies(List<List<char>> universe)
    {
        int galaxyNumber = 1;
        List<Galaxy> output = new();
        for (int i = 0; i < universe.Count; i++)
        {
            for (int j = 0; j < universe[i].Count; j++)
            {
                if (universe[i][j] == '#')
                {
                    output.Add(new Galaxy(i, j, galaxyNumber));
                    galaxyNumber++;
                }
            }
        }

        return output;
    }

    private void FindEmptyRowsAndColumns()
    {
        for (int i = 0; i < universe.Count; i++)
        {
            bool empty = !universe[i].Any(c => c != '.');
            if (empty)
                emptyRows.Add(i);
        }

        for (int i = 0; i < universe[0].Count; i++)
        {
            bool empty = !universe.Select(r => r[i]).Any(c => c != '.');
            if (empty)
                emptyColumns.Add(i);
        }
    }

    private long CalculateShortestPath(Galaxy a, Galaxy b, int expansionRate)
    {
        long output = 0;
        if (a.X > b.X)
            output += a.X - b.X;
        else
            output += b.X - a.X;

        if (a.Y > b.Y)
            output += a.Y - b.Y;
        else
            output += b.Y - a.Y;

        foreach (var row in emptyRows)
        {
            if ((a.X < row && b.X > row) || (b.X < row && a.X > row))
                output += (expansionRate - 1);
        }

        foreach (var column in emptyColumns)
        {
            if ((a.Y < column && b.Y > column) || (b.Y < column && a.Y > column))
                output += (expansionRate - 1);
        }

        return output;
    }

    public record Galaxy(int X, int Y, int Number);
}
