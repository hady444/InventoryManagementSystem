using BLL.Services.Abstraction;
using BLL.ViewModel;
using Contract;
using DAL.Models;
using DAL.Repos.Abstraction;

namespace BLL.Services.Implementation
{
    public class StockSummaryService : IStockSummaryService
    {
        private readonly IStockTransactionRepository _transactionRepo;
        private readonly IProductRepository _productRepo;
        private readonly IWarehouseRepository _warehouseRepo;

        public StockSummaryService(
            IStockTransactionRepository transactionRepo,
            IProductRepository productRepo,
            IWarehouseRepository warehouseRepo)
        {
            _transactionRepo = transactionRepo;
            _productRepo = productRepo;
            _warehouseRepo = warehouseRepo;
        }

        public async Task<StockSummaryVM> GetSummaryAsync()
        {
            var transactions = await _transactionRepo.GetAllAsync(new TransactionFilterVM());

            var totalIn = transactions
                .Where(t => t.TransactionType == TransactionType.In.ToString())
                .Sum(t => t.Quantity);

            var totalOut = transactions
                .Where(t => t.TransactionType == TransactionType.Out.ToString())
                .Sum(t => t.Quantity);

            var lowStock = (await _transactionRepo.GetAllAsync(new TransactionFilterVM()))
                .Where(s => s.Quantity <= 5) // 5 is the threshold
                .Select(s => new LowStockVM
                {
                    ProductName = s.ProductName,
                    WarehouseName = s.WarehouseName,
                    Quantity = s.Quantity
                }).ToList();

            return new StockSummaryVM
            {
                TotalStockIn = totalIn,
                TotalStockOut = totalOut,
                TotalProducts = (await _productRepo.GetAllAsync()).Count,
                TotalWarehouses = (await _warehouseRepo.GetAllAsync()).Count,
                LowStockProducts = lowStock
            };
        }
    }
}