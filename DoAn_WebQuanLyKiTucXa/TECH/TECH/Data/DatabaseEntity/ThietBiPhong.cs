using System.ComponentModel.DataAnnotations.Schema;
using TECH.Data.DatabaseEntity;
using TECH.SharedKernel;

namespace Website.Data.DatabaseEntity
{
    [Table("ThietBiPhong")]
    public class ThietBiPhong : DomainEntity<int>
    {
        public int MaThietBi { get; set; }
        public int MaPhong { get; set; }
        public DateTime? NgayCap { get; set; }
        public string? GhiChu { get; set; }

        [ForeignKey("MaThietBi")]
        public ThietBi? ThietBi { get; set; }

        [ForeignKey("MaPhong")]
        public Phong? Phong { get; set; }
    }
}
