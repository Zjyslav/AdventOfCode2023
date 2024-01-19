
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
            counts.Add(CountPossibleArrangements(record));
        }

        return counts.Sum();
    }

    private int CountPossibleArrangements(ConditionRecord record)
    {
        List<uint[]> previous = GetAllPotentialRemainingParts(converter.ConvertToBase3Array(record.Row), record.DamagedGroups[0], false);
        List<uint[]> current;

        for (int i = 1; i < record.DamagedGroups.Length; i++)
        {
            current = new();
            bool last = i == record.DamagedGroups.Length - 1;
            foreach (var part in previous)
            {
                current.AddRange(GetAllPotentialRemainingParts(part, record.DamagedGroups[i], last));
            }
            previous = current;
        }

        return previous.Where(x => converter.ConvertArrayToString(x).IndexOf('#') == -1).Count();
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

    private List<uint[]> GetAllPotentialRemainingParts(uint[] input, int damagedGroup, bool last)
    {
        string inputString = converter.ConvertArrayToString(input);
        List<uint[]> parts = new();
        uint[] lastAdded = [];
        for (int i = 0; i < inputString.Length; i++)
        {
            string inputSkipped = inputString.Substring(0, i);
            if (inputSkipped.Contains('#'))
                return parts;
            uint[] inputFragment = converter.ConvertToBase3Array(inputString.Substring(i));
            uint[] part = GetPotentialRemainingPart(inputFragment, damagedGroup, last);
            if (part.Length == 0 || (part.Length == 1 && part[0] == 0))
                return parts;
            else if (lastAdded.SequenceEqual(part) == false)
            {
                parts.Add(part);
                lastAdded = part;
            }
        }
        return parts;
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
