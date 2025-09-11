using Testing_Programs_2.Models;

namespace Testing_Programs_2.Services;

public class FunctionService
{
    public (double result, string path) CalculateFunction(FunctionInput input)
    {
        string path = "";
        double result = 0;

        if (input.X < 0 && input.B != 0)
        {
            path = "x < 0, b != 0";
            result = input.A * Math.Pow(input.X, 3) + input.B * Math.Pow(input.X, 2);
        }
        else if (input.X > 0 && input.B == 0)
        {
            path = "x > 0 b = 0";
            result = (input.X - input.A) / (input.X - input.C);
        }
        else
        {
            path = "Остальные случаи";
            result = (input.X + 5) / (input.C * (input.X - 10));
        }
        
        return (result, path);
    }

    public double CalculateExpectedResult(FunctionInput input)
    {
        return input.X switch
        {
            < 0 when input.B != 0 => input.A * Math.Pow(input.X, 3) + input.B * Math.Pow(input.X, 2),
            > 0 when input.B == 0 => (input.X - input.A) / (input.X - input.C),
            _ => (input.X + 5) / (input.C * (input.X - 10))
        };
    }
}