using System.ComponentModel.DataAnnotations;

namespace HMS.Core.Dtos.Request.Rooms
{
    public class RoomCreateRequest
    {
        [Required] public int RoomTypeId { get; set; }
        [Required, MaxLength(10)] public string RoomNumber { get; set; } = string.Empty;
        [Required, Range(1, 100)] public int Floor { get; set; }
        public string? Description { get; set; }
    }
}
