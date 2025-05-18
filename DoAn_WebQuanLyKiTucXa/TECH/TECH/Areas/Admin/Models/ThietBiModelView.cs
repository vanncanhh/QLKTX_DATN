namespace TECH.Areas.Admin.Models
{
    public class ThietBiModelView
    {
        public int Id { get; set; }
        public string? TenThietBi { get; set; }
        public string? TinhTrang { get; set; }
        public string? GhiChu { get; set; }
        public int? MaLoai { get; set; }
        public string? LoaiThietBi { get; set; }
        public DateTime? NgayCap { get; set; }
    }
}
