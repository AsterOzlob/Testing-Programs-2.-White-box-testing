using Testing_Programs_2.Models;

namespace Testing_Programs_2.Services;

public class TestingService
{
    private readonly FunctionService _functionService;
    
    public TestingService(FunctionService functionService)
    {
        _functionService = functionService;
    }

    public List<TestResult> TestStatementCoverage()
    {
        var testCases = new List<FunctionInput>
        {
            new() { A = 1, B = 1, C = 2, X = -1 }, // x<0, b≠0
            new() { A = 1, B = 0, C = 2, X = 1 },  // x>0, b=0
            new() { A = 1, B = 1, C = 2, X = 0 }   // остальные случаи
        };

        return ExecuteTests(testCases);
    }
    
    public List<TestResult> TestDecisionCoverage()
    {
        var testCases = new List<FunctionInput>
        {
            new() { A = 1, B = 1, C = 2, X = -1 }, // x<0, b≠0 - true
            new() { A = 1, B = 0, C = 2, X = 1 },  // x>0, b=0 - true
            new() { A = 1, B = 1, C = 2, X = 1 },  // x>0, b≠0 - false
            new() { A = 1, B = 0, C = 2, X = -1 }  // x<0, b=0 - false
        };

        return ExecuteTests(testCases);
    }
    
    public List<TestResult> TestConditionCoverage()
    {
        var testCases = new List<FunctionInput>
        {
            // x<0, b≠0 - оба условия true
            new() { A = 1, B = 1, C = 2, X = -1 },
            // x<0, b=0 - первое true, второе false
            new() { A = 1, B = 0, C = 2, X = -1 },
            // x>0, b≠0 - первое false, второе true
            new() { A = 1, B = 1, C = 2, X = 1 },
            // x>0, b=0 - оба условия true
            new() { A = 1, B = 0, C = 2, X = 1 }
        };

        return ExecuteTests(testCases);
    }
    
    public List<TestResult> TestDecisionConditionCoverage()
    {
        var testCases = new List<FunctionInput>
        {
            // x<0, b≠0 - оба условия true
            new() { A = 1, B = 1, C = 2, X = -1 },
            // x<0, b=0 - первое true, второе false
            new() { A = 1, B = 0, C = 2, X = -1 },
            // x>0, b≠0 - первое false, второе true
            new() { A = 1, B = 1, C = 2, X = 1 },
            // x>0, b=0 - оба условия true
            new() { A = 1, B = 0, C = 2, X = 1 },
            // остальные случаи
            new() { A = 1, B = 1, C = 2, X = 0 }
        };

        return ExecuteTests(testCases);
    }
    
    public List<TestResult> TestCombinatorialConditionCoverage()
    {
        var testCases = new List<FunctionInput>
        {
            // Все возможные комбинации условий
            new() { A = 1, B = 1, C = 2, X = -1 }, // x<0, b≠0
            new() { A = 1, B = 0, C = 2, X = -1 }, // x<0, b=0
            new() { A = 1, B = 1, C = 2, X = 1 },  // x>0, b≠0
            new() { A = 1, B = 0, C = 2, X = 1 },  // x>0, b=0
            new() { A = 1, B = 1, C = 2, X = 0 },  // остальные случаи
            new() { A = 1, B = 0, C = 2, X = 0 }   // остальные случаи
        };

        return ExecuteTests(testCases);
    }

    private List<TestResult> ExecuteTests(List<FunctionInput> testCases)
    {
        var results = new List<TestResult>();

        foreach (var testCase in testCases)
        {
            var expected = _functionService.CalculateExpectedResult(testCase);
            var (actual, path) = _functionService.CalculateFunction(testCase);
            
            results.Add(new TestResult()
            {
                Input = testCase,
                ExpectedResult = expected,
                ActualResult = actual,
                IsSuccessful = Math.Abs(expected - actual) < 0.0001,
                Path = path
            });
        }
        
        return results; // Added missing return statement
    }
}