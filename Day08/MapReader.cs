namespace Day08;
public class MapReader
{
    List<Node> nodes = new();
    char[] instructions;
    public MapReader(string filePath)
    {
        if (File.Exists(filePath) == false)
            throw new FileNotFoundException($"File {filePath} not found.");

        var inputLines = File.ReadAllLines(filePath);

        instructions = inputLines[0].ToCharArray();

        for (int i = 2; i < inputLines.Length; i++)
        {
            nodes.Add(new Node(inputLines[i]));
        }

        foreach (Node node in nodes)
        {
            SetIndexes(node);
        }
    }

    public long CountSteps(string startAddress, string endAddress)
    {
        long steps = 0;
        int currentIndex = nodes.IndexOf(
            nodes
                .Where(n => n.Address == startAddress)
                .First());

        int endIndex = nodes.IndexOf(
            nodes
                .Where(n => n.Address == endAddress)
                .First());

        for (int i = 0; i < instructions.Length; i++)
        {
            if (instructions[i] == 'L')
            {
                currentIndex = nodes[currentIndex].LeftIndex;
            }
            else if (instructions[i] == 'R')
            {
                currentIndex = nodes[currentIndex].RightIndex;
            }
            steps++;
            if (currentIndex == endIndex)
                break;

            if (i == instructions.Length - 1)
            {
                i = -1;
            }
        }

        return steps;
    }

    public long CountStepsToOnlyZ()
    {
        List<long> stepsToLoop = new();

        var startNodes = nodes.Where(n => n.Address.EndsWith('A'));

        foreach (var node in startNodes)
        {
            stepsToLoop.Add(CountStepsToLoop(node.Address));
        }

        long lcm = stepsToLoop[0];

        for (int i = 1; i < stepsToLoop.Count; i++)
        {
            lcm = LowestCommonMultiple(lcm, stepsToLoop[i]);
        }

        return lcm;
    }
    private long CountStepsToLoop(string startAddress)
    {
        long steps = 0;
        Node currentNode = nodes.Where(n => n.Address == startAddress).First();
        List<EndRecord> ends = new();

        for (int i = 0; i < instructions.Length; i++)
        {
            int index;
            if (instructions[i] == 'L')
            {
                index = currentNode.LeftIndex;
            }
            else
            {
                index = currentNode.RightIndex;
            }
            currentNode = nodes[index];
            steps++;
            if (currentNode.Address.EndsWith('Z'))
            {
                if (ends.Any(e => e.EndIndex == index && e.InstructionIndex == i))
                {
                    return ends
                        .Where(e => e.EndIndex == index && e.InstructionIndex == i)
                        .First()
                        .Steps;
                }
                else
                    ends.Add(new EndRecord(index, i, steps));
            }

            if (i == instructions.Length - 1)
            {
                i = -1;
            }
        }
        return 0;
    }

    private Node LookupNextNode(Node node, char direction)
    {
        if (direction == 'L')
            return nodes.Where(n => n.Address == node.Left).First();
        else if (direction == 'R')
            return nodes.Where(n => n.Address == node.Right).First();
        else
            throw new ArgumentException("Ontly acceptable directions are L and R.");
    }

    private void SetIndexes(Node node)
    {
        Node left = LookupNextNode(node, 'L');
        Node right = LookupNextNode(node, 'R');

        node.LeftIndex = nodes.IndexOf(left);
        node.RightIndex = nodes.IndexOf(right);
    }

    private record EndRecord(int EndIndex, int InstructionIndex, long Steps);

    private long LowestCommonMultiple(long a, long b)
    {
        var divisorsA = GetDivisors(a);
        var divisorsB = GetDivisors(b);

        List<long> resultDivisors = new();

        foreach (long divisor in divisorsA)
        {
            if (divisorsB.Contains(divisor))
                divisorsB.Remove(divisor);
        }
        resultDivisors.AddRange(divisorsA);
        resultDivisors.AddRange(divisorsB);

        long result = 1;
        foreach (var divisor in resultDivisors)
        {
            result *= divisor;
        }
        return result;
    }

    private List<long> GetDivisors(long x)
    {
        List<long> output = new();
        long divisor = 2;

        while (x > 1)
        {
            if (x % divisor == 0)
            {
                output.Add(divisor);
                x /= divisor;
            }
            else
                divisor++;
        }
        return output;
    }
}
