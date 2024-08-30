using System.Collections.Generic;
using System.Threading.Tasks;
using QUANLYVANHOA.Controllers;
using QUANLYVANHOA.Controllers;

namespace QUANLYVANHOA.Interfaces
{
    public interface ISysUserRepository
    {
        Task<(IEnumerable<SysUser>, int)> GetAll(string? userName, int pageNumber, int pageSize);
        Task<SysUser> GetByID(int userId);
        Task<int> Create(SysUser user);
        Task<int> Update(SysUser user);
        Task<int> Delete(int userId);
        Task<SysUser> GetByRefreshToken(string refreshToken);
        Task<SysUser> VerifyLogin(string userName, string password);
    }
}