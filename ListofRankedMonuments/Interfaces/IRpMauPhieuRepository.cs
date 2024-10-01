using QUANLYVANHOA.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QUANLYVANHOA.Interfaces
{
    public interface IRpMauPhieuRepository
    {
        Task<(IEnumerable<RpMauPhieu>, int)> GetAllMauPhieu(string? name, int pageNumber, int pageSize);
        Task<RpMauPhieu> GetMauPhieuByID(int id);
        Task<int> InsertMauPhieu(RpMauPhieuInsertModel obj);
        Task<int> UpdateMauPhieu(RpMauPhieuUpdateModel obj);
        Task<int> DeleteMauPhieu(int id);
        Task<List<CtgChiTieu>> GetChiTieusHierarchyByMauPhieuID(int mauPhieuId);
        Task<List<CtgTieuChi>> GetTieuChisHierarchyByMauPhieuID(int mauPhieuId);
        Task<List<RpChiTietMauPhieu>> GetChiTietMauPhieuByMauPhieuID(int mauPhieuId);

        // Thêm và xóa tiêu chí
        Task AddTieuChiBaoCao(int mauPhieuId, int tieuChiId);
        Task DeleteTieuChiBaoCao(int mauPhieuId, int tieuChiId);

        // Thêm và xóa chỉ tiêu
        Task AddChiTieuBaoCao(int mauPhieuId, int chiTieuId);
        Task DeleteChiTieuBaoCao(int mauPhieuId, int chiTieuId);

        // Thêm, sửa, xóa ChiTietMauPhieu
        Task AddChiTietBaoCao(RpChiTietMauPhieu chiTietMauPhieu);
        Task UpdateChiTietBaoCao(RpChiTietMauPhieuUpdateModel chiTietMauPhieu);
        Task DeleteChiTietBaoCao(int chiTietMauPhieuId);
    }
}