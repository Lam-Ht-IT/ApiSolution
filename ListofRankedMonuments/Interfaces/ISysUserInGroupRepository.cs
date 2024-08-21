using QUANLYVANHOA.Models;

namespace QUANLYVANHOA.Interfaces
{
    public interface ISysUserInGroupRepository
    {
        Task<IEnumerable<SysUserInGroup>> GetAll();
        Task<IEnumerable<SysUserInGroup>> GetByGroupID(int groupID);
        Task<IEnumerable<SysUserInGroup>> GetByUserID(int userID);
        Task<SysUserInGroup> GetByID(int userInGroupID);
        Task<int> Create(SysUserInGroup userInGroup);
        Task<int> Update(SysUserInGroup userInGroup);
        Task<int> Delete(int userInGroupID);
    }
}
