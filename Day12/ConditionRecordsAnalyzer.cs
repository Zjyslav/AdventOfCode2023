
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Day12;
public class ConditionRecordsAnalyzer
{
    ConditionRecord[] records;
    public ConditionRecordsAnalyzer(string filePath, int copies = 1)
    {
        if (File.Exists(filePath) == false)
            throw new FileNotFoundException($"File {filePath} not found.");

        records = File.ReadAllLines(filePath)
            .Select(l => ParseConditionRecord(l, copies))
            .ToArray();
    }

    public int GetSumOfPossibleArrangements(int startIndex, int rows)
    {
        List<int> counts = new();

        Stopwatch stopwatch = new();
        for (int i = startIndex; i < records.Length; i++)
        {
            ConditionRecord? record = records[i];
            stopwatch.Restart();
            int count = CountPossibleArrangements(record);
            stopwatch.Stop();
            Console.WriteLine($"{i}.\t{stopwatch.Elapsed}\t{count}");
            counts.Add(count);

            if (counts.Count >= rows)
                break;
        }
        return counts.Sum();
    }
    public int GetSumOfPossibleArrangements(int startIndex)
    {
        return GetSumOfPossibleArrangements(startIndex, records.Length - startIndex);
    }
    public int GetSumOfPossibleArrangements()
    {
        return GetSumOfPossibleArrangements(0);
    }
    private int CountPossibleArrangements(ConditionRecord record)
    {
        return IterateOnRemainingParts([record.Row], record.DamagedGroups);
    }

    private int IterateOnRemainingParts(IEnumerable<string> remainingParts, int[] damagedGroups)
    {
        int output = 0;
        if (damagedGroups.Length == 1)
        {
            foreach (var part in remainingParts)
            {
                var current = GetAllPotentialRemainingParts(part, damagedGroups[0], true).ToList();
                output += current
                    .Where(s => s.IndexOf('#') == -1)
                    .Count();
            }
        }
        else
        {
            foreach (var part in remainingParts)
            {
                var current = GetAllPotentialRemainingParts(part, damagedGroups[0], false);
                output += IterateOnRemainingParts(current, damagedGroups[1..]);
            }
        }
        return output;
    }

    private string? GetPotentialRemainingPart(string input, int damagedGroup, bool last)
    {
        string pattern = $@"(?<=\A[.\?]*[?#]{{{damagedGroup}}}{ (last ? "" : @"[.\?]")})[.#\?]*";
        Match? matches = Regex.Matches(input, pattern).FirstOrDefault();
        return matches?.Value;
    }

    private IEnumerable<string> GetAllPotentialRemainingParts(string input, int damagedGroup, bool last)
    {
        string? lastReturned = null;
        for (int i = 0; i < input.Length; i++)
        {
            string inputSkipped = input.Substring(0, i);
            if (inputSkipped.Contains('#'))
                yield break;
            string inputFragment = input.Substring(i);
            string? part = GetPotentialRemainingPart(inputFragment, damagedGroup, last);
            if (part is null)
                yield break;
            else if (lastReturned != part)
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
