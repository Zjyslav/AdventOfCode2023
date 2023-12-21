namespace Day04;
public class ScratchcardsAnalyzer
{
    string[] inputLines;
    public ScratchcardsAnalyzer(string filePath)
    {
        if (File.Exists(filePath) == false)
            throw new FileNotFoundException($"File {filePath} not found.");

        inputLines = File.ReadAllLines(filePath);
    }

    public int GetSumOfScratchcardsPoints()
    {
        var points = GetAllScratchcards()
            .Select(c => CountScratchcardPoints(c));

        return points.Sum();
    }

    private int CountScratchcardPoints(Scratchcard scratchcard)
    {
        int output = 0;

        foreach (int number in scratchcard.WinningNumbers)
        {
            if (scratchcard.NumbersIHave.Contains(number))
            {
                if (output == 0)
                    output += 1;
                else
                    output *= 2;
            }
        }

        return output;
    }

    private Scratchcard ParseScratchcard(string input)
    {
        if (int.TryParse(input.Split(':')[0].Replace("Card", "").Trim(),
            out int cardNumber) == false)
        {
            throw new FormatException("Unable to parse card number");
        }

        var numberStrings = 
            input
            .Split(':')[1]
            .Trim()
            .Split('|');

        var winningNumbers =
            numberStrings[0]
            .Trim()
            .Split(' ')
            .Select(s => s.Trim())
            .Where(s => s.Length > 0)
            .Select(s => int.Parse(s.Trim()))
            .ToList();

        var numbersIHave =
            numberStrings[1]
            .Trim()
            .Split(' ')
            .Select(s => s.Trim())
            .Where(s => s.Length > 0)
            .Select(s => int.Parse(s.Trim()))
            .ToList();

        return new Scratchcard()
        {
            CardNumber = cardNumber,
            WinningNumbers = winningNumbers,
            NumbersIHave = numbersIHave
        };
    }

    private List<Scratchcard> GetAllScratchcards()
    {
        List<Scratchcard> output = new();

        foreach (string line in inputLines)
        {
            output.Add(ParseScratchcard(line));
        }

        return output;
    }
}
