using System.Text.Json.Serialization;

namespace QUANLYVANHOA.Models
{
    public class TieuChi
    {
        [JsonPropertyName("TieuChiID")]
        public int TieuChiID { get; set; }

        [JsonPropertyName("MaTieuChi")]
        public string? MaTieuChi { get; set; }

        [JsonPropertyName("TenTieuChi")]
        public string TenTieuChi { get; set; }

        [JsonPropertyName("TieuChiChaID")]
        public int? TieuChiChaID { get; set; }

        [JsonPropertyName("GhiChu")]
        public string GhiChu { get; set; }

        [JsonPropertyName("KieuDuLieuCot")]
        public int? KieuDuLieuCot { get; set; }

        [JsonPropertyName("TrangThai")]
        public bool? TrangThai { get; set; }

        [JsonPropertyName("LoaiTieuChi")]
        public int? LoaiTieuChi { get; set; }
        public List<TieuChi>? Children { get; set; } = new List<TieuChi>();

    }

    public class TieuChiModel 
    {
        public int TieuChiID { get; set; }

        [JsonPropertyName("MaTieuChi")]
        public string? MaTieuChi { get; set; }

        [JsonPropertyName("TenTieuChi")]
        public string TenTieuChi { get; set; }

        [JsonPropertyName("TieuChiChaID")]
        public int? TieuChiChaID { get; set; }

        [JsonPropertyName("GhiChu")]
        public string GhiChu { get; set; }

        [JsonPropertyName("KieuDuLieuCot")]
        public int? KieuDuLieuCot { get; set; }

        [JsonPropertyName("TrangThai")]
        public bool? TrangThai { get; set; }

        [JsonPropertyName("LoaiTieuChi")]
        public int? LoaiTieuChi { get; set; }
        public List<TieuChi>? Children { get; set; } = new List<TieuChi>();

    }

}
