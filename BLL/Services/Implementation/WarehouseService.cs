
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

        public async Task CreateAsync(CreateWarehouseVM vm)
        {
            var warehouse = _mapper.Map<Warehouse>(vm);

            await _repo.AddAsync(warehouse);
            await _repo.SaveAsync();
        }

        public async Task<bool> UpdateAsync(int id, CreateWarehouseVM vm)
        {
            var warehouse = await _repo.GetByIdAsync(id);
            if (warehouse == null) return false;

            warehouse.Name = vm.Name;
            warehouse.Location = vm.Location;

            await _repo.UpdateAsync(warehouse);
            await _repo.SaveAsync();

            return true;
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
