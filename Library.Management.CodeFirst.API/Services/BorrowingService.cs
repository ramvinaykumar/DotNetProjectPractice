using Library.Management.CodeFirst.API.DTOs;
using Library.Management.CodeFirst.API.Models;
using Library.Management.CodeFirst.API.Repositories;

namespace Library.Management.CodeFirst.API.Services
{
    public class BorrowingService : IBorrowingRepository
    {
        private readonly IBorrowingRepository _borrowingRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;

        public BorrowingService(IBorrowingRepository borrowingRepository, IBookRepository bookRepository, IUserRepository userRepository)
        {
            _borrowingRepository = borrowingRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }

        //public async Task<bool> BorrowBookAsync(BorrowRequestDTO request)
        //{
        //    var book = await _bookRepository.GetByIdAsync(request.BookId);
        //    var user = await _userRepository.GetByIdAsync(request.UserId);

        //    if (book == null || user == null || !book.IsAvailable || user.BorrowedBooksCount >= 5)
        //        return false;

        //    await _borrowingRepository.BorrowBookAsync(request);
        //    return true;
        //}

        

        public Task<IEnumerable<BorrowingRecord>> GetUserBorrowingHistory(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ReturnBookAsync(int borrowId)
        {
            throw new NotImplementedException();
        }

        Task<BorrowingRecord> IBorrowingRepository.BorrowBookAsync(BorrowRequestDTO requestDto)
        {
            throw new NotImplementedException();
        }
    }
}
