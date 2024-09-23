using System.Collections.Generic;
using System.Threading.Tasks;
using QUANLYVANHOA.Controllers;

namespace QUANLYVANHOA.Interfaces
{
    public interface ISysUserRepository
    {
        Task<(IEnumerable<SysUser>, int)> GetAll(string? userName, int pageNumber, int pageSize);
        Task<SysUser> GetByID(int userId);
        Task<int> Create(SysUserInsertModel user);
        Task<int> Update(SysUserUpdateModel user);
        Task<int> UpdateRefreshToken(UpdateRefreshTokenModel obj);
        Task<int> Delete(int userId);
        Task<SysUser> GetByRefreshToken(string refreshToken);
        Task<SysUser> VerifyLogin(string userName, string password);

    }
}