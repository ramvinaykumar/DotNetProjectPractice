using Library.Management.CodeFirst.API.Models;

namespace Library.Management.CodeFirst.API.DTOs
{
    public class BookDTO
    {
        public int BookId { get; set; }

        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public BookType Type { get; set; }
    }
}
