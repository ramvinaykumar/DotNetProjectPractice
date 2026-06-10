namespace HMS.Core.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public int RoomTypeId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public int Floor { get; set; }
        public string Status { get; set; } = "Available";
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastCleaned { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        // From join
        public string? TypeName { get; set; }
        public decimal BasePrice { get; set; }
        public int MaxOccupancy { get; set; }
        public string? AmenitiesList { get; set; }
        public string? ImageUrl { get; set; }
        // Computed
        public decimal EstimatedTotal { get; set; }
    }
}
