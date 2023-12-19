using System.Diagnostics;
using System.Dynamic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Position = (int r, int c);

namespace Day03;
public class EngineSchematicAnalyzer
{
    string[] inputLines;
    public EngineSchematicAnalyzer(string filePath)
    {
        if (File.Exists(filePath) == false)
            throw new FileNotFoundException($"File {filePath} not found.");

        inputLines = File.ReadAllLines(filePath);
    }

    public int GetSumOfPartNumbers()
    {
        var numbers = GetPartNumbers();
        return numbers.Sum();
    }

    public int GetSumOfGearRatios()
    {
        var ratios = GetGearRatios();
        return ratios.Sum();
    }

    private List<int> GetGearRatios()
    {
        List<int> output = new();
        List<Position> gearPositions = GetGearPositions();
        List<(Position?, Position?)> digitPositions = GetDigitsPositions();

        foreach (var position in gearPositions)
        {
            var adjacentRanges = digitPositions
                .Where(r => IsAdjacentToRange(position, r))
                .ToList();

            if (adjacentRanges.Count != 2)
                continue;

            output.Add(ParseDigits(adjacentRanges[0]) * ParseDigits(adjacentRanges[1]));
        }

        return output;
    }

    private List<int> GetPartNumbers()
    {
        List<int> output = new();

        List<(Position? start, Position? end)> possiblePositions = GetDigitsPositions();

        foreach (var position in possiblePositions)
        {
            if (VerifyPartNumber(position, out int number) == true)
                output.Add(number);
        }

        return output;
    }

    private bool VerifyPartNumber((Position? start, Position? end) position, out int number)
    {
        number = ParseDigits(position);

        bool IsPartNumber = false;

        int row = (int)position.start?.r!;
        int start = (int)position.start?.c!;
        int end = (int)position.end?.c!;
        Position surroundStart = (Math.Max(row - 1, 0), Math.Max(start - 1, 0));
        Position surroundEnd = (Math.Min(row + 1, inputLines.Length -1), end + 1);

        for (int i = surroundStart.r; i <= surroundEnd.r; i++)
        {
            for (int j = surroundStart.c; j <= surroundEnd.c && j < inputLines[i].Length; j++)
            {
                if (char.IsDigit(inputLines[i][j]) == false
                    && inputLines[i][j] != '.')
                {
                    IsPartNumber = true;
                    return IsPartNumber;
                }
            }
        }

        return IsPartNumber;
    }

    private int ParseDigits((Position? start, Position? end) position)
    {
        int number = 0;

        if (position.start is null || position.end is null)
            throw new ArgumentNullException("position");

        int row = (int)position.start?.r!;
        int start = (int)position.start?.c!;
        int end = (int)position.end?.c!;
        int.TryParse(inputLines[row].Substring(start, end - start + 1),
                     out number);

        return number;
    }

    private List<(Position? start, Position? end)> GetDigitsPositions()
    {
        List<(Position?, Position?)> output = new();

        for (int i = 0; i < inputLines.Length; i++)
        {
            bool foundNumber = false;
            Position? start = null;
            Position? last = null;

            for (int j = 0; j < inputLines[i].Length; j++)
            {
                if (char.IsDigit(inputLines[i][j]))
                {
                    last = (i, j);
                    if (foundNumber == false)
                    {
                        foundNumber = true;
                        start = (i, j);
                    }

                    if (j == inputLines[i].Length - 1
                        && foundNumber)
                    {
                        output.Add((start, last));
                        last = null;
                        foundNumber = false;
                    }

                }
                else
                {
                    if (foundNumber)
                    {
                        output.Add((start, last));
                        last = null;
                        foundNumber = false;
                    }
                }
            }
        }

        return output;
    }

    private List<Position> GetGearPositions()
    {
        List<Position> output = new();

        for (int i = 0; i < inputLines.Length; i++)
        {
            for (int j = 0; j < inputLines[i].Length; j++)
            {
                if (inputLines[i][j] == '*')
                {
                    output.Add((i, j));
                }
            }
        }

        return output;
    }

    private bool IsAdjacentToRange(Position position, (Position? start, Position? end) range)
    {
        Position surroundStart = (position.r - 1, position.c - 1);
        Position surroundEnd = (position.r + 1, position.c + 1);

        if (range.start?.r > surroundEnd.r
            || range.start?.c > surroundEnd.c
            || range.end?.r < surroundStart.r
            || range.end?.c < surroundStart.c)
            return false;
        else
            return true;
    }
}
