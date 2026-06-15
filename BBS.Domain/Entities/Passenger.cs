namespace BBS.Domain.Entities
{
    public class Passenger
    {
        public int PassengerId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string MobileNo { get; set; } = string.Empty;
    }
}
