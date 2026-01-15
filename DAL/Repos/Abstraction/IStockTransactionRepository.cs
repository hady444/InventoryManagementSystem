using Contract;
using DAL.Models;

namespace DAL.Repos.Abstraction
{
    public interface IStockTransactionRepository
    {
        Task<IEnumerable<StockTransactionVM>> GetAllAsync(TransactionFilterVM filter);
        Task<PagedResult<StockTransactionVM>> GetAllAsync(TransactionFilterVM filter, int pageNumber = 1, int pageSize = 3);
        Task CreateAsync(StockTransaction trans);
        Task<StockTransaction?> GetByIdAsync(int id);
        Task<StockTransactionVM?> GetDetailsAsync(int id);
        Task SaveAsync();
        Task<int> GetCurrentStockAsync(int productId, int warehouseId);
    }
}
