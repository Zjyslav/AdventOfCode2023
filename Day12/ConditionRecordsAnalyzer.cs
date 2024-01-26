using System.Text;
using System.Text.RegularExpressions;

namespace Day12;
public class ConditionRecordsAnalyzer
{
    ConditionRecord[] records;
    Queue<ConditionRecord> recordsQueue = new();
    CountingProgressReport?[] progressReports = [];
    private readonly string _filePath;
    private readonly string _outputFolder;
    private readonly int _copies;

    public ConditionRecordsAnalyzer(string filePath, string outputFolder, int copies = 1)
    {
        _filePath = filePath;
        _outputFolder = outputFolder;
        _copies = copies;

        if (File.Exists(_filePath) == false)
            throw new FileNotFoundException($"File {_filePath} not found.");

        records = File.ReadAllLines(_filePath)
            .Select((l, i) => ParseConditionRecord(l, _copies, i))
            .ToArray();

        SetUpQueue();
    }

    public async Task RunCountingTasks(int taskCount)
    {
        Console.WriteLine($"Records in queue: {recordsQueue.Count}");
        Console.WriteLine();

        progressReports = new CountingProgressReport[taskCount];
        
        List<Task> tasks = [];
        List<Progress<CountingProgressReport>> progressList = [];
        for (int i = 0; i < taskCount; i++)
        {
            var progress = new Progress<CountingProgressReport>();
            progress.ProgressChanged += Progress_ProgressChanged;
            tasks.Add(WorkOnCounting(progress, i + 1));
            progressList.Add(progress);
        }

        var runningTasks = Task.WhenAll(tasks);

        while (runningTasks.IsCompleted == false)
        {
            Console.ReadLine();
            Console.WriteLine("Report:");
            for (int i = 0; i < progressReports.Length; i++)
            {
                CountingProgressReport? report = progressReports[i];
                if (report is not null)
                {
                    Console.WriteLine($"Task {report.TaskNumber}\t{report.Stopwatch.Elapsed}\t{report.Record!.index}\t{report.Count}");
                }
            }
            Console.WriteLine();
        }
    }

    private void Progress_ProgressChanged(object? sender, CountingProgressReport e)
    {
        progressReports[e.TaskNumber - 1] = null;
        progressReports[e.TaskNumber-1] = e;
    }

    private void CountPossibleArrangements(ConditionRecord record,
                                          IProgress<CountingProgressReport> progress,
                                          CountingProgressReport progressReport)
    {
        int result = IterateOnRemainingParts([record.Row], record.DamagedGroups, progress, progressReport);
        SaveResultOutput(result, record);
    }

    private void SaveResultOutput(int result, ConditionRecord record)
    {
        string path = Path.Combine(_outputFolder,
            $"{Path.GetFileNameWithoutExtension(_filePath)}-{_copies}-{record.index.ToString("0000")}.csv");
        string contents = $"{record};{result}";
        File.WriteAllText(path, contents);
    }

    private int IterateOnRemainingParts(IEnumerable<string> remainingParts,
                                        int[] damagedGroups,
                                        IProgress<CountingProgressReport> progress,
                                        CountingProgressReport progressReport)
    {
        progress.Report(progressReport);

        int output = 0;
        if (damagedGroups.Length == 1)
        {
            foreach (var part in remainingParts)
            {
                var current = GetAllPotentialRemainingParts(part, damagedGroups[0], true).ToList();
                output += current
                    .Where(s => s.IndexOf('#') == -1)
                    .Count();
                progressReport.Count += output;
                progress.Report(progressReport);
            }
        }
        else
        {
            foreach (var part in remainingParts)
            {
                var current = GetAllPotentialRemainingParts(part, damagedGroups[0], false);
                output += IterateOnRemainingParts(current, damagedGroups[1..], progress, progressReport);
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

    private ConditionRecord ParseConditionRecord(string input, int copies, int index)
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

        return new ConditionRecord(row, damagedGroups, index);
    }

    private void SetUpQueue()
    {
        string inputName = $"{Path.GetFileNameWithoutExtension(_filePath)}-{_copies}-";

        if (Directory.Exists(_outputFolder) == false)
            Directory.CreateDirectory(_outputFolder);

        var processedIndexes = Directory
            .EnumerateFiles(_outputFolder)
            .Select(s => Path.GetFileNameWithoutExtension(s))
            .Where(s => s.StartsWith(inputName))
            .Select(s => int.Parse(s.Substring(inputName.Length)));

        var notProcessed = records
            .Where(r => processedIndexes.Contains(r.index) == false)
            .ToArray();

        Random.Shared.Shuffle(notProcessed);

        foreach (var record in notProcessed)
        {
            recordsQueue.Enqueue(record);
        }
    }

    private async Task WorkOnCounting(IProgress<CountingProgressReport> progress, int taskNumber)
    {
        while (recordsQueue.Any())
        {
            ConditionRecord record = recordsQueue.Dequeue();
            CountingProgressReport progressReport = new(record, taskNumber);
            progress.Report(progressReport);
            

            Console.WriteLine($"Task {taskNumber} starts work on record index {record.index}. Remaining in queue: {recordsQueue.Count}");

            await Task.Run(() =>
            {
                CountPossibleArrangements(record, progress, progressReport);
            });

            progressReport.Stopwatch.Stop();
            Console.WriteLine($"Task {taskNumber} ends work on record index {record.index}. Elapsed: {progressReport.Stopwatch.Elapsed} Remaining in queue: {recordsQueue.Count}");
            progress.Report(progressReport);
        }
    }

    public record ConditionRecord(string Row, int[] DamagedGroups, int index)
    {
        public override string ToString()
        {
            return $"{index};{Row};{string.Join(',',DamagedGroups.Select(x => x.ToString()))}";
        }
    };
}
