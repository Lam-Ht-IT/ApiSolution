using System.Text.Json.Serialization;

namespace QUANLYVANHOA.Models
{
    public class ChiTieu
    {
        [JsonPropertyName("ChiTieuID")]
        public int ChiTieuID { get; set; }

        [JsonPropertyName("MaChiTieu")]
        public string MaChiTieu { get; set; }

        [JsonPropertyName("TenChiTieu")]
        public string TenChiTieu { get; set; }

        [JsonPropertyName("ChiTieuChaID")]
        public int? ChiTieuChaID { get; set; }

        [JsonPropertyName("GhiChu")]
        public string? GhiChu { get; set; }

        [JsonPropertyName("TrangThai")]
        public bool TrangThai { get; set; }

        [JsonPropertyName("LoaiMauPhieuID")]
        public int LoaiMauPhieuID { get; set; }
        public List<ChiTieu>? Children { get; set; } = new List<ChiTieu>();
    }
}
