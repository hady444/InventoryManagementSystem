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

            // =========================
            // Warehouses
            // =========================
            var warehouses = new List<Warehouse>
            {
                new Warehouse { Name = "Main Warehouse" },
                new Warehouse { Name = "Backup Warehouse" },
                new Warehouse { Name = "Outlet Warehouse" }
            };

            context.Warehouses.AddRange(warehouses);
            context.SaveChanges();

            // =========================
            // Products
            // =========================
            var products = new List<Product>
            {
                new Product { Name = "Gaming Laptop", SKU = "SKU-001", Description = "High-end gaming laptop" },
                new Product { Name = "Office Laptop", SKU = "SKU-002", Description = "Business laptop" },
                new Product { Name = "Mouse", SKU = "SKU-003", Description = "Wireless mouse" },
                new Product { Name = "Keyboard", SKU = "SKU-004", Description = "Mechanical keyboard" },
                new Product { Name = "Monitor", SKU = "SKU-005", Description = "27-inch monitor" }
            };

            context.Products.AddRange(products);
            context.SaveChanges();

            // =========================
            // Stock Transactions (SOURCE OF TRUTH)
            // =========================
            var transactions = new List<StockTransaction>
            {
                // =========================
                // Main Warehouse
                // =========================
                new StockTransaction { ProductId = products[0].Id, WarehouseId = warehouses[0].Id, Quantity = 30, Type = TransactionType.In, Notes = "Initial stock" },
                new StockTransaction { ProductId = products[0].Id, WarehouseId = warehouses[0].Id, Quantity = 5,  Type = TransactionType.Out, Notes = "Sold items" },
                new StockTransaction { ProductId = products[0].Id, WarehouseId = warehouses[0].Id, Quantity = 10, Type = TransactionType.In, Notes = "Restock" },

                new StockTransaction { ProductId = products[1].Id, WarehouseId = warehouses[0].Id, Quantity = 20, Type = TransactionType.In, Notes = "Initial stock" },
                new StockTransaction { ProductId = products[1].Id, WarehouseId = warehouses[0].Id, Quantity = 3,  Type = TransactionType.Out, Notes = "Sold items" },

                new StockTransaction { ProductId = products[2].Id, WarehouseId = warehouses[0].Id, Quantity = 150, Type = TransactionType.In, Notes = "Initial stock" },
                new StockTransaction { ProductId = products[2].Id, WarehouseId = warehouses[0].Id, Quantity = 20,  Type = TransactionType.Out, Notes = "Bulk sale" },
                new StockTransaction { ProductId = products[2].Id, WarehouseId = warehouses[0].Id, Quantity = 10,  Type = TransactionType.Out, Notes = "Damaged items" },

                new StockTransaction { ProductId = products[3].Id, WarehouseId = warehouses[0].Id, Quantity = 50, Type = TransactionType.In, Notes = "Initial stock" },

                // =========================
                // Backup Warehouse
                // =========================
                new StockTransaction { ProductId = products[0].Id, WarehouseId = warehouses[1].Id, Quantity = 15, Type = TransactionType.In, Notes = "Backup stock" },
                new StockTransaction { ProductId = products[0].Id, WarehouseId = warehouses[1].Id, Quantity = 2,  Type = TransactionType.Out, Notes = "Transferred out" },

                new StockTransaction { ProductId = products[1].Id, WarehouseId = warehouses[1].Id, Quantity = 10, Type = TransactionType.In, Notes = "Initial stock" },
                new StockTransaction { ProductId = products[1].Id, WarehouseId = warehouses[1].Id, Quantity = 4,  Type = TransactionType.Out, Notes = "Sold items" },

                new StockTransaction { ProductId = products[3].Id, WarehouseId = warehouses[1].Id, Quantity = 60, Type = TransactionType.In, Notes = "Initial stock" },
                new StockTransaction { ProductId = products[3].Id, WarehouseId = warehouses[1].Id, Quantity = 10, Type = TransactionType.Out, Notes = "Returned items" },

                new StockTransaction { ProductId = products[4].Id, WarehouseId = warehouses[1].Id, Quantity = 25, Type = TransactionType.In, Notes = "Initial stock" },

                // =========================
                // Outlet Warehouse
                // =========================
                new StockTransaction { ProductId = products[4].Id, WarehouseId = warehouses[2].Id, Quantity = 20, Type = TransactionType.In, Notes = "Initial stock" },
                new StockTransaction { ProductId = products[4].Id, WarehouseId = warehouses[2].Id, Quantity = 5,  Type = TransactionType.Out, Notes = "Damaged items" },

                new StockTransaction { ProductId = products[2].Id, WarehouseId = warehouses[2].Id, Quantity = 40, Type = TransactionType.In, Notes = "Initial stock" },
                new StockTransaction { ProductId = products[2].Id, WarehouseId = warehouses[2].Id, Quantity = 8,  Type = TransactionType.Out, Notes = "Sold items" },

                new StockTransaction { ProductId = products[3].Id, WarehouseId = warehouses[2].Id, Quantity = 30, Type = TransactionType.In, Notes = "Initial stock" },

                // =========================
                // Extra Movements (Real-life noise)
                // =========================
                new StockTransaction { ProductId = products[0].Id, WarehouseId = warehouses[0].Id, Quantity = 3,  Type = TransactionType.Out, Notes = "Flash sale" },
                new StockTransaction { ProductId = products[2].Id, WarehouseId = warehouses[1].Id, Quantity = 20, Type = TransactionType.In, Notes = "Supplier shipment" },
                new StockTransaction { ProductId = products[2].Id, WarehouseId = warehouses[1].Id, Quantity = 5,  Type = TransactionType.Out, Notes = "Sample units" },
                new StockTransaction { ProductId = products[1].Id, WarehouseId = warehouses[2].Id, Quantity = 10, Type = TransactionType.In, Notes = "New arrival" },
                new StockTransaction { ProductId = products[1].Id, WarehouseId = warehouses[2].Id, Quantity = 2,  Type = TransactionType.Out, Notes = "Sold items" }
            };

            context.StockTransactions.AddRange(transactions);
            context.SaveChanges();

            // =========================
            // WarehouseStock (DERIVED DATA)
            // =========================
            var warehouseStocks = context.StockTransactions
                .GroupBy(t => new { t.ProductId, t.WarehouseId })
                .Select(g => new WarehouseStock
                {
                    ProductId = g.Key.ProductId,
                    WarehouseId = g.Key.WarehouseId,
                    Quantity = g.Sum(t => t.Type == TransactionType.In ? t.Quantity : -t.Quantity)
                })
                .Where(ws => ws.Quantity > 0)
                .ToList();

            context.WarehouseStocks.AddRange(warehouseStocks);
            context.SaveChanges();
        }
    }
}
