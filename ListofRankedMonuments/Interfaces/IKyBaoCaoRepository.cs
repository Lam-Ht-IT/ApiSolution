using QUANLYVANHOA.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QUANLYVANHOA.Interfaces
{
    public interface IKyBaoCaoRepository
    {
        Task<(IEnumerable<KyBaoCao>, int)> GetAll(string? name, int pageNumber, int pageSize);
        Task<KyBaoCao> GetByID(int id);
        Task<int> Insert(KyBaoCao obj);
        Task<int> Update(KyBaoCao obj);
        Task<int> Delete(int id);
    }
}
