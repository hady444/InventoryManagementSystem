
using AutoMapper;
using BLL.Services.Abstraction;
using BLL.ViewModel;
using Contract;
using DAL.Models;
using DAL.Repos.Abstraction;

namespace BLL.Services.Implementation
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository _repo;
        private readonly IMapper _mapper;

        public WarehouseService(IWarehouseRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<Warehouse>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<PagedResult<Warehouse>> GetAllAsync(int pageNumber = 1, int pageSize = 3)
        {
            return await _repo.GetAllPagedAsync(pageNumber, pageSize);
        }

        public async Task<Warehouse?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<Response> CreateAsync(CreateWarehouseVM vm)
        {
            if (vm == null) return new Response(false, null, null);
            if (vm.Name.Length > 150) return new Response(false, "Name", "Name is too big - Max 50");
            var ws = await _repo.GetAllAsync();
            if (ws.Any(w => w.Name == vm.Name)) { return new Response(false, "Name", "Name already Exists"); }
            if (vm.Location != null && vm.Location.Length > 50) return new Response(false, "Location", "Location is too big - Max 200");

            var warehouse = _mapper.Map<Warehouse>(vm);
            try
            {
                await _repo.AddAsync(warehouse);
                await _repo.SaveAsync();
                return new Response(true, null, null);
            }
            catch( Exception ex)
            {
                return new Response(false, null, ex.Message);
            }
        }

        public async Task<Response> UpdateAsync(int id, CreateWarehouseVM vm)
        {
            var warehouse = await _repo.GetByIdAsync(id);
            if (warehouse == null) return new Response(false, null, "item not found");
            if (vm.Name != warehouse.Name)
            {
                var ws = await _repo.GetAllAsync();
                if (ws.Any(w=> vm.Name == w.Name)) { return new Response(false, "Name", "Name already exists"); }
            }
            
            if (vm.Name.Length > 150) return new Response(false, "Name", "Name is too big - Max 50");
            if (vm.Location != null && vm.Location.Length > 50) return new Response(false, "Location", "Location is too big - Max 200");


            warehouse.Name = vm.Name;
            warehouse.Location = vm.Location;
            try
            {
                await _repo.UpdateAsync(warehouse);
                await _repo.SaveAsync();
                return new Response(true, null, null);
            }
            catch(Exception ex)
            {
                return new Response(false, null, ex.Message);
            }
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var warehouse = await _repo.GetByIdAsync(id);
            if (warehouse == null) return false;

            warehouse.IsDeleted = true;
            warehouse.DeletedAt = DateTime.UtcNow;

            await _repo.SaveAsync();
            return true;
        }
    }
}
