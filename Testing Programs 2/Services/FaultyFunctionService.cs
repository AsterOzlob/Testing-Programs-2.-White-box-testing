using Testing_Programs_2.Models;

namespace Testing_Programs_2.Services;

public class FaultyFunctionService
{
    public (double result, string path) CalculateFunctionWithErrors(FunctionInput input)
    {
        string path = "";
        double result = 0;

        // ОШИБКА 1: В кубической функции неправильная степень
        if (input.X < 0 && input.B != 0)
        {
            path = "x < 0, b ≠ 0";
            // ОШИБКА: используем x² вместо x³ в первом слагаемом
            result = input.A * Math.Pow(input.X, 2) + input.B * Math.Pow(input.X, 2);
        }
        // ОШИБКА 2: В рациональной функции перепутаны числитель и знаменатель
        else if (input.X > 0 && input.B == 0)
        {
            path = "x > 0, b = 0";
            // ОШИБКА: (x - c)/(x - a) вместо (x - a)/(x - c)
            result = (input.X - input.C) / (input.X - input.A);
        }
        // ОШИБКА 3: В функции по умолчанию неправильная константа
        else
        {
            path = "Остальные случаи";
            // ОШИБКА: используем 6 вместо 5
            result = (input.X + 6) / (input.C * (input.X - 10));
        }
        
        return (result, path);
    }
}
