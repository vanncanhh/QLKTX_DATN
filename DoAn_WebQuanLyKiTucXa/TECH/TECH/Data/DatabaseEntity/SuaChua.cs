using System.ComponentModel.DataAnnotations.Schema;
using TECH.SharedKernel;

namespace TECH.Data.DatabaseEntity
{
    [Table("SuaChua")]
    public class SuaChua : DomainEntity<int>
    {
        public int? UserId { get; set; }
        public int? MaPhong { get; set; }
        public DateTime? NgayTao { get; set; }
        public string? Comment { get; set; }
        public int? Status { get; set; }
        public decimal? TienSua { get; set; }
        public string? TenNguoiSua { get; set; }
    }
}
