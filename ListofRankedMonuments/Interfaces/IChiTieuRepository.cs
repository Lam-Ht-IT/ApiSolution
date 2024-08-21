using System.Collections.Generic;
using System.Threading.Tasks;
using QUANLYVANHOA.Models;

namespace QUANLYVANHOA.Interfaces
{
    public interface IChiTieuRepository
    {
        Task<(IEnumerable<ChiTieu>, int)> GetAll(string? name, int pageNumber, int pageSize);
        Task<ChiTieu> GetByID(int id);
        Task<int> Insert(ChiTieu chiTieu);
        Task<int> Update(ChiTieu chiTieu);
        Task<int> Delete(int id);
    }
}
