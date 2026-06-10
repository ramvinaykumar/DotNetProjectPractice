namespace HMS.Core.Dtos.Request.Customer
{
    public class CustomerQueryRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? SearchTerm { get; set; }
        public bool? IsVIP { get; set; }
        public bool? IsActive { get; set; } = true;
    }
}
