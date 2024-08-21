using System.Collections.Generic;
using System.Threading.Tasks;
using QUANLYVANHOA.Controllers;
using QUANLYVANHOA.Controllers;

namespace QUANLYVANHOA.Interfaces
{
    public interface IUserRepository
    {
        Task<(IEnumerable<User>, int)> GetAll(string? userName, int pageNumber, int pageSize);
        Task<User> GetByID(int userId);
        Task<int> Insert(User user);
        Task<int> Update(User user);
        Task<int> Delete(int userId);
        Task<User> VerifyLoginAsync(string userName, string password);
    }
}