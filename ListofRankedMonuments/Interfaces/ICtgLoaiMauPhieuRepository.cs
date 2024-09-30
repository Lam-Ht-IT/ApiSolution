using QUANLYVANHOA.Controllers;

namespace QUANLYVANHOA.Interfaces
{
    public interface ICtgLoaiMauPhieuRepository
    {
        Task<(IEnumerable<CtgLoaiMauPhieu>, int)> GetAll(string? name, int pageNumber, int pageSize);
        Task<CtgLoaiMauPhieu> GetByID(int id);
        Task<int> Insert(CtgLoaiMauPhieuModelInsert obj);
        Task<int> Update(CtgLoaiMauPhieuModelUpdate obj);
        Task<int> Delete(int id);
    }
}
