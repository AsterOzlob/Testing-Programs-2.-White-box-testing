
using Testing_Programs_2.Models;
using Testing_Programs_2.Services;
using Xunit;

namespace Testing_Programs_2.Tests;

public class FunctionServiceTests
{
    private readonly FunctionService _functionService;

    public FunctionServiceTests()
    {
        _functionService = new FunctionService();
    }

    [Theory]
    [InlineData(1.0, 1.0, 2.0, -1.0, 0.0)]  // a*x³ + b*x² = 1*(-1) + 1*1 = -1 + 1 = 0
    [InlineData(2.0, 3.0, 1.0, -2.0, -4.0)] // 2*(-8) + 3*4 = -16 + 12 = -4
    [InlineData(0.5, 2.0, 3.0, -3.0, -13.5)] // 0.5*(-27) + 2*9 = -13.5 + 18 = 4.5
    public void CalculateFunction_WhenXLessThanZeroAndBNotZero_ReturnsCubicFunction(
        double a, double b, double c, double x, double expected)
    {
        // Arrange
        var input = new FunctionInput { A = a, B = b, C = c, X = x };

        // Act
        var (result, path) = _functionService.CalculateFunction(input);

        // Assert
        Assert.Equal(expected, result, 4);
        Assert.Equal("x < 0, b ≠ 0", path);
    }

    [Theory]
    [InlineData(1.0, 0.0, 2.0, 1.0, 0.0)]           // (1-1)/(1-2) = 0/-1 = 0
    [InlineData(2.0, 0.0, 3.0, 5.0, 1.5)]          // (5-2)/(5-3) = 3/2 = 1.5
    [InlineData(3.0, 0.0, 1.0, 4.0, 0.3333)]       // (4-3)/(4-1) = 1/3 ≈ 0.3333
    public void CalculateFunction_WhenXGreaterThanZeroAndBZero_ReturnsRationalFunction(
        double a, double b, double c, double x, double expected)
    {
        // Arrange
        var input = new FunctionInput { A = a, B = b, C = c, X = x };

        // Act
        var (result, path) = _functionService.CalculateFunction(input);

        // Assert
        Assert.Equal(expected, result, 4);
        Assert.Equal("x > 0, b = 0", path);
    }

    [Theory]
    [InlineData(1.0, 1.0, 2.0, 0.0, -0.25)]        // (0+5)/(2*(0-10)) = 5/(2*(-10)) = -0.25
    [InlineData(1.0, 0.0, 1.0, -1.0, -0.3636)]     // (-1+5)/(1*(-1-10)) = 4/(1*(-11)) ≈ -0.3636
    [InlineData(2.0, 2.0, 3.0, 5.0, -0.6667)]      // (5+5)/(3*(5-10)) = 10/(3*(-5)) ≈ -0.6667
    public void CalculateFunction_DefaultCase_ReturnsDefaultFunction(
        double a, double b, double c, double x, double expected)
    {
        // Arrange
        var input = new FunctionInput { A = a, B = b, C = c, X = x };

        // Act
        var (result, path) = _functionService.CalculateFunction(input);

        // Assert
        Assert.Equal(expected, result, 4);
        Assert.Equal("Остальные случаи", path);
    }

    [Theory]
    [InlineData(1.0, 0.0, 1.0, 1.0, double.NaN)]   // Деление на 0: (1-1)/(1-1) = 0/0
    [InlineData(1.0, 0.0, 10.0, 10.0, double.NaN)] // Деление на 0: (10-1)/(10-10) = 9/0
    public void CalculateFunction_DivisionByZero_ReturnsNaN(
        double a, double b, double c, double x, double expected)
    {
        // Arrange
        var input = new FunctionInput { A = a, B = b, C = c, X = x };

        // Act
        var (result, path) = _functionService.CalculateFunction(input);

        // Assert
        Assert.True(double.IsNaN(result));
        Assert.True(double.IsNaN(expected));
    }

    [Fact]
    public void CalculateFunction_BoundaryCase_XNegativeWithBZero_ReturnsDefaultFunction()
    {
        // Arrange
        var input = new FunctionInput { A = 1.0, B = 0.0, C = 2.0, X = -1.0 };

        // Act
        var (result, path) = _functionService.CalculateFunction(input);

        // Assert
        Assert.Equal("Остальные случаи", path);
        Assert.Equal(-0.3636, result, 4);
    }

    [Fact]
    public void CalculateFunction_BoundaryCase_XZeroWithBNotZero_ReturnsDefaultFunction()
    {
        // Arrange
        var input = new FunctionInput { A = 1.0, B = 1.0, C = 2.0, X = 0.0 };

        // Act
        var (result, path) = _functionService.CalculateFunction(input);

        // Assert
        Assert.Equal("Остальные случаи", path);
        Assert.Equal(-0.25, result, 4);
    }
}