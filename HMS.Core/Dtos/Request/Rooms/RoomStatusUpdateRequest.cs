using System.ComponentModel.DataAnnotations;

namespace HMS.Core.Dtos.Request.Rooms
{
    public class RoomStatusUpdateRequest
    {
        [Required] public string Status { get; set; } = string.Empty;
        public DateTime? LastCleaned { get; set; }
    }
}
