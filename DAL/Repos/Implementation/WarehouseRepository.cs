using DAL.Data;
using DAL.Models;
using DAL.Repos.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repos.Implementation
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly PrDBContext _ctx;

        public WarehouseRepository(PrDBContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<List<Warehouse>> GetAllAsync()
        {
            return await _ctx.Warehouses.ToListAsync();
        }

        public async Task<Warehouse?> GetByIdAsync(int id)
        {
            return await _ctx.Warehouses
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task AddAsync(Warehouse warehouse)
        {
            await _ctx.Warehouses.AddAsync(warehouse);
        }

        public Task UpdateAsync(Warehouse warehouse)
        {
            _ctx.Warehouses.Update(warehouse);
            return Task.CompletedTask;
        }

        public async Task SaveAsync()
        {
            await _ctx.SaveChangesAsync();
        }
    }
}
