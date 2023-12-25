using System.Text.RegularExpressions;

namespace Day06;
public class BoatRaces
{
    List<Race> races = new();

    public BoatRaces(string filePath)
    {
        if (File.Exists(filePath) == false)
            throw new FileNotFoundException($"File {filePath} not found.");

        var inputLines = File.ReadAllLines(filePath);
        ParseRaces(inputLines);
    }

    public int GetMultipliedMargins()
    {
        int output = 1;

        foreach (var race in races)
        {
            output *= CalculateMarginOfError(race);
        }

        return output;
    }

    private int CalculateMarginOfError(Race race)
    {
        int output = 0;
        for (int i = 1; i <= race.Time; i++)
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
                int.Parse(timeMatches[i].Value),
                int.Parse(distanceMatches[i].Value)));
        }
    }
}
