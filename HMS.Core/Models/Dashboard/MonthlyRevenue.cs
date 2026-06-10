namespace HMS.Core.Models.Dashboard
{
    public class MonthlyRevenue
    {
        public int MonthNumber { get; set; }
        public string MonthName { get; set; } = string.Empty;
        public int TransactionCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AvgTransactionValue { get; set; }
    }
}
