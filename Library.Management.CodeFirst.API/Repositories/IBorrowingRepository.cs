using Library.Management.CodeFirst.API.DTOs;
using Library.Management.CodeFirst.API.Models;

namespace Library.Management.CodeFirst.API.Repositories
{
    public interface IBorrowingRepository
    {
        Task<BorrowingRecord> BorrowBookAsync(BorrowRequestDTO requestDto);
        Task<bool> ReturnBookAsync(int borrowId);
        Task<IEnumerable<BorrowingRecord>> GetUserBorrowingHistory(int userId);
    }
}
