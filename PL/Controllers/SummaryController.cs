using BLL.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class SummaryController : Controller
    {
        private readonly IStockSummaryService _summaryService;

        public SummaryController(IStockSummaryService summaryService)
        {
            _summaryService = summaryService;
        }

        public async Task<IActionResult> Index()
        {
            var vm = await _summaryService.GetSummaryAsync();
            return View(vm);
        }
    }
}
