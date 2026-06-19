using BBS.Application.DTOs.Passenger;
using MediatR;

namespace BBS.Application.Commands
{
    /// <summary>
    /// Represents a command to update passenger details.
    /// </summary>
    public class UpdatePassengerCommand : IRequest<PassengerResponse>
    {
        public int PassengerId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }
    }
}
