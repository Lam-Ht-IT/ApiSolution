using System.Text.Json.Serialization;

namespace QUANLYVANHOA.Models
{
    public class RpChiTietMauPhieu
    {
        [JsonPropertyName("ChiTietMauPhieuId")]
        public int ChiTietMauPhieuId { get; set; }

        [JsonPropertyName("MauPhieuId")]
        public int MauPhieuId { get; set; }

        [JsonPropertyName("ChitieuID")]
        public int ChitieuID { get; set; }

        [JsonPropertyName("TieuChiIDs")]
        public List<int> TieuChiIDs { get; set; }

        [JsonPropertyName("GopCot")]
        public int GopCot { get; set; }

        [JsonPropertyName("GoptuCot")]
        public int GoptuCot { get; set; }

        [JsonPropertyName("GopDenCot")]
        public int GopDenCot { get; set; }

        [JsonPropertyName("SoCotGop")]
        public int SoCotGop { get; set; }

        [JsonPropertyName("NoiDung")]
        public string NoiDung { get; set; }


        [JsonPropertyName("GhiChu")]
        public string GhiChu { get; set; }
    }

    public class RpChiTietMauPhieuInsertModel
    {
        [JsonPropertyName("MauPhieuId")]
        public int MauPhieuId { get; set; }

        [JsonPropertyName("ChitieuID")]
        public int ChitieuID { get; set; }

        [JsonPropertyName("TieuChiIDs")]
        public List<int> TieuChiIDs { get; set; }

        [JsonPropertyName("GopCot")]
        public int GopCot { get; set; }

        [JsonPropertyName("GoptuCot")]
        public int GoptuCot { get; set; }

        [JsonPropertyName("GopDenCot")]
        public int GopDenCot { get; set; }

        [JsonPropertyName("SoCotGop")]
        public int SoCotGop { get; set; }

        [JsonPropertyName("NoiDung")]
        public string NoiDung { get; set; }

        [JsonPropertyName("GhiChu")]
        public string GhiChu { get; set; }
    }

    public class RpChiTietMauPhieuUpdateModel
    {
        [JsonPropertyName("ChiTietMauPhieuId")]
        public int ChiTietMauPhieuId { get; set; }

        [JsonPropertyName("MauPhieuId")]
        public int MauPhieuId { get; set; }

        [JsonPropertyName("ChitieuID")]
        public int ChitieuID { get; set; }

        [JsonPropertyName("TieuChiIDs")]
        public List<int> TieuChiIDs { get; set; }

        [JsonPropertyName("GopCot")]
        public int GopCot { get; set; }

        [JsonPropertyName("GoptuCot")]
        public int GoptuCot { get; set; }

        [JsonPropertyName("GopDenCot")]
        public int GopDenCot { get; set; }

        [JsonPropertyName("SoCotGop")]
        public int SoCotGop { get; set; }

        [JsonPropertyName("NoiDung")]
        public string NoiDung { get; set; }

        [JsonPropertyName("GhiChu")]
        public string GhiChu { get; set; }
    }


}
