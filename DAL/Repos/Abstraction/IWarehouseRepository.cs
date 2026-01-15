using Contract;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos.Abstraction
{
    public interface IWarehouseRepository
    {
        Task<List<Warehouse>> GetAllAsync();
        Task<List<Warehouse>> GetAllNoFilterAsync();
        Task<PagedResult<Warehouse>> GetAllPagedAsync(int pageNumber = 1, int pageSize = 3);
        Task<Warehouse?> GetByIdAsync(int id);
        Task AddAsync(Warehouse warehouse);
        Task UpdateAsync(Warehouse warehouse);
        Task SaveAsync();
    }
}
