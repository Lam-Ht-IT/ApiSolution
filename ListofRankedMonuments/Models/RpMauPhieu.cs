using System.Text.Json.Serialization;

namespace QUANLYVANHOA.Models
{
    public class RpMauPhieu
    {
        [JsonPropertyName("MauPhieuId")]
        public int MauPhieuId { get; set;}

        [JsonPropertyName("TenMauPhieu")]
        public string TenMauPhieu { get; set; }

        [JsonPropertyName("MaMauPhieu")]
        public string MaMauPhieu { get; set; }

        [JsonPropertyName("LoaiMauPhieuId")]
        public int LoaiMauPhieuId { get; set; }

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
        [JsonPropertyName("MauPhieuId")]
        public int MauPhieuId { get; set; }

        [JsonPropertyName("TenMauPhieu")]
        public string TenMauPhieu { get; set; }

        [JsonPropertyName("MaMauPhieu")]
        public string MaMauPhieu { get; set; }

        [JsonPropertyName("LoaiMauPhieuId")]
        public int LoaiMauPhieuId { get; set; }

        [JsonPropertyName("NguoiTao")]
        public string NguoiTao { get; set; }
    }

    public class RpMauPhieuUpdateModel
    {
        [JsonPropertyName("MauPhieuId")]
        public int MauPhieuId { get; set; }

        [JsonPropertyName("TenMauPhieu")]
        public string TenMauPhieu { get; set; }

        [JsonPropertyName("MaMauPhieu")]
        public string MaMauPhieu { get; set; }

        [JsonPropertyName("LoaiMauPhieuId")]
        public int LoaiMauPhieuId { get; set; }

        [JsonPropertyName("NguoiTao")]
        public string NguoiTao { get; set; }
    }

    public class RpMauPhieuDeleteModel
    {
        [JsonPropertyName("MauPhieuId")]
        public int MauPhieuId { get; set; }
    }
}