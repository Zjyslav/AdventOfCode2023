using Day01;

try
{
    int sum = CalibrationValues.GetCalibrationValueSum("input.txt");
    Console.WriteLine($"Part 1: {sum}");

    sum = CalibrationValues.GetCalibrationValueSum("input.txt", true);
    Console.WriteLine($"Part 2: {sum}");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}