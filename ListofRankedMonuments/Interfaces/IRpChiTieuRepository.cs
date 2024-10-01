using QUANLYVANHOA.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QUANLYVANHOA.Interfaces
{
    public interface IRpChiTieuRepository
    {
        Task<(IEnumerable<RpChiTieu>, int)> GetAll();
        Task<RpChiTieu> GetByID(int id);
        Task<int> Insert(RpChiTieuInsertModel obj);
        Task<int> Update(RpChiTieuUpdateModel obj);
        Task<int> Delete(int id);
    }
}
