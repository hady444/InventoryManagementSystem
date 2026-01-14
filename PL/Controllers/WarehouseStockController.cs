using DAL.Repos.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    [Route("WarehouseStock")]
    public class WarehouseStockController : Controller
    {
        private readonly IWarehouseStockRepository _stockRepo;
        
        public WarehouseStockController(IWarehouseStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }
        [HttpGet("GetStock")]
        public async Task<IActionResult> GetStock(int productId, int warehouseId)
        {
            var stock = await _stockRepo.GetOrCreateStockAsync(productId, warehouseId);
            return Json(stock.Quantity);
        }
    }
}
