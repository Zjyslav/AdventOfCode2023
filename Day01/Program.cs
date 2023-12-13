using Day01;

try
{
    int sum = CalibrationValues.GetCalibrationValueSum("input.txt");
    Console.WriteLine(sum);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}