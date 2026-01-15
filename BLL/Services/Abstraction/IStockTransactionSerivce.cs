
using BLL.ViewModel;
using Contract;
using DAL.Models;

namespace BLL.Services.Abstraction
{
    public interface IStockTransactionSerivce
    {
        Task<IEnumerable<StockTransactionVM>> GetTransactions(TransactionFilterVM filter);
        Task<PagedResult<StockTransactionVM>> GetTransactions(TransactionFilterVM filter, int pageNumber = 1, int pageSize = 3);
        Task<StockTransaction?> GetByIdAsync(int id);
        Task<StockTransactionVM?> GetDetailsAsync(int id);
        Task<Response> CreateAsync(CreateTransactionVM vm);
        Task<Response> UpdateAsync(int id, CreateTransactionVM vm);
        Task<Response> SoftDeleteAsync(int id);
    }
}
