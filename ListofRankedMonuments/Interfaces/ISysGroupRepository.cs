using QUANLYVANHOA.Models;

namespace QUANLYVANHOA.Interfaces
{
    public interface ISysGroupRepository
    {
        Task<(IEnumerable<SysGroup>, int)> GetAll(string? groupName, int pageNumber, int pageSize);
        Task<SysGroup> GetByID(int groupID);
        Task<int> Create(SysGroup group);
        Task<int> Update(SysGroup group);
        Task<int> Delete(int groupID);

    }
}
