namespace BBS.Domain.Entities
{
    public class Bus
    {
        public int BusId { get; set; }

        public string BusNumber { get; set; } = string.Empty;

        public string BusName { get; set; } = string.Empty;

        public int TotalSeats { get; set; }
    }
}
