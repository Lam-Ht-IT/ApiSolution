using QUANLYVANHOA.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QUANLYVANHOA.Interfaces
{
    public interface IRpMauPhieuRepository
    {
        Task<(IEnumerable<RpMauPhieu>, int)> GetAll(string? name);
        Task<RpMauPhieu> GetByID(int id);
        Task<int> Insert(RpMauPhieu obj);
        Task<int> Update(RpMauPhieu obj);
        Task<int> Delete(int id);
    }
}