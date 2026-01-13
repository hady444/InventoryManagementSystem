using AutoMapper;
using BLL.Services.Abstraction;
using BLL.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _service;
        private readonly IMapper _mapper;

        public ProductController(IProductService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _service.GetAllAsync();
            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var response = await _service.CreateAsync(vm);
            if (!response.success)
            {
                ModelState.AddModelError(response.key??"", response.message??"");
                return View(vm);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            var vm = _mapper.Map<CreateProductVM>(product);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateProductVM vm)
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
                else
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
            var product = await _service.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }
    }
}
