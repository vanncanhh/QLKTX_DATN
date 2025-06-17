namespace TECH.Areas.Admin.Models
{
    public class DotDangKyKTXModelView
    {
        public int Id { get; set; }
        public string? TenDot { get; set; }
        public DateTime ThoiGianBatDau { get; set; }
        public DateTime ThoiGianKetThuc { get; set; }
        public string? MoTa { get; set; }
        public bool TrangThai { get; set; }
    }
}
