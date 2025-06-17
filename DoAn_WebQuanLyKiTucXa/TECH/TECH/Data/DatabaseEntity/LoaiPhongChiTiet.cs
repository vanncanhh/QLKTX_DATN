using System.ComponentModel.DataAnnotations.Schema;
using TECH.SharedKernel;

namespace TECH.Data.DatabaseEntity
{
    [Table("LoaiPhongChiTiet")]
    public class LoaiPhongChiTiet : DomainEntity<int>
    {
        [Column(TypeName = "nvarchar(250)")]
        public string? TenLoai { get; set; }

        public int SoLuongNguoi { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        public decimal DonGia { get; set; }

        public bool LaPhongDichVu { get; set; }

        public bool CoDieuHoa { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? LoaiGiuong { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? GhiChu { get; set; }

        public virtual ICollection<Phong>? Phongs { get; set; }
    }
}
