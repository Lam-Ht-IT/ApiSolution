using System.Text.Json.Serialization;

namespace QUANLYVANHOA.Controllers
{
    public class DiTichXepHang
    {
        [JsonPropertyName("DiTichXepHangID")]
        public int DitichXepHangID { get; set; }
        [JsonPropertyName("TenDiTich")]
        public string TenDiTich { get; set; }
        [JsonPropertyName("ThuTuXepHang")]
        public int ThuTuXepHang { get; set; }
    }

    public class DiTichXepHangModel
    {
        [JsonPropertyName("TenDiTich")]
        public string TenDiTich { get; set; }
        [JsonPropertyName("ThuTuXepHang")]
        public int ThuTuXepHang { get; set; }

    }
}
