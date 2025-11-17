using Testing_Programs_2.Models;

namespace Testing_Programs_2.Services;

public class TestingService
{
    private readonly FunctionService _functionService;
    private readonly FaultyFunctionService _faultyFunctionService;
    private readonly Random _random;

    public TestingService(FunctionService functionService, FaultyFunctionService faultyFunctionService)
    {
        _functionService = functionService;
        _faultyFunctionService = faultyFunctionService;
        _random = new Random();
    }

    public List<TestResult> RunRandomTests(int numberOfTests)
    {
        var results = new List<TestResult>();

        for (int i = 0; i < numberOfTests; i++)
        {
            // Генерируем случайные параметры
            var a = GenerateRandomDouble(-10, 10);
            var b = GenerateRandomDouble(-10, 10);
            var c = GenerateRandomDouble(-10, 10);
            var x = GenerateRandomDouble(-15, 15);

            // Убедимся, что c ≠ 0 для избежания деления на 0 в некоторых случаях
            if (Math.Abs(c) < 0.001) c = 1.0;

            var testResult = ExecuteComparisonTest(a, b, c, x, $"Случайный тест {i + 1}");
            results.Add(testResult);
        }

        return results;
    }

    public List<TestResult> RunTargetedTests(int testsPerCategory)
    {
        var results = new List<TestResult>();

        // Целевые тесты для каждой ветки функции
        results.AddRange(GenerateCubicFunctionTests(testsPerCategory));
        results.AddRange(GenerateRationalFunctionTests(testsPerCategory));
        results.AddRange(GenerateDefaultFunctionTests(testsPerCategory));
        results.AddRange(GenerateBoundaryTests(testsPerCategory));

        return results;
    }

    private List<TestResult> GenerateCubicFunctionTests(int count)
    {
        var results = new List<TestResult>();

        for (int i = 0; i < count; i++)
        {
            var x = GenerateRandomDouble(-15, -0.1); // x < 0
            var b = GenerateRandomDouble(0.1, 10);   // b ≠ 0
            var a = GenerateRandomDouble(-10, 10);
            var c = GenerateRandomDouble(-10, 10);

            var testResult = ExecuteComparisonTest(a, b, c, x, "Кубическая функция");
            results.Add(testResult);
        }

        return results;
    }

    private List<TestResult> GenerateRationalFunctionTests(int count)
    {
        var results = new List<TestResult>();

        for (int i = 0; i < count; i++)
        {
            var x = GenerateRandomDouble(0.1, 15);   // x > 0
            var b = 0.0;                            // b = 0
            var a = GenerateRandomDouble(-10, 10);
            var c = GenerateRandomDouble(-10, 10);

            // Убедимся, что x ≠ c для избежания деления на 0
            if (Math.Abs(x - c) < 0.1) c = x + 1.0;

            var testResult = ExecuteComparisonTest(a, b, c, x, "Рациональная функция");
            results.Add(testResult);
        }

        return results;
    }

    private List<TestResult> GenerateDefaultFunctionTests(int count)
    {
        var results = new List<TestResult>();

        for (int i = 0; i < count; i++)
        {
            // Случай, когда x ≥ 0 и b ≠ 0, или другие комбинации
            var scenario = _random.Next(3);
            double x, b;

            switch (scenario)
            {
                case 0: // x ≥ 0 и b ≠ 0
                    x = GenerateRandomDouble(0, 15);
                    b = GenerateRandomDouble(0.1, 10);
                    break;
                case 1: // x < 0 и b = 0
                    x = GenerateRandomDouble(-15, -0.1);
                    b = 0.0;
                    break;
                default: // x = 0 и b ≠ 0
                    x = 0.0;
                    b = GenerateRandomDouble(0.1, 10);
                    break;
            }

            var a = GenerateRandomDouble(-10, 10);
            var c = GenerateRandomDouble(-10, 10);

            // Убедимся, что x ≠ 10 для избежания деления на 0
            if (Math.Abs(x - 10) < 0.1) x = 9.0;

            var testResult = ExecuteComparisonTest(a, b, c, x, "Функция по умолчанию");
            results.Add(testResult);
        }

        return results;
    }

