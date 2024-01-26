using Day12;

int taskCount = 1;
int copies = 1;

if (args.Length >= 1)
{
    copies = int.Parse(args[0]);
}
if (args.Length == 2)
{
    taskCount = int.Parse(args[1]);
}

ConditionRecordsAnalyzer analyzer = new("input.txt", "output", copies);

await analyzer.RunCountingTasks(taskCount);
