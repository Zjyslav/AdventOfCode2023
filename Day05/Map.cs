
namespace Day05;

public class Map
{

    public string SourceCategory { get; set; }
    public string DestinationCategory { get; set; }
    public List<MapLine> Lines { get; set; } = new();
    public Map(string sourceCategory, string destinationCategory)
    {
        SourceCategory = sourceCategory;
        DestinationCategory = destinationCategory;
    }

    public long GetDestinationNumber(long sourceNumber)
    {
        foreach (var line in Lines)
        {
            if (sourceNumber >= line.SourceRangeStart && sourceNumber < line.SourceRangeEnd)
            {
                long difference = sourceNumber - line.SourceRangeStart;
                return line.DestinationRangeStart + difference;
            }
        }
        return sourceNumber;
    }
}