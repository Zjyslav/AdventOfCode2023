
using System.Text;
using System.Text.RegularExpressions;

namespace Day12;
public class ConditionRecordsAnalyzer
{
    ConditionRecord[] records;
    Base3Converter converter = new();
    public ConditionRecordsAnalyzer(string filePath, int copies = 1)
    {
        if (File.Exists(filePath) == false)
            throw new FileNotFoundException($"File {filePath} not found.");

        records = File.ReadAllLines(filePath)
            .Select(l => ParseConditionRecord(l, copies))
            .ToArray();
    }

    public int GetSumOfPossibleArrangements()
    {
        List<int> counts = new();

        foreach (var record in records)
        {
            int count = CountPossibleArrangements(record);
            counts.Add(count);
        }

        return counts.Sum();
    }

    private int CountPossibleArrangements(ConditionRecord record)
    {
        uint[] input = converter.ConvertToBase3Array(record.Row);
        IEnumerable<uint[]> parts = [input];
        return IterateOnRemainingParts(parts, record.DamagedGroups);
    }

    private int IterateOnRemainingParts(IEnumerable<uint[]> remainingParts, int[] damagedParts)
    {
        int output = 0;
        if (damagedParts.Length == 1)
        {
            foreach (var part in remainingParts)
            {
                var current = GetAllPotentialRemainingParts(part, damagedParts[0], true).ToList();
                output += current
                    .Where(x => converter.ConvertArrayToString(x).IndexOf('#') == -1)
                    .Count();
            }
        }
        else
        {
            foreach (var part in remainingParts)
            {
                var remaining = GetAllPotentialRemainingParts(part, damagedParts[0], false);
                output += IterateOnRemainingParts(remaining, damagedParts[1..]);
            }
        }
        return output;
    }

    private uint[] GetPotentialRemainingPart(uint[] input, int damagedGroup, bool last)
    {
        string inputString = converter.ConvertArrayToString(input);
        string pattern = $@"(?<=\A[.\?]*[?#]{{{damagedGroup}}}{ (last ? "" : @"[.\?]")})[.#\?]*";
        var matchValue = Regex
            .Matches(inputString, pattern)
            .FirstOrDefault()?
            .Value;
        if (matchValue is null)
            return [];
        else
            return converter.ConvertToBase3Array(matchValue);
    }

    private IEnumerable<uint[]> GetAllPotentialRemainingParts(uint[] input, int damagedGroup, bool last)
    {
        string inputString = converter.ConvertArrayToString(input);
        uint[] lastReturned = [];
        for (int i = 0; i < inputString.Length; i++)
        {
            string inputSkipped = inputString.Substring(0, i);
            if (inputSkipped.Contains('#'))
                yield break;
            uint[] inputFragment = converter.ConvertToBase3Array(inputString.Substring(i));
            uint[] part = GetPotentialRemainingPart(inputFragment, damagedGroup, last);
            if (part.Length == 0 || (part.Length == 1 && part[0] == 0))
                yield break;
            else if (lastReturned.SequenceEqual(part) == false)
            {
                lastReturned = part;
                yield return part;
            }
        }
    }

    private ConditionRecord ParseConditionRecord(string input, int copies)
    {
        var inputParts = input.Split(' ');

        StringBuilder rowBuilder = new();
        for (int i = 0; i < copies; i++)
        {
            rowBuilder.Append(inputParts[0]);
            if (i < copies - 1)
                rowBuilder.Append("?");
        }
        string row = rowBuilder.ToString();

        StringBuilder groupsBuilder = new StringBuilder();
        for (int i = 0; i < copies; i++)
        {
            groupsBuilder.Append(inputParts[1]);
            if (i < copies - 1)
                groupsBuilder.Append(",");
        }

        int[] damagedGroups = groupsBuilder
            .ToString()
            .Split(',')
            .Select(s => int.Parse(s))
            .ToArray();

        return new ConditionRecord(row, damagedGroups);
    }

    public record ConditionRecord(string Row, int[] DamagedGroups);
}
