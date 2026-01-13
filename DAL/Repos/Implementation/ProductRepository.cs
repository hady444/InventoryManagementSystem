using DAL.Data;
using DAL.Models;
using DAL.Repos.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repos.Implementation
{
    public class ProductRepository: IProductRepository
    {
        private readonly PrDBContext _ctx;

        public ProductRepository(PrDBContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _ctx.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _ctx.Products
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Product product)
        {
            await _ctx.Products.AddAsync(product);
        }

        public async Task SaveAsync()
        {
            await _ctx.SaveChangesAsync();
        }
    }
}
