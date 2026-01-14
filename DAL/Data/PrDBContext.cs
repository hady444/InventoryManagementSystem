using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data
{
    public class PrDBContext: DbContext
    {
        public PrDBContext(DbContextOptions<PrDBContext> dbContextOptions) : base(dbContextOptions)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<StockTransaction> StockTransactions { get; set; }
        public DbSet<WarehouseStock> WarehouseStocks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasIndex(p => p.SKU).IsUnique();
            modelBuilder.Entity<Warehouse>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<StockTransaction>()
                .HasOne(t => t.Product)
                .WithMany(p => p.StockTransactions)
                .HasForeignKey(t => t.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockTransaction>()
                .HasOne(t => t.Warehouse)
                .WithMany(w => w.StockTransactions)
                .HasForeignKey(t => t.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<StockTransaction>()
                .HasQueryFilter(t => !t.IsDeleted);

            modelBuilder.Entity<Warehouse>()
                .HasQueryFilter(w => !w.IsDeleted);
            modelBuilder.Entity<WarehouseStock>()
                .HasIndex(x => new { x.ProductId, x.WarehouseId })
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
