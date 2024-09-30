using QUANLYVANHOA.Controllers;

namespace QUANLYVANHOA.Interfaces
{
    public interface ICtgDonViTinhRepository
    {
        Task<(IEnumerable<CtgDonViTinh>, int)> GetAll(string? name, int pageNumber, int pageSize);
        Task<CtgDonViTinh> GetByID(int id);
        Task<int> Insert(CtgDonViTinhModelInsert obj);
        Task<int> Update(CtgDonViTinhModelUpdate obj);
        Task<int> Delete(int id);

    }
}
