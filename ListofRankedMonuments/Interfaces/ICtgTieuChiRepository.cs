using QUANLYVANHOA.Models;
using QUANLYVANHOA.Controllers;

namespace QUANLYVANHOA.Interfaces
{
    public interface ICtgTieuChiRepository
    {
        public Task<(IEnumerable<CtgTieuChi>, int)> GetAll(string? name, int pageNumber, int pageSize);
        public Task<CtgTieuChi> GetByID(int id);
        public Task<int> Insert(CtgTieuChi obj);
        public Task<int> Update(CtgTieuChi obj);
        public Task<int> Delete(int id);

    }
}
