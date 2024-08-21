using QUANLYVANHOA.Controllers;

namespace QUANLYVANHOA.Interfaces
{
    public interface IDiTichXepHangRepository
    {
        public Task<(IEnumerable<DiTichXepHang>,int)> GetAll(string? name, int pageNumber, int pageSize);
        public Task<DiTichXepHang> GetByID(int id);
        public Task<int> Insert (DiTichXepHang obj);
        public Task<int> Update(DiTichXepHang obj);
        public Task<int> Delete(int id);
    }
}
