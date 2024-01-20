using Day12;

int? startIndex = null, rows = null;

if (args.Length >= 1)
{
    startIndex = int.Parse(args[0]);
}
if (args.Length == 2)
{
    rows = int.Parse(args[1]);
}

ConditionRecordsAnalyzer analyzer = new("input.txt");

int sum;
if (startIndex is null)
{
    sum = analyzer.GetSumOfPossibleArrangements();
}
else if (rows is null)
{
    sum = analyzer.GetSumOfPossibleArrangements((int)startIndex);
}
else
{
    sum = analyzer.GetSumOfPossibleArrangements((int)startIndex, (int)rows);
}

Console.WriteLine($"Part 1: {sum}");

analyzer = new("input.txt", 5);

if (startIndex is null)
{
    sum = analyzer.GetSumOfPossibleArrangements();
}
else if (rows is null)
{
    sum = analyzer.GetSumOfPossibleArrangements((int)startIndex);
}
else
{
    sum = analyzer.GetSumOfPossibleArrangements((int)startIndex, (int)rows);
}

Console.WriteLine($"Part 2: {sum}");