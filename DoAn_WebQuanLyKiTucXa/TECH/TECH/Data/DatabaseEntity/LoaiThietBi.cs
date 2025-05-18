using System.ComponentModel.DataAnnotations.Schema;
using TECH.SharedKernel;

namespace TECH.Data.DatabaseEntity
{
    [Table("LoaiThietBi")]
    public class LoaiThietBi : DomainEntity<int>
    {
        public string? TenLoai { get; set; }
        public string? GhiChu { get; set; }
    }
}
