using System.Text.RegularExpressions;

namespace Day07;

public class Hand : IComparable<Hand>
{
    Dictionary<char, int> cards = new();
    public static List<char> CardTypes = ['2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A'];
    public string CardsString { get; }
    public HandType HandType { get; }

    public int Bid { get; }

    public Hand(string cardsString, int bid)
    {
        CardsString = cardsString;
        Bid = bid;

        foreach (char c in CardTypes)
        {
            cards[c] = new Regex(c.ToString()).Matches(cardsString).Count;
        }

        HandType = GetHandType();
    }

    private int GetCardValue(char c)
    {
        return CardTypes.IndexOf(c);
    }

    private HandType GetHandType()
    {
        int max = cards.Values.Max();
        switch (max)
        {
            case 5:
                return HandType.FiveOfAKind;
            case 4:
                return HandType.FourOfAKind;
            case 3:
                if (cards.Values.Any(v => v == 2))
                    return HandType.FullHouse;
                else
                    return HandType.ThreeOfAKind;
            case 2:
                if (cards.Values.Where(v => v == 2).Count() == 2)
                    return HandType.TwoPair;
                else
                    return HandType.OnePair;
            case 1:
                return HandType.HighCard;
            default:
                return HandType.None;
        }
    }

    public int CompareTo(Hand? other)
    {
        if (other is null)
        {
            return 1;
        }
        else if (HandType > other.HandType)
        {
            return 1;
        }
        else if (HandType < other.HandType)
        {
            return -1;
        }
        else
        {
            char[] thisChars = CardsString.ToCharArray();
            char[] otherChars = other.CardsString.ToCharArray();

            for (int i = 0; i < thisChars.Length; i++)
            {
                int thisVal = GetCardValue(thisChars[i]);
                int otherVal = GetCardValue(otherChars[i]);

                if (thisVal > otherVal)
                    return 1;
                if (thisVal < otherVal)
                    return -1;
            }
            return 0;
        }
    }
}
