using BLL.ViewModel;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services.Abstraction
{
    public interface IWarehouseService
    {
        Task<List<Warehouse>> GetAllAsync();
        Task<Warehouse?> GetByIdAsync(int id);
        Task CreateAsync(CreateWarehouseVM vm);
        Task<bool> UpdateAsync(int id, CreateWarehouseVM vm);
        Task<bool> SoftDeleteAsync(int id);
    }
}
