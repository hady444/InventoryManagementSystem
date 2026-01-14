using BLL.Services.Abstraction;
using BLL.ViewModel;
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

        public async Task<IActionResult> Index()
        {
            var warehouses = await _service.GetAllAsync();
            return View(warehouses);
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
        public async Task<IActionResult> Reverse(int id)
        {
            await _service.SoftDeleteAsync(id);
            return RedirectToAction(nameof(Index));
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
