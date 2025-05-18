using Microsoft.EntityFrameworkCore;
using Website.Data.DatabaseEntity;
namespace TECH.Data.DatabaseEntity
{
    public class DataBaseEntityContext : DbContext
    {
        public DataBaseEntityContext(DbContextOptions<DataBaseEntityContext> options) : base(options) { }

        public DbSet<ChiTietHoaDon> chiTietHoaDons { set; get; }
        public DbSet<Nha> Nhas { set; get; }
        public DbSet<DichVu> dichVus { set; get; }
        public DbSet<HoaDon> hoaDons { set; get; }
        public DbSet<KhachHang> khachHangs { set; get; }
        public DbSet<NhanVien> nhanViens { set; get; }
        public DbSet<HopDong> HopDongs { set; get; }
        public DbSet<Phong> Phongs { set; get; }
        public DbSet<ThanhPho> ThanhPhos { set; get; }
        public DbSet<QuanHuyen> quanHuyens { set; get; }
        public DbSet<PhuongXa> PhuongXas { set; get; }
        public DbSet<DichVuPhong> DichVuPhongs { set; get; }
        public DbSet<ThanhVienPhong> ThanhVienPhongs { set; get; }
        public DbSet<LoiPham> LoiPhams { set; get; }
        public DbSet<TheKiTucXa> TheKiTucXas { set; get; }
        public DbSet<LoaiThietBi> LoaiThietBis { get; set; }
        public DbSet<ThietBi> ThietBis { get; set; }
        public DbSet<ThietBiPhong> ThietBiPhongs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);            
        }
    }
}

