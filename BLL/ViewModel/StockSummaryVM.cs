namespace BLL.ViewModel
{
    public class StockSummaryVM
    {
        public int TotalStockIn { get; set; }
        public int TotalStockOut { get; set; }
        public int CurrentStock => TotalStockIn - TotalStockOut;

        public int TotalProducts { get; set; }
        public int TotalWarehouses { get; set; }

        public List<LowStockVM> LowStockProducts { get; set; } = new();
    }

    public class LowStockVM
    {
        public string ProductName { get; set; } = "";
        public string WarehouseName { get; set; } = "";
        public int Quantity { get; set; }
    }
}

