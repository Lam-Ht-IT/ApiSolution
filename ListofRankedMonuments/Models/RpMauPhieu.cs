using System.Text.Json.Serialization;

namespace QUANLYVANHOA.Models
{
    public class RpMauPhieu
    {
        public int MauPhieuId { get; set; }
        public string TenMauPhieu { get; set; }
        public string MaMauPhieu { get; set; }
        public int LoaiMauPhieuId { get; set; }
        public string? Ngaytao { get; set; }
        public string NguoiTao { get; set; }
    }
}
