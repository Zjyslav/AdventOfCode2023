
using System.Text;

namespace Day13;
public class PatternAnalyzer
{
    List<Pattern> patterns;
    List<PatternAsNumbers> patternsAsNumbers = new();
    public PatternAnalyzer(string filePath)
    {
        if (File.Exists(filePath) == false)
            throw new FileNotFoundException($"File {filePath} not found.");

        string input = File.ReadAllText(filePath);

        patterns = input
            .Split("\r\n\r\n")
            .Where(s => s.Length > 0)
            .Select(s => s.Split("\r\n"))
            .Select(s => s
                .Where(x => x.Length > 0)
                .ToArray())
            .Select(s => new Pattern(s))
            .ToList();

        ConvertAllPatternsToNumbers();
    }

    public int SummarizePatternNotes()
    {
        List<int> reflectionLines = [];
        foreach (var pattern in patternsAsNumbers)
        {
            reflectionLines.Add(FindReflectionLines(pattern));
        }
        return reflectionLines.Sum();
    }

    public int SummarizePatternNotesWithSmudges()
    {
        List<int> reflectionLines = [];
        foreach (var pattern in patternsAsNumbers)
        {
            reflectionLines.Add(FindNewReflectionLines(pattern));
        }
        return reflectionLines.Sum();
    }

    private int FindReflectionLines(PatternAsNumbers pattern)
    {
        int output = 0;

        int last = pattern.Rows[0];
        for (int i = 1; i < pattern.Rows.Length; i++)
        {
            if (pattern.Rows[i] == last)
            {
                bool isReflectionLine = CheckIfReflectionLine(pattern.Rows, i);
                if (isReflectionLine)
                    output += i * 100;
            }
            last = pattern.Rows[i];
        }

        last = pattern.Columns[0];
        for (int i = 1; i < pattern.Columns.Length; i++)
        {
            if (pattern.Columns[i] == last)
            {
                bool isReflectionLine = CheckIfReflectionLine(pattern.Columns, i);
                if (isReflectionLine)
                    output += i;
            }
            last = pattern.Columns[i];
        }

        return output;
    }

    private bool CheckIfReflectionLine(int[] numbers, int index)
    {
        int i = index;
        int j = index - 1;

        while (true)
        {
            if (i > numbers.Length - 1 || j < 0)
                break;

            if (numbers[i] != numbers[j])
                return false;

            i++;
            j--;
        }
        return true;
    }

    private int FindNewReflectionLines(PatternAsNumbers pattern)
    {
        int last = pattern.Rows[0];
        for (int i = 1; i < pattern.Rows.Length; i++)
        {
            if (pattern.Rows[i] == last || DifferentByOneBit(pattern.Rows[i], last))
            {
                bool isNewReflectionLine = CheckIfNewReflectionLine(pattern.Rows, i);
                if (isNewReflectionLine)
                    return i * 100;
            }
            last = pattern.Rows[i];
        }

        last = pattern.Columns[0];
        for (int i = 1; i < pattern.Columns.Length; i++)
        {
            if (pattern.Columns[i] == last || DifferentByOneBit(pattern.Columns[i], last))
            {
                bool isNewReflectionLine = CheckIfNewReflectionLine(pattern.Columns, i);
                if (isNewReflectionLine)
                    return i;
            }
            last = pattern.Columns[i];
        }

        return 0;
    }
    private bool CheckIfNewReflectionLine(int[] numbers, int index)
    {
        int i = index;
        int j = index - 1;
        bool foundSmudge = false;

        while (true)
        {
            if (i > numbers.Length - 1 || j < 0)
                break;

            bool differentByOneBit = DifferentByOneBit(numbers[i], numbers[j]);

            if ((numbers[i] != numbers[j] && differentByOneBit == false)
                || (differentByOneBit && foundSmudge))
            {
                return false;
            }

            if (differentByOneBit)
                foundSmudge = true;

            i++;
            j--;
        }
        return foundSmudge;
    }

    private bool DifferentByOneBit(int x, int y)
    {
        int difference = x ^ y;
        return (difference != 0 && (difference & (difference - 1)) == 0);
    }

    private void ConvertAllPatternsToNumbers()
    {
        foreach (var pattern in patterns)
        {
            patternsAsNumbers.Add(ConvertPatternToNumbers(pattern));
        }
    }

    private PatternAsNumbers ConvertPatternToNumbers(Pattern pattern)
    {
        List<int> rows = [];
        foreach (var line in pattern.Lines)
        {
            int asNumber = Convert
                .ToInt32(line
                    .Replace(".", "0")
                    .Replace("#", "1"),
                    2);
            rows.Add(asNumber);
        }

        List<int> columns = new List<int>();
        for (int i = 0; i < pattern.Lines[0].Length; i++)
        {
            StringBuilder builder = new();
            foreach (var line in pattern.Lines)
            {
                builder.Append(line[i]);
            }
            int asNumber = Convert
                .ToInt32(builder
                    .ToString()
                    .Replace(".", "0")
                    .Replace("#", "1"),
                    2);
            columns.Add(asNumber);
        }

        return new PatternAsNumbers(rows.ToArray(), columns.ToArray());
    }

    public record Pattern(string[] Lines);
    public record PatternAsNumbers(int[] Rows, int[] Columns);
}
