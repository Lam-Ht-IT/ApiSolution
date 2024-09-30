using System.Text.Json.Serialization;

namespace QUANLYVANHOA.Controllers
{
    public class CtgLoaiDiTich
    {
        [JsonPropertyName("LoaiDiTichID")]
        public int LoaiDiTichID { get; set; }

        [JsonPropertyName("LoaiDiTichChaID")]
        public int LoaiDiTichChaID { get; set; }

        [JsonPropertyName("TenLoaiDiTich")]
        public string TenLoaiDiTich { get; set; }

        [JsonPropertyName("MaLoaiDiTich")]
        public string MaLoaiDiTich { get; set; }

        [JsonPropertyName("TrangThai")]
        public bool TrangThai { get; set; }

        [JsonPropertyName("GhiChu")]
        public string GhiChu { get; set; }

        [JsonPropertyName("Loai")]
        public int Loai { get; set; }
    }

    public class CtgLoaiDiTichModelInsert
    {
        [JsonPropertyName("TenLoaiDiTich")]
        public string TenLoaiDiTich { get; set; }

        [JsonPropertyName("GhiChu")]
        public string GhiChu { get; set; }
    }

    public class CtgLoaiDiTichModelUpdate
    {
        [JsonPropertyName("LoaiDiTichID")]
        public int LoaiDiTichID { get; set; }

        [JsonPropertyName("TenLoaiDiTich")]
        public string TenLoaiDiTich { get; set; }

        [JsonPropertyName("GhiChu")]
        public string GhiChu { get; set; }
    }
}
