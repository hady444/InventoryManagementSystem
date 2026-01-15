
using AutoMapper;
using BLL.Services.Abstraction;
using BLL.ViewModel;
using Contract;
using DAL.Models;
using DAL.Repos.Abstraction;

namespace BLL.Services.Implementation
{
    public class ProductService: IProductService
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<PagedResult<Product>> GetAllAsync(int pageNumber=1, int pageSize=3)
        {
            return await _repo.GetAllPagedAsync(pageNumber, pageSize);
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }


        public async Task<Response> CreateAsync(CreateProductVM vm)
        {
            if (vm == null) return new Response(false, null, null);
            if (vm.Name.Length > 150) return new Response( false, "Name", "Name is too big - Max 150");
            if (vm.SKU.Length > 50) return new Response(false, "SKU", "SKU is too big - Max 50");
            if (vm.Description != null && vm.Description.Length > 500) return new Response(false, "Description", "Description is too big -  Max 500");
            var prds = await _repo.GetAllNoFilterAsync();
            var check = prds.Any(p => p.SKU == vm.SKU);
            if (check) return new Response(false, "SKU","this SKU is already used");
            var product = _mapper.Map<Product>(vm);
            try
            {
                await _repo.AddAsync(product);
                await _repo.SaveAsync();
                return new Response(true, null, null);
            }
            catch (Exception ex)
            {
                return new Response(false, null, ex.Message);
            }
        }

        public async Task<Response> UpdateAsync(int id, CreateProductVM vm)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null) return new Response(false, null , "item not found");
            if (vm == null) return new Response(false, null, null);
            if (vm.Name.Length > 150) return new Response(false, "Name", "Name is too big - Max 150");
            if (vm.SKU.Length > 50) return new Response(false, "SKU", "SKU is too big - Max 50");
            if (product.SKU != vm.SKU)
            {
                var prds = await _repo.GetAllNoFilterAsync();
                if (prds.Any(p=>p.SKU == vm.SKU)) { return new Response(false, "SKU", "SKU already exists"); }
            }
            if (vm.Description != null && vm.Description.Length > 500) return new Response(false, "Description", "Description is too big -  Max 500");
            product.Name = vm.Name;
            product.SKU = vm.SKU;
            product.Description = vm.Description;
            try
            {
                await _repo.SaveAsync();
                return new Response(true, null, null);
            }
            catch (Exception ex)
            {
                return new Response(false, null, ex.Message);
            }
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null) return false;

            product.IsDeleted = true;
            product.DeletedAt = DateTime.UtcNow;

            await _repo.SaveAsync();
            return true;
        }
    }
}
