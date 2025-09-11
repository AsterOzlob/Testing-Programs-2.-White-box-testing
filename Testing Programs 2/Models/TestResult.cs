namespace Testing_Programs_2.Models;

public class TestResult
{
    public FunctionInput Input { get; set; }
    public double ExpectedResult { get; set; }
    public double ActualResult { get; set; }
    public bool IsSuccessful { get; set; }
    public string Path { get; set; }
}