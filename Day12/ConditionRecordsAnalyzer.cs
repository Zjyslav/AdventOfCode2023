
using System.Text.RegularExpressions;

namespace Day12;
public class ConditionRecordsAnalyzer
{
    ConditionRecord[] records;
    public ConditionRecordsAnalyzer(string filePath)
    {
        if (File.Exists(filePath) == false)
            throw new FileNotFoundException($"File {filePath} not found.");

        records = File.ReadAllLines(filePath)
            .Select(l => ParseConditionRecord(l))
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
        List<string> previous = GetAllPotentialRemainingParts(record.Row, record.DamagedGroups[0], false);
        List<string> current;

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

        return previous.Where(s => s.IndexOf('#') == -1).Count();
    }

    private string? GetPotentialRemainingPart(string input, int damagedGroup, bool last)
    {
        string pattern = $@"(?<=\A[.\?]*[?#]{{{damagedGroup}}}{ (last ? "" : @"[.\?]")})[.#\?]*";
        return Regex
            .Matches(input, pattern)
            .FirstOrDefault()?
            .Value;
    }

    private List<string> GetAllPotentialRemainingParts(string input, int damagedGroup, bool last)
    {
        List<string> parts = new();
        for (int i = 0; i < input.Length; i++)
        {
            string inputSkipped = input.Substring(0, i);
            if (inputSkipped.Contains('#'))
                return parts;
            string inputFragment = input.Substring(i);
            string? part = GetPotentialRemainingPart(inputFragment, damagedGroup, last);
            if (part is null)
                return parts;
            else if (parts.LastOrDefault() != part)
                parts.Add(part);
        }
        return parts;
    }

    private ConditionRecord ParseConditionRecord(string input)
    {
        var inputParts = input.Split(' ');
        string row = inputParts[0];
        int[] damagedGroups = inputParts[1]
            .Split(',')
            .Select(s => int.Parse(s))
            .ToArray();

        return new ConditionRecord(row, damagedGroups);
    }

    public record ConditionRecord(string Row, int[] DamagedGroups);
}
