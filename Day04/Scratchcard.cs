namespace Day04;

public class Scratchcard
{
    public int CardNumber { get; set; }
    public List<int> WinningNumbers { get; set; } = new();
    public List<int> NumbersIHave { get; set; } = new();
}