
namespace Day05;
public class Almanach
{
    List<Map> maps;
    List<long> seeds;
    public Almanach(string filePath)
    {
        if (File.Exists(filePath) == false)
            throw new FileNotFoundException($"File {filePath} not found.");

        var inputLines = File.ReadAllLines(filePath);

        seeds = ParseSeeds(inputLines);        
        maps = ParseMaps(inputLines);
    }

    public long GetLowestLocationForSeeds()
    {
        List<long> locations = new List<long>();
        foreach (var seed in seeds)
        {
            locations.Add(
                GetDestinationNumber(
                    "seed",
                    "location",
                    seed));
        }

        return locations.Min();
    }

    public long GetLowestLocationForSeedRanges()
    {
        bool first = true;
        long lowestLocation = 0;

        for (int i = 0; i < seeds.Count; i+= 2)
        {
            for (int j = 0; j < seeds[i + 1]; j++)
            {
                long location = GetDestinationNumber(
                    "seed",
                    "location",
                    seeds[i] + j);

                if (first || location < lowestLocation)
                {
                    lowestLocation = location;
                    first = false;
                }
            }
        }
        return lowestLocation;
    }

    private List<long> ParseSeeds(string[] inputLines)
    {
        List<long> output = new();

        string seedsLine = inputLines.Where(l => l.StartsWith("seeds: ")).First();
        seedsLine = seedsLine.Replace("seeds: ", "");

        var seeds = seedsLine.Split(' ');

        foreach (var seed in seeds)
        {
            output.Add(long.Parse(seed.Trim()));
        }

        return output;
    }

    private List<Map> ParseMaps(string[] inputLines)
    {
        List<Map> output = new();
        for (int i = 0; i < inputLines.Length; i++)
        {
            if (inputLines[i].EndsWith(" map:"))
            {
                string mapSignature = inputLines[i].Replace(" map:", "");
                var elements = mapSignature.Split('-');

                string sourceCategory = elements[0];
                string destinationCategory = elements[2];

                Map map = new Map(sourceCategory, destinationCategory);

                i++;
                do
                {
                    var numbers = inputLines[i].Split(' ');
                    if (numbers.Length == 3)
                    {
                        map.Lines.Add(new MapLine(
                            long.Parse(numbers[1].Trim()),
                            long.Parse(numbers[0].Trim()),
                            long.Parse(numbers[2].Trim())));
                    }
                    else
                        break;
                    i++;
                } while (i< inputLines.Length);

                output.Add(map);
            }
        }
        return output;
    }

    private long GetDestinationNumber(string sourceCategory,
                                      string destinationCategory,
                                      long sourceNumber)
    {
        string currentCategory = sourceCategory;
        long currentNumber = sourceNumber;

        while (currentCategory != destinationCategory)
        {
            Map? currentMap = maps
                .Where(m => m.SourceCategory == currentCategory)
                .FirstOrDefault();

            if (currentMap is null)
                return 0;

            currentNumber = currentMap.GetDestinationNumber(currentNumber);
            currentCategory = currentMap.DestinationCategory;
        }

        return currentNumber;
    }
}