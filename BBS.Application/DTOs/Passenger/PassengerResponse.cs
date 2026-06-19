namespace BBS.Application.DTOs.Passenger
{
    public class PassengerResponse
    {
        public int PassengerId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public string DateOfBirth { get; set; } = string.Empty;

        public string Active { get; set; } = string.Empty;
    }
}
