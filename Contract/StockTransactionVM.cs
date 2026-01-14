namespace Contract
{
    public class StockTransactionVM
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string TransactionType { get; set; }
        public string ProductName { get; set; }
        public string WarehouseName { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Notes { get; set; }
        
    }
}