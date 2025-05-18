namespace TECH.Areas.Admin.Models
{
    public class ThietBiPhongModelView
    {
        public int Id { get; set; }
        public int MaPhong { get; set; }
        public int MaThietBi { get; set; }

        public DateTime? NgayCap { get; set; }
        public string? GhiChu { get; set; }
        public string? TenThietBi { get; set; }
        public string? LoaiThietBi { get; set; }
        public string? TinhTrang { get; set; }
    }
}
