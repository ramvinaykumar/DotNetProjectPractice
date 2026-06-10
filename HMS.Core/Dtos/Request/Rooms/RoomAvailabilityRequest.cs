using System.ComponentModel.DataAnnotations;

namespace HMS.Core.Dtos.Request.Rooms
{
    public class RoomAvailabilityRequest
    {
        [Required] public DateTime CheckInDate { get; set; }
        [Required] public DateTime CheckOutDate { get; set; }
        public int? RoomTypeId { get; set; }
        public int? Adults { get; set; }
        public int? Children { get; set; }
    }
}
