namespace TECH.Areas.Admin.Models
{
    public class SuaChuaModelView
    {
        public int Id { get; set; }
        public int? MaThietBi { get; set; }
        public int? UserId { get; set; }
        public int? MaPhong { get; set; }
        public DateTime? NgayTao { get; set; }
        public string? Comment { get; set; }
        public int? Status { get; set; }
        public decimal? TienSua { get; set; }
        public string? TenNguoiSua { get; set; }

        // Thông tin mở rộng cho hiển thị (không có trong DB)
        public string? TenPhong { get; set; }
        public string? TenThietBi { get; set; }
    }
}
