using DAL.Models;
using Microsoft.Extensions.DependencyInjection;

namespace DAL.Data
{
    public static class DbSeeder
    {
        public static void Seed(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<PrDBContext>();

            if (context.Products.Any())
                return;

            var warehouse1 = new Warehouse
            {
                Name = "Main Warehouse"
            };

            var warehouse2 = new Warehouse
            {
                Name = "Backup Warehouse"
            };

            context.Warehouses.AddRange(warehouse1, warehouse2);

            var product1 = new Product
            {
                Name = "Laptop",
                SKU = "SKU-001",
                Description = "Gaming Laptop"
            };

            var product2 = new Product
            {
                Name = "Mouse",
                SKU = "SKU-002"
            };

            context.Products.AddRange(product1, product2);

            context.SaveChanges();

            // Stock Transactions
            var transactions = new List<StockTransaction>
        {
            new StockTransaction
            {
                ProductId = product1.Id,
                WarehouseId = warehouse1.Id,
                Quantity = 10,
                Type=TransactionType.In,
                Notes = "Initial stock"
            },
            new StockTransaction
            {
                ProductId = product2.Id,
                WarehouseId = warehouse1.Id,
                Quantity = 50,
                Type=TransactionType.In,
                Notes = "Initial stock"
            }
        };
            

            context.StockTransactions.AddRange(transactions);
            context.SaveChanges();
            var warehouseStock1 = new WarehouseStock
            {
                ProductId=product1.Id,
                WarehouseId=warehouse1.Id,
                Quantity = 10
            };
            var warehouseStock2 = new WarehouseStock
            {
                ProductId=product2.Id,
                WarehouseId=warehouse1.Id,
                Quantity = 50
            };
            context.WarehouseStocks.AddRange(warehouseStock1, warehouseStock2);

            context.SaveChanges();
        }
    }

}
