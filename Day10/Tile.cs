namespace Day10;
public class Tile
{
    public char Symbol { get; }
    public int X { get; set; }
    public int Y { get; set; }
    public List<(int x, int y)> Connections { get; set; } = new();
    public Tile(char symbol, int x, int y)
    {
        Symbol = symbol;
        X = x;
        Y = y;

        switch (symbol)
        {
            case '|':
                Connections.Add((x, y - 1));
                Connections.Add((x, y + 1));
                break;
            case '-':
                Connections.Add((x - 1, y));
                Connections.Add((x + 1, y));
                break;
            case 'L':
                Connections.Add((x, y - 1));
                Connections.Add((x + 1, y));
                break;
            case 'J':
                Connections.Add((x, y - 1));
                Connections.Add((x - 1, y));
                break;
            case '7':
                Connections.Add((x, y + 1));
                Connections.Add((x - 1, y));
                break;
            case 'F':
                Connections.Add((x, y + 1));
                Connections.Add((x + 1, y));
                break;
            case '.':
            case 'S':
            default:
                break;

        }
    }
}
