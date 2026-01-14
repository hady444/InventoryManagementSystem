using DAL.Models;

namespace BLL.ViewModel
{
    public class CreateTransactionVM
    {
        public int ProductId { get; set; }
        public TransactionType Type { get; set; }

        public int WarehouseId { get; set; }
        public int Quantity { get; set; }

        public string? Notes { get; set; }
        }
}
