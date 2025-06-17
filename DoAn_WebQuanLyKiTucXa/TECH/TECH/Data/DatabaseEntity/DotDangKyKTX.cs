using System.ComponentModel.DataAnnotations.Schema;
using TECH.SharedKernel;

namespace TECH.Data.DatabaseEntity
{
    [Table("DotDangKyKTX")]
    public class DotDangKyKTX : DomainEntity<int>
    {
        [Column(TypeName = "nvarchar(255)")]
        public string? TenDot { get; set; }

        public DateTime ThoiGianBatDau { get; set; }

        public DateTime ThoiGianKetThuc { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? MoTa { get; set; }

        public bool TrangThai { get; set; } // true = mở, false = đóng
    }
}
