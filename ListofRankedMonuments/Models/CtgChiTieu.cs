using System.Text.Json.Serialization;

namespace QUANLYVANHOA.Models
{
    public class CtgChiTieu
    {
        [JsonPropertyName("ChiTieuID")]
        public int ChiTieuID { get; set; }

        [JsonPropertyName("MaChiTieu")]
        public string MaChiTieu { get; set; }

        [JsonPropertyName("TenChiTieu")]
        public string TenChiTieu { get; set; }

        [JsonPropertyName("ChiTieuChaID")]
        public int ChiTieuChaID { get; set; }

        [JsonPropertyName("GhiChu")]
        public string? GhiChu { get; set; }

        [JsonPropertyName("TrangThai")]
        public bool TrangThai { get; set; }

        [JsonPropertyName("LoaiMauPhieuID")]
        public int LoaiMauPhieuID { get; set; }
        public List<CtgChiTieu>? Children { get; set; } = new List<CtgChiTieu>();
    }

    public class CtgChiTieuInsertModel
    {
        [JsonPropertyName("MaChiTieu")]
        public string MaChiTieu { get; set; }

        [JsonPropertyName("TenChiTieu")]
        public string TenChiTieu { get; set; }

        [JsonPropertyName("ChiTieuChaID")]
        public int? ChiTieuChaID { get; set; }

        [JsonPropertyName("GhiChu")]
        public string? GhiChu { get; set; } 

        [JsonPropertyName("LoaiMauPhieuID")]
        public int LoaiMauPhieuID { get; set; }

    }

    public class CtgChiTieuInsertChidrenModel
    {
        [JsonPropertyName("MaChiTieu")]
        public string MaChiTieu { get; set; }

        [JsonPropertyName("TenChiTieu")]
        public string TenChiTieu { get; set; }

        [JsonPropertyName("ChiTieuChaID")]
        public int? ChiTieuChaID { get; set; }

        [JsonPropertyName("GhiChu")]
        public string? GhiChu { get; set; }
    }

    public class CtgChiTieuUpdateModel
    {
        [JsonPropertyName("ChiTieuID")]
        public int ChiTieuID { get; set; }

        [JsonPropertyName("ChiTieuChaID")]
        public int? ChiTieuChaID { get; set; }

        [JsonPropertyName("MaChiTieu")]
        public string MaChiTieu { get; set; }

        [JsonPropertyName("TenChiTieu")]
        public string TenChiTieu { get; set; }

        [JsonPropertyName("LoaiMauPhieuID")]
        public int LoaiMauPhieuID { get; set; }

        [JsonPropertyName("GhiChu")]
        public string? GhiChu { get; set; }
    }

}
