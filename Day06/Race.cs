namespace Day06;

public class Race
{

    public long Time { get; set; }
    public long BestDistance { get; set; }
    public Race(long time, long bestDistance)
    {
        Time = time;
        BestDistance = bestDistance;
    }

    public long CalculateDistance(long buttonHoldTime)
    {
        return buttonHoldTime * (Time - buttonHoldTime);
    }
}