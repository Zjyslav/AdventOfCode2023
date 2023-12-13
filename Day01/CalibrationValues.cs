namespace Day01;
public static class CalibrationValues
{
    public static int GetCalibrationValueSum(string filePath)
    {
        int sum = 0;

        if (File.Exists(filePath) == false)
            throw new FileNotFoundException(filePath);

        string[] inputLines = File.ReadAllLines(filePath);

        foreach (string line in inputLines)
        {
            sum += GetCalibrationValue(line);
        }

        return sum;
    }
    public static int GetCalibrationValue(string input)
    {
        int output = 0;

        char[] chars = input.ToCharArray();

        for (int i = 0; i < chars.Length; i++)
        {
            if (char.IsDigit(chars[i]))
            {
                output += 10 * int.Parse(chars[i].ToString());
                break;
            }
        }

        for (int i = chars.Length - 1; i >= 0; i--)
        {
            if (char.IsDigit(chars[i]))
            {
                output += int.Parse(chars[i].ToString());
                break;
            }
        }

        return output;
    }


}
