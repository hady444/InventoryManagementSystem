using BLL.Services.Abstraction;
using BLL.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace PL.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStockSummaryService _summaryService;

        public HomeController(IStockSummaryService summaryService)
        {
            _summaryService = summaryService;
        }
        public async Task<IActionResult> Index()
        {
            var summary = await _summaryService.GetSummaryAsync();
            return View(summary);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
