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
                ModelState.AddModelError(response.key, response.message);
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
        public async Task<IActionResult> Delete(int id)
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

        //public async Task<IActionResult> Index()
        //{
        //    var warehouses = await _ctx.Warehouses
        //        .ToListAsync();

        //    return View(warehouses);
        //}
        //public IActionResult Create()
        //{
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(CreateWarehouseVM vm)
        //{
        //    if (!ModelState.IsValid)
        //        return View(vm);

        //    var warehouse = new Warehouse
        //    {
        //        Name = vm.Name,
        //        Location = vm.Location
        //    };

        //    await _ctx.Warehouses.AddAsync(warehouse);
        //    await _ctx.SaveChangesAsync();

        //    return RedirectToAction(nameof(Index));
        //}
        //public async Task<IActionResult> Edit(int id)
        //{
        //    var warehouse = await _ctx.Warehouses
        //        .FirstOrDefaultAsync(w => w.Id == id);

        //    if (warehouse == null)
        //        return NotFound();

        //    var vm = new CreateWarehouseVM
        //    {
        //        Name = warehouse.Name,
        //        Location = warehouse.Location
        //    };

        //    return View(vm);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, CreateWarehouseVM vm)
        //{
        //    if (!ModelState.IsValid)
        //        return View(vm);

        //    var warehouse = await _ctx.Warehouses.FindAsync(id);
        //    if (warehouse == null)
        //        return NotFound();

        //    warehouse.Name = vm.Name;
        //    warehouse.Location = vm.Location;

        //    await _ctx.SaveChangesAsync();

        //    return RedirectToAction(nameof(Index));
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var warehouse = await _ctx.Warehouses.FindAsync(id);
        //    if (warehouse == null)
        //        return NotFound();

        //    warehouse.IsDeleted = true;
        //    warehouse.DeletedAt = DateTime.UtcNow;

        //    await _ctx.SaveChangesAsync();

        //    return RedirectToAction(nameof(Index));
        //}
        //public async Task<IActionResult> Details(int id)
        //{
        //    var warehouse = await _ctx.Warehouses
        //        .FirstOrDefaultAsync(w => w.Id == id);

        //    if (warehouse == null)
        //        return NotFound();
            
        //    return View(warehouse);
        //}
    }
}
