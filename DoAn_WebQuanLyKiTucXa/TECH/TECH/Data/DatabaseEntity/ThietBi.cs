using System.ComponentModel.DataAnnotations.Schema;
using TECH.SharedKernel;

namespace TECH.Data.DatabaseEntity
{
    [Table("ThietBi")]
    public class ThietBi : DomainEntity<int>
    {
        public string? TenThietBi { get; set; }
        public string? TinhTrang { get; set; }
        public string? GhiChu { get; set; }
        public int? MaLoai { get; set; }

        [ForeignKey("MaLoai")]
        public LoaiThietBi? LoaiThietBi { get; set; }
    }
}
