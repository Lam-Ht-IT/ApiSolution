using QUANLYVANHOA.Models;
using QUANLYVANHOA.Controllers;

namespace QUANLYVANHOA.Interfaces
{
    public interface ITieuChiRepository
    {
        public Task<(IEnumerable<TieuChi>, int)> GetAll(string? name, int pageNumber, int pageSize);
        public Task<TieuChi> GetByID(int id);
        public Task<int> Insert(TieuChi obj);
        public Task<int> Update(TieuChi obj);
        public Task<int> Delete(int id);

    }
}
