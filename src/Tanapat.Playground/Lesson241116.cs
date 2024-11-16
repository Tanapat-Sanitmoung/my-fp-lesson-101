using static System.Math;

namespace Tanapat.Playground.Lesson241116;


public static class Exams
{
    public static void Exam01()
    {
        Func<double, double, double> GetBML = 
            (weight, height) => Round(weight / Pow(height, 2), 2);

        var inputWeight = "73"; //kg
        var inputHeight = "1.73"; //meters

        var weight = double.Parse(inputWeight);
        var height = double.Parse(inputHeight);

        Dictionary<int, string> BmiRanges = new() {
            [0] = "Underweight",
            [1] = "Overweight",
            [2] = "Healthyweight"
        };

        var result = GetBML(weight, height) switch
        {
            var bmi when bmi <  18.5 => (Range: 0, BMI: bmi),
            var bmi when bmi >= 25   => (Range: 1, BMI: bmi),
            var bmi                  => (Range: 2, BMI: bmi)
        };

        Console.WriteLine($"BMI range : {BmiRanges[result.Range]}, BMI : {result.BMI}");   
    }
}