    private List<TestResult> GenerateBoundaryTests(int count)
    {
        var results = new List<TestResult>();

        var boundaryCases = new[]
        {
            // Граничные значения x вокруг 0
            new { X = -0.0001, B = 1.0, Description = "x ≈ -0" },
            new { X = 0.0001, B = 0.0, Description = "x ≈ +0" },
            new { X = 0.0, B = 1.0, Description = "x = 0" },
            
            // Граничные значения b вокруг 0
            new { X = -5.0, B = 0.0001, Description = "b ≈ +0" },
            new { X = -5.0, B = -0.0001, Description = "b ≈ -0" },
            new { X = 5.0, B = 0.0001, Description = "b ≈ +0 при x > 0" },
            
            // Граничные значения для деления на 0
            new { X = 10.0, B = 1.0, Description = "x = 10 (деление на 0)" },
            new { X = 10.1, B = 0.0, Description = "x ≈ 10" },
            new { X = 9.9, B = 0.0, Description = "x ≈ 10" }
        };

        for (int i = 0; i < Math.Min(count, boundaryCases.Length); i++)
        {
            var @case = boundaryCases[i];
            var a = GenerateRandomDouble(-5, 5);
            var c = GenerateRandomDouble(-5, 5);

            var testResult = ExecuteComparisonTest(a, @case.B, c, @case.X, $"Граничный случай: {@case.Description}");
            results.Add(testResult);
        }

        // Если нужно больше тестов, генерируем дополнительные
        for (int i = boundaryCases.Length; i < count; i++)
        {
            var x = GenerateRandomDouble(-0.1, 0.1); // Вокруг 0
            var b = GenerateRandomDouble(-0.1, 0.1); // Вокруг 0
            var a = GenerateRandomDouble(-5, 5);
            var c = GenerateRandomDouble(-5, 5);

            var testResult = ExecuteComparisonTest(a, b, c, x, "Граничный случай");
            results.Add(testResult);
        }

        return results;
    }

    public List<TestResult> RunXUnitTests()
    {
        var xunitRunner = new XUnitTestRunner();
        return xunitRunner.RunXUnitTests();
    }

    private TestResult ExecuteComparisonTest(double a, double b, double c, double x, string testMethod)
    {
        var input = new FunctionInput { A = a, B = b, C = c, X = x };
        
        // Вычисляем эталонный результат
        var (referenceResult, referencePath) = _functionService.CalculateFunction(input);
        
        // Вычисляем результат ошибочной функции
        var (faultyResult, faultyPath) = _faultyFunctionService.CalculateFunctionWithErrors(input);

        // УСПЕШНО, если результаты РАЗЛИЧАЮТСЯ (выявили ошибку)
        // НЕУСПЕШНО, если результаты СОВПАЛИ (не выявили ошибку)
        bool isSuccessful = !AreEqualWithTolerance(referenceResult, faultyResult);

        return new TestResult
        {
            Input = input,
            ExpectedResult = referenceResult,
            ActualResult = faultyResult,
            IsSuccessful = isSuccessful,
            Path = referencePath,
            TestMethod = testMethod,
            ReferencePath = referencePath,
            FaultyPath = faultyPath
        };
    }

    private double GenerateRandomDouble(double minValue, double maxValue)
    {
        return _random.NextDouble() * (maxValue - minValue) + minValue;
    }

    private bool AreEqualWithTolerance(double expected, double actual)
    {
        if (double.IsNaN(expected) && double.IsNaN(actual))
            return true;
        
        if (double.IsInfinity(expected) && double.IsInfinity(actual))
            return true;

        return Math.Abs(expected - actual) < 0.0001;
    }
}