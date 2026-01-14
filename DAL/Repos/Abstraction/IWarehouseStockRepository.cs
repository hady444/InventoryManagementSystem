using DAL.Models;

namespace DAL.Repos.Abstraction
{
    public interface IWarehouseStockRepository
    {
        Task<WarehouseStock> GetOrCreateStockAsync(int productId, int warehouseId);
    }
}
