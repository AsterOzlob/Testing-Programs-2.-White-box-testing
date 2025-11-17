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
    public string TestType { get; set; } = "Random";

    [BindProperty]
    public int NumberOfTests { get; set; } = 10;

    [BindProperty]
    public int TestsPerCategory { get; set; } = 5;

    public List<TestResult> TestResults { get; set; }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (TestType == "Random")
        {
            TestResults = _testingService.RunRandomTests(NumberOfTests);
        }
        else if (TestType == "Targeted")
        {
            TestResults = _testingService.RunTargetedTests(TestsPerCategory);
        }

        return Page();
    }

    public IActionResult OnPostRunXUnitTests()
    {
        TestResults = _testingService.RunXUnitTests();
        return Page();
    }
}