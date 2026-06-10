namespace HMS.Core.Dtos.Response
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public StaffResponse Staff { get; set; } = null!;
    }
}
