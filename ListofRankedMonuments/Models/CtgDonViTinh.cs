using System.Text.Json.Serialization;

namespace QUANLYVANHOA.Controllers
{
    public class CtgDonViTinh
    {
        [JsonPropertyName("DonViTinhID")]
        public int DonViTinhID { get; set; }

        [JsonPropertyName("TenDonViTinh")]
        public string TenDonViTinh { get; set; }

        [JsonPropertyName("MaDonViTinh")]
        public string MaDonViTinh { get; set; }

        [JsonPropertyName("TrangThai")]
        public bool TrangThai { get; set; }

        [JsonPropertyName("GhiChu")]
        public string GhiChu { get; set; }
    }

    public class CtgDonViTinhModelInsert
    {
        [JsonPropertyName("TenDonViTinh")]
        public string TenDonViTinh { get; set; }

        [JsonPropertyName("MaDonViTinh")]
        public string MaDonViTinh { get; set; }

        [JsonPropertyName("TrangThai")]
        public bool TrangThai { get; set; }

        [JsonPropertyName("GhiChu")]
        public string GhiChu { get; set; }
    }

    public class CtgDonViTinhModelUpdate
    {
        [JsonPropertyName("DonViTinhID")]
        public int DonViTinhID { get; set; }

        [JsonPropertyName("TenDonViTinh")]
        public string TenDonViTinh { get; set; }

        [JsonPropertyName("MaDonViTinh")]
        public string MaDonViTinh { get; set; }

        [JsonPropertyName("TrangThai")]
        public bool TrangThai { get; set; }

        [JsonPropertyName("GhiChu")]
        public string GhiChu { get; set; }
    }
}
