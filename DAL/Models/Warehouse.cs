
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Warehouse
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Location { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public ICollection<StockTransaction> StockTransactions { get; set; }
    }
}
