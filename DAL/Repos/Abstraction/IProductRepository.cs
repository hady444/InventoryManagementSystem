using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos.Abstraction
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task SaveAsync();
    }
}
