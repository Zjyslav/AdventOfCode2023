using System.Text;

namespace Day12;
public class Base3Converter
{
    private readonly char _char0;
    private readonly char _char1;
    private readonly char _char2;

    public Base3Converter(char char0 = '.',
                          char char1 = '#',
                          char char2 = '?')
    {
        _char0 = char0;
        _char1 = char1;
        _char2 = char2;
    }
    public uint[] ConvertToBase3Array(string input)
    {
        List<uint> output = [];
        string[] inputParts = SplitStringInput(input);
        foreach (string part in inputParts)
        {
            output.Add(ConvertToBase3(part));
        }
        return output.ToArray();
    }

    public uint ConvertToBase3(string input)
    {
        uint output = 0;

        char[] reverse = input.Reverse().ToArray();

        for (uint i = 0; i < reverse.Length; i++)
        {
            uint digit;
            if (reverse[i] == _char0)
                digit = 0;
            else if (reverse[i] == _char1)
                digit = 1;
            else if (reverse[i] == _char2)
                digit = 2;
            else
                throw new FormatException($"Provided string {input} contains illegal character: {reverse[i]}.");

            output += digit * (uint) Math.Pow(3, i);
        }

        return output;
    }

    public string ConvertArrayToString(uint[] input)
    {
        StringBuilder builder = new();
        foreach (uint number in input)
        {
            builder.Append(ConvertToString(number));
        }
        string toReverse = builder.ToString();
        return new string(toReverse.Reverse().ToArray());
    }

    public string ConvertToString(uint input)
    {
        StringBuilder builder = new();
        while (input > 0)
        {
            uint digit = input % 3;
            if (digit == 0)
                builder.Append(_char0);
            else if (digit == 1)
                builder.Append(_char1);
            else if (digit == 2)
                builder.Append(_char2);
            input /= 3;
        }
        string output = builder.ToString();
        return output.Substring(0, output.Length - 1);
    }

    private string[] SplitStringInput(string input, int length = 19)
    {
        List<string> output = [];
        string reversedInput = new string(input.Reverse().ToArray());

        for (int i = 0; i < (reversedInput.Length / length) + 1; i++)
        {
            int startIndex = length * i;
            int adjustedLength = Math.Min(reversedInput.Length - startIndex, length);
            string part = new string(reversedInput
                .Substring(startIndex, adjustedLength)
                .Reverse()
                .ToArray());
            output.Add(_char1 + part);
        }
        return output.ToArray();
    }
}
