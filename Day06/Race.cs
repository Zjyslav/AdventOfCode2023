namespace Day06;

public class Race
{

    public int Time { get; set; }
    public int BestDistance { get; set; }
    public Race(int time, int bestDistance)
    {
        Time = time;
        BestDistance = bestDistance;
    }
    public int CalculateDistance(int buttonHoldTime)
    {
        return buttonHoldTime * (Time - buttonHoldTime);
    }
}