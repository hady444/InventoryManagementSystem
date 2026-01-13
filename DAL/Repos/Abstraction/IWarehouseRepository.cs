using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos.Abstraction
{
    public interface IWarehouseRepository
    {
        Task<List<Warehouse>> GetAllAsync();
        Task<Warehouse?> GetByIdAsync(int id);
        Task AddAsync(Warehouse warehouse);
        Task UpdateAsync(Warehouse warehouse);
        Task SaveAsync();
    }
}
