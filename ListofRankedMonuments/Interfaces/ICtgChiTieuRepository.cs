using System.Collections.Generic;
using System.Threading.Tasks;
using QUANLYVANHOA.Models;

namespace QUANLYVANHOA.Interfaces
{
    public interface ICtgChiTieuRepository
    {
        Task<(IEnumerable<CtgChiTieu>, int)> GetAll(string? name, int pageNumber, int pageSize);
        Task<CtgChiTieu> GetByID(int id);
        Task<IEnumerable<CtgChiTieu>> GetByLoaiMauPhieuID(int loaiMauPhieuID);
        Task<int> Insert(CtgChiTieuModelInsert chiTieu);
        Task<int> Update(CtgChiTieuModelUpdate chiTieu);
        Task<int> Delete(int id);
    }
}
