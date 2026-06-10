using AutoMapper;
using Library.Management.CodeFirst.API.Data;
using Library.Management.CodeFirst.API.DTOs;
using Library.Management.CodeFirst.API.Models;
using Library.Management.CodeFirst.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Management.CodeFirst.API.Services
{
    public class BookService : IBookRepository
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public BookService(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookDTO>> GetAllBooksAsync()
        {
            var books = await _context.Books.ToListAsync();
            return _mapper.Map<List<BookDTO>>(books);
        }

        public async Task<BookDTO> GetBookByIdAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            return _mapper.Map<BookDTO>(book);
        }

        public async Task<BookDTO> AddBookAsync(BookDTO bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return _mapper.Map<BookDTO>(book);
        }

        public async Task UpdateAsync(BookDTO bookDto)
        {
            var book = await _context.Books.FindAsync(bookDto.BookId);
            if (book == null)
            {
                throw new KeyNotFoundException("Book not found");
            }

            _mapper.Map(bookDto, book);
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RemoveBookAsync(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null) return false;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
