namespace Day14;
public class PlatformLoadAnalyzer
{
    char[][] _platform;
    public PlatformLoadAnalyzer(string filePath)
    {
        _platform = File.ReadAllLines(filePath).Select(s => s.ToCharArray()).ToArray();
        
    }

    public int CalculateLoadOnNorthSupportBeams()
    {
        char[][] platformTiltedNorth = TiltNorth(_platform);
        int load = 0;
        for (int r = 0; r < platformTiltedNorth.Length; r++)
        {
            for (int c = 0; c < _platform[r].Length; c++)
            {
                if (_platform[r][c] == 'O')
                {
                    load += _platform.Length - r;
                }
            }
        }
        return load;
    }

    private char[][] TiltNorth(char[][] platform)
    {
        for (int r = 0; r < platform.Length; r++)
        {
            for (int c = 0; c < platform[r].Length; c++)
            {
                if (platform[r][c] == 'O')
                    RollNorth(r, c, ref platform);
            }
        }
        return platform;
    }

    private void RollNorth(int r, int c, ref char[][] platform)
    {
        if (r == 0)
            return;

        for (int i = r - 1; i >= 0; i--)
        {
            if (i == 0 && platform[i][c] == '.')
            {
                platform[r][c] = '.';
                platform[i][c] = 'O';
                return;
            }
            if (platform[i][c] == 'O' || platform[i][c] == '#')
            {
                platform[r][c] = '.';
                platform[i + 1][c] = 'O';
                return;
            }
        }
    }
}
