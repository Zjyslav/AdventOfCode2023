
namespace Day01;
public static class CalibrationValues
{
    public static int GetCalibrationValueSum(string filePath, bool includeSpelledDigits = false)
    {
        int sum = 0;

        if (File.Exists(filePath) == false)
            throw new FileNotFoundException(filePath);

        string[] inputLines = File.ReadAllLines(filePath);

        foreach (string line in inputLines)
        {
            sum += GetCalibrationValue(line, includeSpelledDigits);
        }

        return sum;
    }

    public static int GetCalibrationValue(string input, bool includeSpelledDigits = false)
    {
        (int? index, int? digit) first = (null, null), last = (null, null);

        string[] digits =
        {
            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"
        };

        string[] words =
        {
            "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"
        };

        for (int i = 0; i < digits.Length; i++)
        {
            int firstIndex = input.IndexOf(digits[i]);
            if (firstIndex == -1)
                continue;
            if (firstIndex < first.index || first.index == null)
                first = (firstIndex, i);

            int lastIndex = firstIndex;
            while (lastIndex < input.Length - 1)
            {
                int nextIndex = input.IndexOf(digits[i], lastIndex + 1);
                if (nextIndex == -1)
                    break;
                lastIndex = nextIndex;
            }

            if (lastIndex > last.index || last.index == null)
                last = (lastIndex, i);
        }

        if (includeSpelledDigits)
        {
            for (int i = 0; i < words.Length; i++)
            {
                int firstIndex = input.IndexOf(words[i]);
                if (firstIndex == -1)
                    continue;
                if (firstIndex < first.index || first.index == null)
                    first = (firstIndex, i);

                int lastIndex = firstIndex;
                while (lastIndex < input.Length - 1)
                {
                    int nextIndex = input.IndexOf(words[i], lastIndex + 1);
                    if (nextIndex == -1)
                        break;
                    lastIndex = nextIndex;
                }

                if (lastIndex > last.index || last.index == null)
                    last = (lastIndex, i);
            }
        }

        if (first.digit is null || last.digit is null)
            return 0;
        else
            return (int) first.digit * 10 + (int) last.digit;
    }
}
