using QUANLYVANHOA.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QUANLYVANHOA.Interfaces
{
    public interface IRpMauPhieuRepository
    {
        Task<(IEnumerable<RpMauPhieu>, int)> GetAll(string? name, int pageNumber, int pageSize);
        Task<RpMauPhieu> GetByID(int id);
        Task<int> InsertMauPhieu(RpMauPhieuInsertModel obj);
        Task<int> Update(RpMauPhieuUpdateModel obj);
        Task<int> Delete(int id);
        Task<List<CtgChiTieu>> GetChiTieusHierarchyByMauPhieuID(int mauPhieuId);
        Task<List<CtgTieuChi>> GetTieuChisHierarchyByMauPhieuID(int mauPhieuId);
        Task<List<RpChiTietMauPhieu>> GetChiTietMauPhieuByMauPhieuID(int mauPhieuId);

        // Thêm và xóa tiêu chí
        Task AddTieuChiMauPhieu(int mauPhieuId, int tieuChiId);
        Task DeleteTieuChiMauPhieu(int mauPhieuId, int tieuChiId);

        // Thêm và xóa chỉ tiêu
        Task AddChiTieuMauPhieu(int mauPhieuId, int chiTieuId);
        Task DeleteChiTieuMauPhieu(int mauPhieuId, int chiTieuId);

        // Thêm, sửa, xóa ChiTietMauPhieu
        Task AddChiTietMauPhieu(RpChiTietMauPhieu chiTietMauPhieu);
        Task UpdateChiTietMauPhieu(RpChiTietMauPhieuUpdateModel chiTietMauPhieu);
        Task DeleteChiTietMauPhieu(int chiTietMauPhieuId);
    }
}