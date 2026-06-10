namespace Library.Management.CodeFirst.API.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public BookType Type { get; set; }
        public bool IsAvailable { get; set; } = true;
    }

    public enum BookType
    {
        Physical,
        EBook,
        AudioBook
    }
}
