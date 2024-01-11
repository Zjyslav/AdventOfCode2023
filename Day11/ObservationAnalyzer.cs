namespace Day11;
public class ObservationAnalyzer
{
    List<List<char>> universe;
    List<List<char>> expandedUniverse;
    public ObservationAnalyzer(string filePath)
    {
        if (File.Exists(filePath) == false)
            throw new FileNotFoundException($"File {filePath} not found.");

        universe = File.ReadAllLines(filePath)
            .Select(l => l.ToList())
            .ToList();

        expandedUniverse = new(universe);
        ExpandUniverse();
    }

    public int GetSumOfShortestPaths()
    {
        List<int> ShortestPaths = new();
        List<Galaxy> galaxies = FindGalaxies(expandedUniverse);

        for (int i = 0; i < galaxies.Count - 1; i++)
        {
            for (int j = i + 1; j < galaxies.Count; j++)
            {
                int path = CalculateShortestPath(galaxies[i], galaxies[j]);
                ShortestPaths.Add(path);
            }
        }

        return ShortestPaths.Sum();
    }

    private void ExpandUniverse()
    {
        List<int> emptyRowIndexes = new();
        List<int> emptyColumnIndexes = new();

        for (int i = 0; i < universe.Count; i++)
        {
            bool empty = !universe[i].Any(c => c != '.');
            if (empty)
                emptyRowIndexes.Add(i);
        }

        for (int i = 0; i < universe[0].Count; i++)
        {
            bool empty = !universe.Select(r => r[i]).Any(c => c != '.');
            if (empty)
                emptyColumnIndexes.Add(i);
        }

        for (int i = 0; i < emptyRowIndexes.Count; i++)
        {
            expandedUniverse.Insert(emptyRowIndexes[i] + i, universe[emptyRowIndexes[i]]);
        }

        for (int i = 0; i < emptyColumnIndexes.Count; i++)
        {
            foreach (var row in expandedUniverse)
            {
                row.Insert(emptyColumnIndexes[i] + i, '.');
            }
        }
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

    private int CalculateShortestPath(Galaxy a, Galaxy b)
    {
        int output = 0;
        if (a.X > b.X)
            output += a.X - b.X;
        else
            output += b.X - a.X;

        if (a.Y > b.Y)
            output += a.Y - b.Y;
        else
            output += b.Y - a.Y;

        return output;
    }

    public record Galaxy(int X, int Y, int Number);
}
