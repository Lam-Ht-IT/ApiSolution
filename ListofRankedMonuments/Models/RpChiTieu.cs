using System.Text.Json.Serialization;

namespace QUANLYVANHOA.Models
{
    public class RpChiTieu
    {
        [JsonPropertyName("ChiTieuBaoCaoID")]
        public int ChiTieuBaoCaoID { get; set; }

        [JsonPropertyName("MauPhieuID")]
        public int MauPhieuID { get; set; }

        [JsonPropertyName("ChiTieuID")]
        public int ChiTieuID { get; set; }

        [JsonPropertyName("HienThi")]
        public bool HienThi { get; set; }
    }

    public class RpChiTieuInsertModel
    {
        [JsonPropertyName("MauPhieuID")]
        public int MauPhieuID { get; set; }

        [JsonPropertyName("ChiTieuID")]
        public int ChiTieuID { get; set; }

        [JsonPropertyName("HienThi")]
        public bool HienThi { get; set; }
    }

    public class RpChiTieuUpdateModel
    {
        [JsonPropertyName("ChiTieuBaoCaoID")]
        public int ChiTieuBaoCaoID { get; set; }

        [JsonPropertyName("MauPhieuID")]
        public int MauPhieuID { get; set; }

        [JsonPropertyName("ChiTieuID")]
        public int ChiTieuID { get; set; }

        [JsonPropertyName("HienThi")]
        public bool HienThi { get; set; }
    }

    public class RpChiTieuDeleteModel
    {
        [JsonPropertyName("ChiTieuBaoCaoID")]
        public int ChiTieuBaoCaoID { get; set; }
    }
}
