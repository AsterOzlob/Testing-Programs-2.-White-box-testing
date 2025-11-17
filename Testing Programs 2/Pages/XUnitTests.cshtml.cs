using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Testing_Programs_2.Models;
using Testing_Programs_2.Services;

namespace Testing_Programs_2.Pages;

public class XUnitTestsModel : PageModel
{
    private readonly TestingService _testingService;
    private readonly ILogger<XUnitTestsModel> _logger;

    public XUnitTestsModel(TestingService testingService, ILogger<XUnitTestsModel> logger)
    {
        _testingService = testingService;
        _logger = logger;
    }

    public List<TestResult> TestResults { get; set; } = new List<TestResult>();
    public DateTime? LastRunTime { get; set; }

    public void OnGet()
    {
        // Страница загружается без результатов
    }

    public IActionResult OnPost()
    {
        try
        {
            _logger.LogInformation("Запуск xUnit тестов...");
            
            TestResults = _testingService.RunXUnitTests();
            LastRunTime = DateTime.Now;
            
            _logger.LogInformation("xUnit тесты завершены. Результатов: {Count}", TestResults.Count);
            
            TempData["SuccessMessage"] = $"Тестирование завершено! Успешно: {TestResults.Count(r => r.IsSuccessful)} из {TestResults.Count}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при запуске xUnit тестов");
            TempData["ErrorMessage"] = $"Ошибка при запуске тестов: {ex.Message}";
        }

        return Page();
    }
}