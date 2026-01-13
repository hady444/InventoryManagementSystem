
using AutoMapper;
using BLL.Services.Abstraction;
using BLL.ViewModel;
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

        public async Task<Warehouse?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<Response> CreateAsync(CreateWarehouseVM vm)
        {
            if (vm == null) return new Response(false, null, null);
            if (vm.Name.Length > 150) return new Response(false, "Name", "Name is too big - Max 50");
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
