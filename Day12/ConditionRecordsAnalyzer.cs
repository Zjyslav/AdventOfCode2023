
using System.Text;
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
        List<int> countsOriginal = new();
        List<int> countsBruteForce = new();

        foreach (var record in records)
        {
            countsOriginal.Add(CountPossibleArrangements(record));
            countsBruteForce.Add(CountPossibleArrangementsByTryingAll(record));
        }

        for (int i = 0; i < records.Length; i++)
        {
            if (countsOriginal[i] != countsBruteForce[i])
            {
                Console.WriteLine($"i: {i}\twrong: {countsOriginal[i]}\t correct: {countsBruteForce[i]}");
            }
        }

        return countsBruteForce.Sum();
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
        string pattern = $@"(?<=[.\?]*[?#]{{{damagedGroup}}}{ (last ? "" : @"[.\?]")})[.#\?]*";
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

    private int CountPossibleArrangementsByTryingAll(ConditionRecord record)
    {
        int count = 0;
        List<string> potentialArrangaments = GetAllPotentialArrangaments(record.Row);

        foreach (var row in potentialArrangaments)
        {
            bool possible = EvaluateArrangament(row, record.DamagedGroups);
            if (possible)
                count++;
        }

        return count;
    }

    private List<string> GetAllPotentialArrangaments(string row)
    {
        List<string> previous = [row];
        List<string> current;

        for (int i = 0; i < row.Length; i++)
        {
            current = new();
            if (row[i] != '?')
                continue;

            foreach (var s in previous)
            {
                current.Add(s.Remove(i, 1).Insert(i, "."));
                current.Add(s.Remove(i, 1).Insert(i, "#"));
            }

            previous = current;
        }

        return previous;
    }

    private bool EvaluateArrangament(string row, int[] damagedGroups)
    {
        StringBuilder builder = new();
        builder.Append(@"\A[.]*");
        for (int i = 0; i < damagedGroups.Length; i++)
        {
            builder.Append($"[#]{{{damagedGroups[i]}}}[.]");
            if (i < damagedGroups.Length - 1)
                builder.Append("+");
        }
        builder.Append(@"*\Z");

        string pattern = builder.ToString();

        return Regex.IsMatch(row, pattern);
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
