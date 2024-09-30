using QUANLYVANHOA.Controllers;

namespace QUANLYVANHOA.Interfaces
{
    public interface ICtgLoaiDiTichRepository
    {
        Task<(IEnumerable<CtgLoaiDiTich>, int)> GetAll(string? name, int pageNumber, int pageSize);
        Task<CtgLoaiDiTich> GetByID(int id);
        Task<int> Insert(CtgLoaiDiTichModelInsert obj);
        Task<int> Update(CtgLoaiDiTichModelUpdate obj);
        Task<int> Delete(int id);
    }
}
