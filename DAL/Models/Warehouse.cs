
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Warehouse
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(200, ErrorMessage = "Shorten the location max 200 letter")]
        public string? Location { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public ICollection<StockTransaction> StockTransactions { get; set; }
    }
}
