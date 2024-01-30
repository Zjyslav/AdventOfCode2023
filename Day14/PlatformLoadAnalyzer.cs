using System.ComponentModel.DataAnnotations;

namespace Day14;
public class PlatformLoadAnalyzer
{
    char[][] _platform;
    public PlatformLoadAnalyzer(string filePath)
    {
        _platform = File.ReadAllLines(filePath).Select(s => s.ToCharArray()).ToArray();
        
    }

    public int CalculateLoadOnNorthSupportBeams(long spincCycles = 0)
    {
        var platform = _platform;
        if (spincCycles == 0)
            platform = TiltNorth(platform);
        else
        {
            List<string> platforms = [ConvertPlatformToString(platform)];
            bool foundRepetition = false;
            long countdown = 0;
            for (long i = 1; i <= spincCycles; i++)
            {
                platform = SpinCycle(platform);
                string platformString = ConvertPlatformToString(platform);
                if (foundRepetition == false && platforms.Contains(platformString))
                {
                    foundRepetition = true;
                    long firstRepeated = platforms.IndexOf(platformString);
                    long period = i - firstRepeated;
                    countdown = (spincCycles - i - 1) % period;
                    if (countdown == 0)
                        break;
                }
                else if (foundRepetition == false)
                    platforms.Add(platformString);
                else
                {
                    if (countdown == 0)
                        break;
                    else
                        countdown--;
                }

            }
        }
        
        int load = 0;
        for (int r = 0; r < platform.Length; r++)
        {
            for (int c = 0; c < platform[r].Length; c++)
            {
                if (platform[r][c] == 'O')
                {
                    load += platform.Length - r;
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

    private char[][] SpinCycle(char[][] platform)
    {
        // North
        platform = TiltNorth(platform);
        // West
        platform = RotatePlatformClockwise(platform);
        platform = TiltNorth(platform);
        // South
        platform = RotatePlatformClockwise(platform);
        platform = TiltNorth(platform);
        // East
        platform = RotatePlatformClockwise(platform);
        platform = TiltNorth(platform);

        platform = RotatePlatformClockwise(platform);
        return platform;
    }

    private char[][] RotatePlatformClockwise(char[][] platform)
    {
        char[][] output = new char[platform[0].Length][];
        for (int i = 0; i < output.Length; i++)
        {
            output[i] = new char[platform.Length];
            for (int j = 0; j < output[i].Length; j++)
            {
                output[i][j] = platform[platform[0].Length - j - 1][i];
            }
        }
        return output;
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
    private string ConvertPlatformToString(char[][] platform)
    {
        return string.Join("",
            platform
                .Select(r => new string(r)));
    }
}
