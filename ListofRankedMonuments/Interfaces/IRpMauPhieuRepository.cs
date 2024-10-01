using QUANLYVANHOA.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QUANLYVANHOA.Interfaces
{
    public interface IRpMauPhieuRepository
    {
        Task<(IEnumerable<RpMauPhieu>, int)> GetAll(string? name);
        Task<RpMauPhieu> GetByID(int id);
        Task<int> Insert(RpMauPhieuInsertModel obj);
        Task<int> Update(RpMauPhieuUpdateModel obj);
        Task<int> Delete(int id);
    }
}