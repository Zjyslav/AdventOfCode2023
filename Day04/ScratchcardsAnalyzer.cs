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

    public int GetNumberOfScratchcards()
    {
        var scratchcards = GetAllScratchcards();
        int maxCardNumber = scratchcards.Select(s => s.CardNumber).Max();

        var scratchcardCounts = scratchcards
            .Select(s => new ScratchcardCount(
                s.CardNumber,
                GetNumberOfMatches(s)))
            .ToList();


        foreach (var count in scratchcardCounts)
        {
            if (count.Remaining == 0)
            {
                continue;
            }

            var cardsWon = count.GetWonCardNumbers(maxCardNumber);
            do
            {
                foreach (var cardNumber in cardsWon)
                {
                    ScratchcardCount cardWon = scratchcardCounts
                        .Where(c => c.CardNumber == cardNumber)
                        .First();
                
                    cardWon.Add();
                }
                count.Use();                
            } while (count.Remaining > 0);
        }

        return scratchcardCounts.Select(s => s.Total).Sum();
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

    private int GetNumberOfMatches(Scratchcard scratchcard)
    {
        int output = 0;
        foreach (int number in scratchcard.WinningNumbers)
        {
            if (scratchcard.NumbersIHave.Contains(number))
            {
                output++;
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

public class ScratchcardCount
{
    private int _total = 1;
    private int _used = 0;

    public int Total { get { return _total; } }
    public int Remaining { get { return _total - _used; } }
    public int CardNumber { get; }
    public int Matches { get; }
    public ScratchcardCount(int cardNumber, int matches)
    {
        CardNumber = cardNumber;
        Matches = matches;
    }

    public void Add()
    {
        _total++;
    }

    public bool Use()
    {
        if (Remaining == 0)
        {
            return false;
        }
        else
        {
            _used++;
            return true;
        }
    }

    public List<int> GetWonCardNumbers(int maxCardNumber)
    {
        List<int> output = new();
        for (int i = CardNumber + 1; i <= maxCardNumber; i++)
        {
            if (output.Count == Matches)
                break;
            output.Add(i);
        }
        return output;
    }
}