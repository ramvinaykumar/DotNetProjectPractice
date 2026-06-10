namespace BBS.Domain.Entities
{
    public class Route
    {
        public int RouteId { get; set; }

        public string SourceCity { get; set; }

        public string DestinationCity { get; set; }

        public decimal DistanceKM { get; set; }
    }
}
