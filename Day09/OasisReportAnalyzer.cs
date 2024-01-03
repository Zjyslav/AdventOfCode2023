namespace Day09;
public class OasisReportAnalyzer
{
    List<List<int>> histories = new();
    public OasisReportAnalyzer(string filePath)
    {
        if (File.Exists(filePath) == false)
            throw new FileNotFoundException($"File {filePath} not found.");

        var inputLines = File.ReadAllLines(filePath);
        foreach (var line in inputLines)
        {
            ParseHistory(line);
        }
    }

    public int GetSumOfPredictions()
    {
        List<int> predictions = new();
        foreach (var history in histories)
        {
            predictions.Add(PredictNextValue(history));
        }

        return predictions.Sum();
    }

    private void ParseHistory(string line)
    {
        var history = line
            .Split(' ')
            .Select(x => int.Parse(x.Trim()))
            .ToList();

        histories.Add(history);
    }

    private int PredictNextValue(List<int> history)
    {
        List<List<int>> sequences = new();
        sequences.Add(history);
        var currentSequence = history;

        for (int i = history.Count; i > 1 ; i--)
        {
            currentSequence = GetSequenceOfDifferences(currentSequence);
            sequences.Add(currentSequence);
            if (currentSequence.All(x => x == 0))
            {
                break;
            }
        }

        sequences.Last().Add(0);

        for (int i = sequences.Count - 2; i >= 0 ; i--)
        {
            int newElement = sequences[i].Last() + sequences[i + 1].Last();
            sequences[i].Add(newElement);
        }

        return sequences[0].Last();
    }

    private List<int> GetSequenceOfDifferences(List<int> inputSequence)
    {
        List<int> outputSequence = new();
        for (int i = 1; i < inputSequence.Count; i++)
        {
            outputSequence.Add(inputSequence[i] - inputSequence[i - 1]);
        }
        return outputSequence;
    }
}
