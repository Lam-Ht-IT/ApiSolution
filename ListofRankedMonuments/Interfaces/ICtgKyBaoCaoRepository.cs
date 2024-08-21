using QUANLYVANHOA.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QUANLYVANHOA.Interfaces
{
    public interface ICtgKyBaoCaoRepository
    {
        Task<(IEnumerable<CtgKyBaoCao>, int)> GetAll(string? name, int pageNumber, int pageSize);
        Task<CtgKyBaoCao> GetByID(int id);
        Task<int> Insert(CtgKyBaoCao obj);
        Task<int> Update(CtgKyBaoCao obj);
        Task<int> Delete(int id);
    }
}
