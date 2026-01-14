
using DAL.Data;
using DAL.Models;
using DAL.Repos.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repos.Implementation
{
    public class WarehouseStockRepository: IWarehouseStockRepository
    {
        private readonly PrDBContext _ctx;

        public WarehouseStockRepository(PrDBContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<WarehouseStock> GetOrCreateStockAsync(int productId, int warehouseId)
        {
            var stock = await _ctx.WarehouseStocks
                .FirstOrDefaultAsync(s =>
                    s.ProductId == productId &&
                    s.WarehouseId == warehouseId);

            if (stock != null)
                return stock;

            stock = new WarehouseStock
            {
                ProductId = productId,
                WarehouseId = warehouseId,
                Quantity = 0
            };

            await _ctx.WarehouseStocks.AddAsync(stock);
            return stock;
        }
    }
}
