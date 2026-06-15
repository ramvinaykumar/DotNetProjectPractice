namespace BBS.Domain.Entities
{
    public class Route
    {
        public int RouteId { get; set; }

        public string SourceCity { get; set; } = string.Empty;

        public string DestinationCity { get; set; } = string.Empty;

        public decimal DistanceKM { get; set; }
    }
}
