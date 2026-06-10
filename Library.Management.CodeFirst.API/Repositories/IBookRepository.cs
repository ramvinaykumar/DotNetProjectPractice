using Library.Management.CodeFirst.API.DTOs;

namespace Library.Management.CodeFirst.API.Repositories
{
    /// <summary>
    /// Interface for book repository providing CRUD operations.
    /// </summary>
    public interface IBookRepository
    {
        /// <summary>
        /// Retrieves all books from the repository.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of BookDTO.</returns>
        Task<IEnumerable<BookDTO>> GetAllBooksAsync();

        /// <summary>
        /// Retrieves a single book by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the book.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the BookDTO.</returns>
        Task<BookDTO> GetBookByIdAsync(int bookId);

        /// <summary>
        /// Adds a new book to the repository.
        /// </summary>
        /// <param name="book">The data transfer object containing the details of the book to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<BookDTO> AddBookAsync(BookDTO bookDto);

        /// <summary>
        /// Updates an existing book in the repository.
        /// </summary>
        /// <param name="book">The data transfer object containing the updated details of the book.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateAsync(BookDTO book);

        /// <summary>
        /// Deletes a book from the repository by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the book to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<bool> RemoveBookAsync(int bookId);
    }
}
