using QUANLYVANHOA.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QUANLYVANHOA.Interfaces
{
    public interface IRpMauPhieuRepository
    {
        Task<(IEnumerable<RpMauPhieu>, int)> GetAll(string? name);
        Task<RpMauPhieu> GetByID(int id);
        Task<int> Insert(RpMauPhieuInsertModel obj);
        Task<int> Update(RpMauPhieuUpdateModel obj);
        Task<int> Delete(int id);
        Task<IEnumerable<CtgChiTieu>> GetChiTieusByMauPhieuID(int mauPhieuId);
        Task<IEnumerable<CtgTieuChi>> GetTieuChisByMauPhieuID(int mauPhieuId);
        Task<IEnumerable<RpChiTietMauPhieu>> GetChiTietMauPhieuByID(int mauPhieuId);

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