using BLL.ViewModel;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services.Abstraction
{
    public interface IProductService
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Response> CreateAsync(CreateProductVM vm);
        Task<Response> UpdateAsync(int id, CreateProductVM vm);
        Task<bool> SoftDeleteAsync(int id);
    }
}
