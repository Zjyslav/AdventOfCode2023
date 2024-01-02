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
}
