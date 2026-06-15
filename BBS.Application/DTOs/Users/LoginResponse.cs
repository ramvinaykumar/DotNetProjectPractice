namespace BBS.Application.DTOs.Users
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime ExpiryDate { get; set; }
    }
}
