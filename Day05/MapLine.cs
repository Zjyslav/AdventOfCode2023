
namespace Day05;

public class MapLine
{
    public long SourceRangeStart { get; set; }
    public long DestinationRangeStart { get; set; }
    public long RangeLength { get; set; }

    public long SourceRangeEnd
    {
        get
        {
            return SourceRangeStart + RangeLength;
        }
    }
    public long DestinationRangeEnd
    {
        get
        {
            return DestinationRangeStart + RangeLength;
        }
    }
    public MapLine(long sourceRangeStart, long destinationRangeStart, long rangeLength)
    {
        SourceRangeStart = sourceRangeStart;
        DestinationRangeStart = destinationRangeStart;
        RangeLength = rangeLength;
    }
}