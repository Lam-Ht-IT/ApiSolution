using System.Collections.Generic;
using System.Threading.Tasks;

namespace QUANLYVANHOA.Interfaces
{
    public interface IRoleRepository
    {
        Task<(IEnumerable<Role>, int)> GetAll(string? roleName, int pageNumber, int pageSize);
        Task<Role> GetByID(int roleId);
        Task<int> Insert(Role role);
        Task<int> Update(Role role);
        Task<int> Delete(int roleId);
    }
}
