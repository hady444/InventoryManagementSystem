
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class StockTransaction
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
        public TransactionType Type { get; set; }

        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
        [Range(0,double.MaxValue)]
        public int Quantity { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        public string? Notes { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
}
