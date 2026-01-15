using BLL.ViewModel;
using Contract;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services.Abstraction
{
    public interface IWarehouseService
    {
        Task<List<Warehouse>> GetAllAsync();
        Task<PagedResult<Warehouse>> GetAllAsync(int pageNumber = 1, int pageSize = 3);
        Task<Warehouse?> GetByIdAsync(int id);
        Task<Response> CreateAsync(CreateWarehouseVM vm);
        Task<Response> UpdateAsync(int id, CreateWarehouseVM vm);
        Task<bool> SoftDeleteAsync(int id);
    }
}
