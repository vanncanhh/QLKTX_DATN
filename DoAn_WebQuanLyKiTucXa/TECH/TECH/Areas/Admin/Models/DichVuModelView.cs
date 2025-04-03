namespace TECH.Areas.Admin.Models
{
    public class DichVuModelView
    {
        public int Id { get; set; }
        public string? TenDV { get; set; }
        public decimal? DonGia { get; set; }
        public string? DonGiaStr { get; set; }
        public string? GhiChu { get; set; }
        public int? LoaiDV { get; set; }
        public string? LoaiDVStr { get; set; }
        public int? SoLuong { get; set; }   
    }
}
