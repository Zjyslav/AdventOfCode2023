using System.Diagnostics;
using static Day12.ConditionRecordsAnalyzer;

namespace Day12;
public class CountingProgressReport
{
    public Stopwatch Stopwatch { get; set; }
    public ConditionRecord? Record { get; set; }
    public int Count { get; set; } = 0;
    public int TaskNumber { get; set; }

    public CountingProgressReport(ConditionRecord? record, int taskNumber)
    {
        Record = record;
        Stopwatch = Stopwatch.StartNew();
        TaskNumber = taskNumber;
    }
}
