using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Testing_Programs_2.Models;
using Testing_Programs_2.Services;

namespace Testing_Programs_2.Pages;

public class IndexModel : PageModel
{
    private readonly TestingService _testingService;

    public IndexModel(TestingService testingService)
    {
        _testingService = testingService;
    }

    [BindProperty]
    public TestingMethod SelectedMethod { get; set; } = TestingMethod.StatementCoverage;

    public List<TestResult> TestResults { get; set; }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        TestResults = SelectedMethod switch
        {
            TestingMethod.StatementCoverage => _testingService.TestStatementCoverage(),
            TestingMethod.DecisionCoverage => _testingService.TestDecisionCoverage(),
            TestingMethod.ConditionCoverage => _testingService.TestConditionCoverage(),
            TestingMethod.DecisionConditionCoverage => _testingService.TestDecisionConditionCoverage(),
            TestingMethod.CombinatorialConditionCoverage => _testingService.TestCombinatorialConditionCoverage(),
            _ => _testingService.TestStatementCoverage()
        };

        return Page();
    }
}