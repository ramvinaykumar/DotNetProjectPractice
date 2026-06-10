namespace Library.Management.CodeFirst.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public int BorrowedBooksCount { get; set; } = 0;
    }

    public enum UserRole
    {
        Member,
        Librarian
    }
}
