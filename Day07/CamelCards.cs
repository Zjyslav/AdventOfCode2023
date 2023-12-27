namespace Day07;
public class CamelCards
{
    List<Hand> hands = new();
    public CamelCards(string filePath, bool useJokerRules = false)
    {
        if (File.Exists(filePath) == false)
            throw new FileNotFoundException($"File {filePath} not found.");

        var inputLines = File.ReadAllLines(filePath);

        foreach (var line in inputLines)
        {
            var lineElements = line.Split(' ');
            hands.Add(new Hand(
                lineElements[0],
                int.Parse(lineElements[1]),
                useJokerRules));
        }

        hands.Sort();
    }

    public int GetTotalWinnings()
    {
        int output = 0;
        for (int i = 0; i < hands.Count; i++)
        {
            output += hands[i].Bid * (i + 1);
        }
        return output;
    }
}
