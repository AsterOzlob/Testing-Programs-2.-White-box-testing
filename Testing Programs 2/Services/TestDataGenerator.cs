using Testing_Programs_2.Models;

namespace Testing_Programs_2.Services;

public class TestDataGenerator
{
    private readonly Random _random = new();

    public List<FunctionInput> GenerateForMethod(TestingMethod method, int count = 10)
    {
        return method switch
        {
            TestingMethod.StatementCoverage => GenerateStatementCoverageData(count),
            TestingMethod.DecisionCoverage => GenerateDecisionCoverageData(count),
            TestingMethod.ConditionCoverage => GenerateConditionCoverageData(count),
            TestingMethod.DecisionConditionCoverage => GenerateDecisionConditionCoverageData(count),
            TestingMethod.CombinatorialConditionCoverage => GenerateCombinatorialConditionCoverageData(count),
            _ => GenerateRandomData(count)
        };
    }
    
    private List<FunctionInput> GenerateStatementCoverageData(int count)
    {
        var data = new List<FunctionInput>();
        
        // Гарантируем минимум по одному тесту для каждой ветки
        data.Add(new() { A = 1, B = 1, C = 2, X = -1 }); // Первая ветка
        data.Add(new() { A = 1, B = 0, C = 2, X = 1 });  // Вторая ветка
        data.Add(new() { A = 1, B = 1, C = 2, X = 0 });  // Третья ветка
        
        // Добавляем случайные данные
        for (int i = 0; i < count - 3; i++)
        {
            data.Add(GenerateRandomInput());
        }
        
        return data;
    }
    
    private List<FunctionInput> GenerateDecisionCoverageData(int count)
    {
        var data = new List<FunctionInput>();
        
        // Гарантируем покрытие всех решений
        data.Add(new() { A = 1, B = 1, C = 2, X = -1 }); // x<0, b≠0 - true
        data.Add(new() { A = 1, B = 0, C = 2, X = -1 }); // x<0, b=0 - false
        data.Add(new() { A = 1, B = 1, C = 2, X = 1 });  // x>0, b≠0 - false
        data.Add(new() { A = 1, B = 0, C = 2, X = 1 });  // x>0, b=0 - true
        
        for (int i = 0; i < count - 4; i++)
        {
            data.Add(GenerateRandomInput());
        }
        
        return data;
    }

    private List<FunctionInput> GenerateConditionCoverageData(int count)
    {
        var data = new List<FunctionInput>();
        
        // Покрываем все элементарные условия
        // Условия: x<0, b≠0, x>0, b=0
        
        // x<0 (true), b≠0 (true)
        data.Add(new() { A = 1, B = 1, C = 2, X = -1 });
        // x<0 (true), b=0 (false)
        data.Add(new() { A = 1, B = 0, C = 2, X = -1 });
        // x>0 (true), b≠0 (true) - но это false для второго условия
        data.Add(new() { A = 1, B = 1, C = 2, X = 1 });
        // x>0 (true), b=0 (true)
        data.Add(new() { A = 1, B = 0, C = 2, X = 1 });
        // x=0 (false), b≠0 (true) - остальные случаи
        data.Add(new() { A = 1, B = 1, C = 2, X = 0 });
        // x=0 (false), b=0 (false) - остальные случаи
        data.Add(new() { A = 1, B = 0, C = 2, X = 0 });

        for (int i = 0; i < count - 6; i++)
        {
            data.Add(GenerateRandomInput());
        }
        
        return data;
    }

    private List<FunctionInput> GenerateDecisionConditionCoverageData(int count)
    {
        var data = new List<FunctionInput>();
        
        // Комбинация покрытия решений и условий
        data.Add(new() { A = 1, B = 1, C = 2, X = -1 }); // x<0, b≠0 - оба true
        data.Add(new() { A = 1, B = 0, C = 2, X = -1 }); // x<0, b=0 - первое true, второе false
        data.Add(new() { A = 1, B = 1, C = 2, X = 1 });  // x>0, b≠0 - первое false, второе true
        data.Add(new() { A = 1, B = 0, C = 2, X = 1 });  // x>0, b=0 - оба true
        data.Add(new() { A = 1, B = 1, C = 2, X = 0 });  // остальные случаи - оба false
        data.Add(new() { A = 1, B = 0, C = 2, X = 0 });  // остальные случаи - оба false

        for (int i = 0; i < count - 6; i++)
        {
            data.Add(GenerateRandomInput());
        }
        
        return data;
    }

    private List<FunctionInput> GenerateCombinatorialConditionCoverageData(int count)
    {
        var data = new List<FunctionInput>();
        
        // Все возможные комбинации условий
        data.Add(new() { A = 1, B = 1, C = 2, X = -1 }); // x<0, b≠0
        data.Add(new() { A = 1, B = 0, C = 2, X = -1 }); // x<0, b=0
        data.Add(new() { A = 1, B = 1, C = 2, X = 1 });  // x>0, b≠0
        data.Add(new() { A = 1, B = 0, C = 2, X = 1 });  // x>0, b=0
        data.Add(new() { A = 1, B = 1, C = 2, X = 0 });  // остальные случаи, b≠0
        data.Add(new() { A = 1, B = 0, C = 2, X = 0 });  // остальные случаи, b=0

        for (int i = 0; i < count - 6; i++)
        {
            data.Add(GenerateRandomInput());
        }
        
        return data;
    }

    private List<FunctionInput> GenerateRandomData(int count)
    {
        var data = new List<FunctionInput>();
        for (int i = 0; i < count; i++)
        {
            data.Add(GenerateRandomInput());
        }
        return data;
    }
    
    private FunctionInput GenerateRandomInput()
    {
        return new FunctionInput
        {
            A = _random.NextDouble() * 10 - 5, // -5 до 5
            B = _random.NextDouble() * 10 - 5,
            C = _random.NextDouble() * 10 - 5,
            X = _random.NextDouble() * 20 - 10 // -10 до 10
        };
    }
}