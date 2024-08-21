using QUANLYVANHOA.Controllers;

namespace QUANLYVANHOA.Interfaces
{
    public interface ICtgDiTichXepHangRepository
    {
        Task<(IEnumerable<CtgDiTichXepHang>,int)> GetAll(string? name, int pageNumber, int pageSize);
        Task<CtgDiTichXepHang> GetByID(int id);
        Task<int> Insert (CtgDiTichXepHangModelInsert  obj);
        Task<int> Update(CtgDiTichXepHangModelUpdate obj);
        Task<int> Delete(int id);
    }
}
