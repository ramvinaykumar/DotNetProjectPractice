using System.ComponentModel.DataAnnotations;

namespace HMS.Core.Dtos.Request.Rooms
{
    public class RoomUpdateRequest : RoomCreateRequest
    {
        [Required] public string Status { get; set; } = "Available";
        public bool? IsActive { get; set; }
    }
}
