using QUANLYVANHOA.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QUANLYVANHOA.Interfaces
{
    public interface ISysFunctionInGroupRepository
    {
        Task<IEnumerable<SysFunctionInGroup>> GetAll();
        Task<IEnumerable<SysFunctionInGroup>> GetByGroupID(int groupID);
        Task<IEnumerable<SysFunctionInGroup>> GetByFunctionID(int functionID);
        Task<SysFunctionInGroup> GetByID(int functionInGroupID);
        Task<int> Create(SysFunctionInGroup functionInGroup);
        Task<int> Update(SysFunctionInGroup functionInGroup);
        Task<int> Delete(int functionInGroupID);
    }
}