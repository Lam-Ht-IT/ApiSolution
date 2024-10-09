using QUANLYVANHOA.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace QUANLYVANHOA.Interfaces
{
    public interface IRpChiTietMauPhieu
    {
        Task<(IEnumerable<RpChiTietMauPhieu>, int)> GetAll(string? name);
        Task<RpChiTietMauPhieu> GetByID(int id);
        Task<int> Insert(RpChiTietMauPhieuInsertModel obj);
        Task<int> Update(RpChiTietMauPhieuUpdateModel obj);
        Task<int> Delete(int id);
    }
}
