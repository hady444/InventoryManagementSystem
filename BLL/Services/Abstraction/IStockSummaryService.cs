
using BLL.ViewModel;

namespace BLL.Services.Abstraction
{
    public interface IStockSummaryService
    {
        Task<StockSummaryVM> GetSummaryAsync();
    }
}
