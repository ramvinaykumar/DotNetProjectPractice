namespace BBS.Domain.Entities
{
    public class RefreshToken
    {
        public int RefreshTokenId { get; set; }

        public int UserId { get; set; }

        public string TokenHash { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool IsRevoked { get; set; }
    }
}
