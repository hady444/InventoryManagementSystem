using BLL.Services.Abstraction;
using BLL.ViewModel;
using Contract;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class WarehouseController : Controller
    {
        private readonly IWarehouseService _service;

        public WarehouseController(IWarehouseService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(int? pageNumber, int? pageSize)
        {
            PagedResult<Warehouse> pagedWarehouses = new PagedResult<Warehouse>();
            List<Warehouse> warehouses = new List<Warehouse>();
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                pagedWarehouses = await _service.GetAllAsync(pageNumber.Value, pageSize.Value);
                return View((pagedWarehouses, warehouses));
            }
            warehouses = await _service.GetAllAsync();
            return View((pagedWarehouses, warehouses));
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateWarehouseVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var response = await _service.CreateAsync(vm);
            if (!response.success) {
                ModelState.AddModelError(response.key??"", response.message??"");
                return View(vm);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var warehouse = await _service.GetByIdAsync(id);
            if (warehouse == null)
                return NotFound();

            var vm = new CreateWarehouseVM
            {
                Name = warehouse.Name,
                Location = warehouse.Location
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateWarehouseVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var response = await _service.UpdateAsync(id, vm);
            if (!response.success)
            {
                if (response.key != null)
                {
                    ModelState.AddModelError(response.key??"", response.message??"");
                    return View(vm);
                }
                    return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int? pageNumber, int? pageSize)
        {
            await _service.SoftDeleteAsync(id);
            return RedirectToAction(nameof(Index), new { pageNumber, pageSize });
        }

        public async Task<IActionResult> Details(int id)
        {
            var warehouse = await _service.GetByIdAsync(id);
            if (warehouse == null)
                return NotFound();

            return View(warehouse);
        }
    }
}
