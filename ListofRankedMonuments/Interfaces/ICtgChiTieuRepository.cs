using System.Collections.Generic;
using System.Threading.Tasks;
using QUANLYVANHOA.Models;

namespace QUANLYVANHOA.Interfaces
{
    public interface ICtgChiTieuRepository
    {
        Task<(IEnumerable<CtgChiTieu>, int)> GetAll(string? name, int pageNumber, int pageSize);
        Task<CtgChiTieu> GetByID(int id);
        Task<int> Insert(CtgChiTieu chiTieu);
        Task<int> Update(CtgChiTieu chiTieu);
        Task<int> Delete(int id);
    }
}
