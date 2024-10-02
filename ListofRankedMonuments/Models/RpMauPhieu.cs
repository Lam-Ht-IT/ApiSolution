using System.Text.Json.Serialization;

namespace QUANLYVANHOA.Models
{
    public class RpMauPhieu
    {
        [JsonPropertyName("MauPhieuID")]
        public int MauPhieuID { get; set;}

        [JsonPropertyName("TenMauPhieu")]
        public string TenMauPhieu { get; set; }

        [JsonPropertyName("MaMauPhieu")]
        public string MaMauPhieu { get; set; }

        [JsonPropertyName("LoaiMauPhieuID")]
        public int LoaiMauPhieuID { get; set; }

        public List<CtgChiTieu> ChiTieus { get; set; }  // Chứa danh sách các chỉ tiêu
        public List<CtgTieuChi> TieuChis { get; set; }  // Chứa danh sách các tiêu chí
        public List<RpChiTietMauPhieu> ChiTietMauPhieus { get; set; }  // Chi tiết mẫu phiếu với gộp cột

        [JsonPropertyName("NgayTao")]
        public string? NgayTao { get; set; }

        [JsonPropertyName("NguoiTao")]
        public string NguoiTao { get; set; }
    }

    public class RpMauPhieuInsertModel
    {
        [JsonPropertyName("TenMauPhieu")]
        public string TenMauPhieu { get; set; }

        [JsonPropertyName("MaMauPhieu")]
        public string MaMauPhieu { get; set; }

        [JsonPropertyName("LoaiMauPhieuID")]
        public int LoaiMauPhieuID { get; set; }
        public List<CtgChiTieu> ChiTieus { get; set; }  // Chứa danh sách các chỉ tiêu
        public List<CtgTieuChi> TieuChis { get; set; }  // Chứa danh sách các tiêu chí
        public List<RpChiTietMauPhieuInsertModel> ChiTietMauPhieus { get; set; }  // Chi tiết mẫu phiếu với gộp cột


        [JsonPropertyName("NguoiTao")]
        public string? NguoiTao { get; set; }
    }

    public class RpMauPhieuUpdateModel
    {
        [JsonPropertyName("MauPhieuID")]
        public int MauPhieuID { get; set; }

        [JsonPropertyName("TenMauPhieu")]
        public string TenMauPhieu { get; set; }

        [JsonPropertyName("MaMauPhieu")]
        public string MaMauPhieu { get; set; }

        [JsonPropertyName("LoaiMauPhieuID")]
        public int LoaiMauPhieuID { get; set; }
        public List<CtgChiTieu> ChiTieus { get; set; }  // Chứa danh sách các chỉ tiêu
        public List<CtgTieuChi> TieuChis { get; set; }  // Chứa danh sách các tiêu chí
        public List<RpChiTietMauPhieu> ChiTietMauPhieus { get; set; }  // Chi tiết mẫu phiếu với gộp cột


        [JsonPropertyName("NguoiTao")]
        public string NguoiTao { get; set; }
    }
}