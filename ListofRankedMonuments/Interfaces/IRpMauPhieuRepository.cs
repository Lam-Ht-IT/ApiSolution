using QUANLYVANHOA.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QUANLYVANHOA.Interfaces
{
    public interface IRpMauPhieuRepository
    {
        Task<(IEnumerable<RpMauPhieu>, int)> GetAllMauPhieu(string? name, int pageNumber, int pageSize);
        Task<RpMauPhieu> GetMauPhieuByID(int id);
        Task<int> CreateMauPhieu(RpMauPhieuInsertModel obj);
        Task<int> UpdateMauPhieu(RpMauPhieuUpdateModel obj);
        Task<int> DeleteMauPhieu(int id);
    }
}