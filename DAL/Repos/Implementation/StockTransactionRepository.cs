
using Contract;
using DAL.Data;
using DAL.Models;
using DAL.Repos.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repos.Implementation
{
    public class StockTransactionRepository:IStockTransactionRepository
    {
        private readonly PrDBContext _ctx;
        public StockTransactionRepository(PrDBContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<IEnumerable<StockTransactionVM>> GetAllAsync(TransactionFilterVM filter)
        {
            var query = _ctx.StockTransactions.AsNoTracking().AsQueryable();
            if (filter.ProductId.HasValue)
            {
                query= query.Where(t => t.ProductId == filter.ProductId);
            }
            if (filter.WarehouseId.HasValue)
            {
                query= query.Where(t => t.WarehouseId == filter.WarehouseId);
            }
            return await query.Select(t => new StockTransactionVM
            {
                Id = t.Id,
                Quantity = t.Quantity,
                TransactionType = t.Type.ToString(),
                ProductName = t.Product.Name,
                WarehouseName = t.Warehouse.Name,
                Notes = t.Notes,
                TransactionDate = t.TransactionDate
            }).ToListAsync();
        }
        public async Task CreateAsync(StockTransaction trans)
        {
            await _ctx.StockTransactions.AddAsync(trans);
        }
        public async Task<StockTransaction?> GetByIdAsync(int id)
        {
            return await _ctx.StockTransactions
                .FirstOrDefaultAsync(t => t.Id == id);
        }
        public async Task<StockTransactionVM?> GetDetailsAsync(int id)
        {
            return await _ctx.StockTransactions.Select(t => new StockTransactionVM
            {
                Quantity = t.Quantity,
                TransactionType = t.Type.ToString(),
                ProductName = t.Product.Name,
                WarehouseName = t.Warehouse.Name,
                Notes = t.Notes,
                TransactionDate = t.TransactionDate
            }).FirstOrDefaultAsync();
            
        }
        public async Task SaveAsync()
        {
            await _ctx.SaveChangesAsync();
        }
        
        

        public async Task<int> GetCurrentStockAsync(int productId, int warehouseId)
        {
            return await _ctx.StockTransactions
                .AsNoTracking()
                .Where(t =>
                    t.ProductId == productId &&
                    t.WarehouseId == warehouseId)
                .SumAsync(t =>
                    t.Type == TransactionType.In
                        ? t.Quantity
                        : -t.Quantity);
        }

    }
}
