using System.Text.Json.Serialization;

namespace QUANLYVANHOA.Models
{
    public class RpMauPhieu
    {
        [JsonPropertyName("MauPhieuId")]
        public int MauPhieuId { get; set; }

        [JsonPropertyName("TenMauPhieu")]
        public string TenMauPhieu { get; set; }

        [JsonPropertyName("MaMauPhieu")]
        public string MaMauPhieu { get; set; }

        [JsonPropertyName("LoaiMauPhieuId")]
        public int LoaiMauPhieuId { get; set; }

        [JsonPropertyName("NgayTao")]
        public string? NgayTao { get; set; }

        [JsonPropertyName("NguoiTao")]
        public string NguoiTao { get; set; }
    }
}
