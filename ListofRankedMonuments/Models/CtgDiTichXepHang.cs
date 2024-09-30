using System.Text.Json.Serialization;

namespace QUANLYVANHOA.Controllers
{
    public class CtgDiTichXepHang
    {
        [JsonPropertyName("DiTichXepHangID")]
        public int DiTichXepHangID { get; set; }

        [JsonPropertyName("DiTichXepHangChaID")]
        public int DiTichXepHangChaID { get; set; }

        [JsonPropertyName("TenDiTich")]
        public string TenDiTich { get; set; }

        [JsonPropertyName("MaDiTich")]
        public string MaDiTich { get; set; }

        [JsonPropertyName("TrangThai")]
        public bool TrangThai { get; set; }

        [JsonPropertyName("GhiChu")]
        public string GhiChu { get; set; }

        [JsonPropertyName("Loai")]
        public int Loai { get; set; }
    }

    public class CtgDiTichXepHangModelInsert
    {
        [JsonPropertyName("TenDiTich")]
        public string TenDiTich { get; set; }

        [JsonPropertyName("GhiChu")]
        public string GhiChu { get; set; }
    }

    public class CtgDiTichXepHangModelUpdate
    {
        [JsonPropertyName("DiTichXepHangID")]
        public int DiTichXepHangID { get; set; }

        [JsonPropertyName("TenDiTich")]
        public string TenDiTich { get; set; }

        [JsonPropertyName("GhiChu")]
        public string GhiChu { get; set; }
    }

}
