using System.Text.RegularExpressions;

namespace Day08;
public class Node
{
    public string Address { get; set; }
    public string Left { get; set; }
    public string Right { get; set; }
    public int LeftIndex { get; set; }
    public int RightIndex { get; set; }
    public Node(string input)
    {
        string pattern = @"\A\w+";
        Address = Regex.Match(input, pattern).Value;

        pattern = @"(?<=\()\w+";
        Left = Regex.Match(input, pattern).Value;

        pattern = @"\w+(?=\))";
        Right = Regex.Match(input, pattern).Value;
    }
}
