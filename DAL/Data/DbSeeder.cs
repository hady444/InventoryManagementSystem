using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

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
                Notes = "Initial stock"
            },
            new StockTransaction
            {
                ProductId = product2.Id,
                WarehouseId = warehouse1.Id,
                Quantity = 50,
                Notes = "Initial stock"
            }
        };

            context.StockTransactions.AddRange(transactions);
            context.SaveChanges();

            Console.WriteLine("\n=== All Products ===");
            foreach (var p in context.Products.IgnoreQueryFilters())
            {
                Console.WriteLine($"{p.Id} | {p.Name} | {p.SKU}");
            }

            Console.WriteLine("\n=== All Warehouses ===");
            foreach (var w in context.Warehouses.IgnoreQueryFilters())
            {
                Console.WriteLine($"{w.Id} | {w.Name}");
            }

            Console.WriteLine("\n=== Stock Transactions ===");
            foreach (var t in context.StockTransactions)
            {
                Console.WriteLine($"Product:{t.ProductId}, Warehouse:{t.WarehouseId}, Qty:{t.Quantity}");
            }
        }
    }

}
