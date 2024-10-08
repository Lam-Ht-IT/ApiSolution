using System.Text.Json.Serialization;

namespace QUANLYVANHOA.Models
{
    public class RpChiTietMauPhieu
    {
        [JsonPropertyName("ChiTietMauPhieuID")]
        public int ChiTietMauPhieuID { get; set; }

        [JsonPropertyName("MauPhieuID")]
        public int MauPhieuID { get; set; }

        [JsonPropertyName("ChitieuID")]
        public int ChiTieuID { get; set; }

        [JsonPropertyName("TieuChiIDs")]
        public string? TieuChiIDs { get; set; }

        [JsonPropertyName("GopCot")]
        public bool? GopCot { get; set; }

        [JsonPropertyName("GoptuCot")]
        public int? GopTuCot { get; set; }

        [JsonPropertyName("GopDenCot")]
        public int? GopDenCot { get; set; }

        [JsonPropertyName("SoCotGop")]
        public int? SoCotGop { get; set; }

        [JsonPropertyName("NoiDung")]
        public string? NoiDung { get; set; }


        [JsonPropertyName("GhiChu")]
        public string? GhiChu { get; set; }
    }

    public class RpChiTietMauPhieuInsertModel
    {
        [JsonPropertyName("MauPhieuID")]
        public int MauPhieuID { get; set; }

        [JsonPropertyName("ChitieuID")]
        public int ChiTieuID { get; set; }

        [JsonPropertyName("TieuChiIDs")]
        public string? TieuChiIDs { get; set; }

        [JsonPropertyName("GopCot")]
        public bool? GopCot { get; set; }

        [JsonPropertyName("GoptuCot")]
        public int? GopTuCot { get; set; }

        [JsonPropertyName("GopDenCot")]
        public int? GopDenCot { get; set; }

        [JsonPropertyName("SoCotGop")]
        public int? SoCotGop { get; set; }

        [JsonPropertyName("NoiDung")]
        public string? NoiDung { get; set; }

        [JsonPropertyName("GhiChu")]
        public string? GhiChu { get; set; }
    }

    public class RpChiTietMauPhieuUpdateModel
    {
        [JsonPropertyName("ChiTietMauPhieuID")]
        public int ChiTietMauPhieuID { get; set; }

        [JsonPropertyName("MauPhieuID")]
        public int MauPhieuID { get; set; }

        [JsonPropertyName("ChitieuID")]
        public int ChiTieuID { get; set; }

        [JsonPropertyName("TieuChiIDs")]
        public List<int> TieuChiIDs { get; set; }

        [JsonPropertyName("GopCot")]
        public bool? GopCot { get; set; }

        [JsonPropertyName("GoptuCot")]
        public bool? GopTuCot { get; set; }

        [JsonPropertyName("GopDenCot")]
        public int? GopDenCot { get; set; }

        [JsonPropertyName("SoCotGop")]
        public int? SoCotGop { get; set; }

        [JsonPropertyName("NoiDung")]
        public string? NoiDung { get; set; }

        [JsonPropertyName("GhiChu")]
        public string? GhiChu { get; set; }
    }


}
