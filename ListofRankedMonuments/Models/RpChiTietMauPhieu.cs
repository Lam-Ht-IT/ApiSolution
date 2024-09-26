namespace QUANLYVANHOA.Models
{
    public class RpChiTietMauPhieu
    {
        public int ChiTietMauPhieuId { get; set; }
        public int MauPhieuId { get; set; }
        public string TieuChiIDs { get; set; }
        public int ChitieuIDs { get; set; }
        public string NoiDung { get; set; }
        public int GopCot { get; set; }
        public int GoptuCot { get; set; }
        public int GopDenCot { get; set; }
        public int SoCotGop { get; set; }
        public string GhiChu { get; set; }
    }
}
