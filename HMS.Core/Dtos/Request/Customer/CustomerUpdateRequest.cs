namespace HMS.Core.Dtos.Request.Customer
{
    public class CustomerUpdateRequest : CustomerCreateRequest
    {
        public bool? IsVIP { get; set; }
    }
}
