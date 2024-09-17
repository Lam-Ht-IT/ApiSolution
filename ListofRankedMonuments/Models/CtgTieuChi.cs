using System.Text.Json.Serialization;

namespace QUANLYVANHOA.Models
{
    public class CtgTieuChi
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

        [JsonPropertyName("CapDo")]
        public int? CapDo { get; set; }

        public List<CtgTieuChi>? Children { get; set; } = new List<CtgTieuChi>();

    }

    public class CtgTieuChiModelInsert
    {
        [JsonPropertyName("MaTieuChi")]
        public string? MaTieuChi { get; set; }

        [JsonPropertyName("TenTieuChi")]
        public string TenTieuChi { get; set; }

        [JsonPropertyName("TieuChiChaID")]
        public int? TieuChiChaID { get; set; }

        [JsonPropertyName("KieuDuLieuCot")]
        public int? KieuDuLieuCot { get; set; }

        [JsonPropertyName("LoaiTieuChi")]
        public int? LoaiTieuChi { get; set; }

    }

    public class CtgTieuChiModelUpdate
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

        [JsonPropertyName("LoaiTieuChi")]
        public int? LoaiTieuChi { get; set; }

    }


}
