using System.Text.RegularExpressions;

namespace Day06;
public class BoatRaces
{
    List<Race> races = new();

    public BoatRaces(string filePath, bool ignoreSpaces = false)
    {
        if (File.Exists(filePath) == false)
            throw new FileNotFoundException($"File {filePath} not found.");

        var inputLines = File.ReadAllLines(filePath);
        if (ignoreSpaces)
        {
            ParseOneRace(inputLines);
        }
        else
        {
            ParseRaces(inputLines);
        }
    }

    public long GetMultipliedMargins()
    {
        long output = 1;

        foreach (var race in races)
        {
            output *= CalculateMarginOfError(race);
        }

        return output;
    }

    private long CalculateMarginOfError(Race race)
    {
        long output = 0;
        for (long i = 1; i <= race.Time; i++)
        {
            if (race.CalculateDistance(i) > race.BestDistance)
                output++;
        }
        return output;
    }

    private void ParseRaces(string[] inputLines)
    {
        string pattern = @"\b\d+\b";
        var timeMatches = Regex.Matches(
            inputLines.Where(l => l.StartsWith("Time:")).First(),
            pattern);
        var distanceMatches = Regex.Matches(
            inputLines.Where(l => l.StartsWith("Distance:")).First(),
            pattern);

        if (timeMatches.Count != distanceMatches.Count)
        {
            throw new FormatException("Same number of times and distances must be provided.");
        }

        for (int i = 0; i < timeMatches.Count; i++)
        {
            races.Add(new Race(
                long.Parse(timeMatches[i].Value),
                long.Parse(distanceMatches[i].Value)));
        }
    }

    private void ParseOneRace(string[] inputLines)
    {
        long time = long.Parse(
            inputLines
            .Where(l => l.StartsWith("Time:"))
            .First()
            .Replace("Time:", "")
            .Replace(" ", ""));

        long distance = long.Parse(
            inputLines
            .Where(l => l.StartsWith("Distance:"))
            .First()
            .Replace("Distance:", "")
            .Replace(" ", ""));

        races.Add(new Race(time, distance));
    }
}
