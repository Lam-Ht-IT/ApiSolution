using QUANLYVANHOA.Models;
using QUANLYVANHOA.Controllers;

namespace QUANLYVANHOA.Interfaces
{
    public interface ICtgTieuChiRepository
    {
        public Task<(IEnumerable<CtgTieuChi>, int)> GetAll(string? name/*, int pageNumber, int pageSize*/);
        public Task<CtgTieuChi> GetByID(int id);
        public Task<int> Insert(CtgTieuChiModelInsert obj);
        public Task<int> Update(CtgTieuChiModelUpdate obj);
        public Task<int> Delete(int id);

    }
}
