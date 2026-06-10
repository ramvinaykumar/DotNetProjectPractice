using System.ComponentModel.DataAnnotations;

namespace HMS.Core.Dtos.Request.Rooms
{
    public class RoomTypeUpsertRequest
    {
        public int? RoomTypeId { get; set; }
        [Required, MaxLength(100)] public string TypeName { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required, Range(1, 100000)] public decimal BasePrice { get; set; }
        [Required, Range(1, 20)] public int MaxOccupancy { get; set; }
        public string? AmenitiesList { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
