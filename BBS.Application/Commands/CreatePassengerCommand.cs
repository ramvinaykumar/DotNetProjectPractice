using BBS.Application.DTOs.Passenger;
using MediatR;

namespace BBS.Application.Commands
{
    /// <summary>
    /// Represents a command to create a new passenger with personal and contact information.
    /// </summary>
    public class CreatePassengerCommand : IRequest<PassengerResponse>
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }
    }
}
