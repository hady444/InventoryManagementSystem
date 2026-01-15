using BLL.Services.Abstraction;
using BLL.ViewModel;
using Contract;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PL.Controllers
{
    public class TransactionController : Controller
    {
        private readonly IStockTransactionSerivce _service;
        private readonly IProductService _productService;
        private readonly IWarehouseService _warehouseService;
        public TransactionController(IStockTransactionSerivce service, IProductService productService,
            IWarehouseService warehouseService)
        {
            _service = service;
            _productService = productService;
            _warehouseService = warehouseService;
        }
        public async Task<IActionResult> Index(TransactionFilterVM? filter, int? pageNumber, int? pageSize)
        {
            PagedResult<StockTransactionVM> pagedTransactions = new PagedResult<StockTransactionVM>();
            IEnumerable<StockTransactionVM> transactions = new List<StockTransactionVM>();
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                pagedTransactions = await _service.GetTransactions(filter ?? new TransactionFilterVM(), pageNumber.Value, pageSize.Value);
                await PopulateDropdownsAsync();
                return View((pagedTransactions, transactions));
            }
            transactions = await _service.GetTransactions(filter ?? new TransactionFilterVM());
            await PopulateDropdownsAsync();
            return View((pagedTransactions, transactions));
        }
        public async Task<IActionResult> Details(int id)
        {
            var transaction = await _service.GetDetailsAsync(id);
            if (transaction == null) return NotFound();
            return View(transaction);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateDropdownsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTransactionVM vm)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
                return View(vm);
            }
            var response = await _service.CreateAsync(vm);

            if (!response.success)
            {
                if (!string.IsNullOrEmpty(response.key))
                    ModelState.AddModelError(response.key, response.message ?? "Error");
                else
                    ModelState.AddModelError(string.Empty, response.message ?? "Error");
                await PopulateDropdownsAsync();
                return View(vm);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var transaction = await _service.GetByIdAsync(id);
            if (transaction == null) return NotFound();

            var vm = new CreateTransactionVM
            {
                ProductId = transaction.ProductId,
                WarehouseId = transaction.WarehouseId,
                Type = transaction.Type,
                Quantity = transaction.Quantity,
                Notes = transaction.Notes
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateTransactionVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var response = await _service.UpdateAsync(id, vm);

            if (!response.success)
            {
                if (!string.IsNullOrEmpty(response.key))
                    ModelState.AddModelError(response.key, response.message ?? "Error");
                else
                    ModelState.AddModelError(string.Empty, response.message ?? "Error");

                return View(vm);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reverse(int id, int? warehouseId, int? productId, int? pageNumber, int? pageSize)
        {
            var response = await _service.SoftDeleteAsync(id);

            if (!response.success)
            {
                TempData["ErrorMessage"] = response.message;
            }

            return RedirectToAction(nameof(Index), new { warehouseId, productId, pageNumber, pageSize });
        }
        private async Task PopulateDropdownsAsync()
        {
            var products = await _productService.GetAllAsync();
            ViewBag.Products = products.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            });

            var warehouses = await _warehouseService.GetAllAsync();
            ViewBag.Warehouses = warehouses.Select(w => new SelectListItem
            {
                Value = w.Id.ToString(),
                Text = w.Name
            });
        }
    }
}
