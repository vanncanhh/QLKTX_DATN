using Microsoft.AspNetCore.Mvc;
using TECH.Areas.Admin.Models;
using TECH.Data.DatabaseEntity;
using TECH.Service;

namespace TECH.Areas.Admin.Controllers
{
    public class SuaChuaController : BaseController
    {
        private readonly DataBaseEntityContext _context;
        private readonly IThietBiService _thietBiService;
        private readonly IHoaDonService _hoaDonService;

        public SuaChuaController(DataBaseEntityContext context, IThietBiService thietBiService, IHoaDonService hoaDonService)
        {
            _context = context;
            _thietBiService = thietBiService;
            _hoaDonService = hoaDonService;
        }

        [HttpPost]
        public JsonResult HoanTatSuaChua(SuaChuaModelView model)
        {
            if (model == null || model.MaPhong == null || model.MaThietBi == null)
                return Json(new { success = false, message = "Thiếu thông tin." });

            try
            {
                // 1. Lưu sửa chữa
                var sua = new SuaChua
                {
                    MaPhong = model.MaPhong,
                    UserId = model.MaThietBi,
                    NgayTao = model.NgayTao ?? DateTime.Now,
                    Comment = model.Comment,
                    TenNguoiSua = model.TenNguoiSua,
                    TienSua = model.TienSua,
                    Status = 1
                };
                _context.SuaChuas.Add(sua);

                // 2. Cập nhật tình trạng thiết bị
                var tb = _context.ThietBis.FirstOrDefault(x => x.Id == model.MaThietBi);
                if (tb != null)
                {
                    tb.TinhTrang = "Đã sửa";
                    _context.ThietBis.Update(tb);
                }

                _context.SaveChanges();

                // 3. Gộp vào hóa đơn tháng
                var thoiGian = DateTime.Now;
                var thang = new DateTime(thoiGian.Year, thoiGian.Month, 1);
                var hopDong = _context.HopDongs.FirstOrDefault(h => h.MaPhong == model.MaPhong && h.TrangThai == 1);
                if (hopDong == null)
                    return Json(new { success = false, message = "Không tìm thấy hợp đồng phòng." });

                var hoaDon = _context.hoaDons.FirstOrDefault(h =>
                    h.MaHopDong == hopDong.Id &&
                    h.HanDong.HasValue &&
                    h.HanDong.Value.Month == thang.Month &&
                    h.HanDong.Value.Year == thang.Year);

                if (hoaDon == null)
                {
                    hoaDon = new HoaDon
                    {
                        MaHopDong = hopDong.Id,
                        HanDong = thang.AddMonths(1).AddDays(-1),
                        TongTien = 0,
                        TrangThai = 0,
                        GhiChu = $"Hóa đơn tháng {thang:MM/yyyy}"
                    };
                    _context.hoaDons.Add(hoaDon);
                    _context.SaveChanges();
                }

                // Gộp phí sửa chữa
                _hoaDonService.AddChiTietHoaDonSuaChua(hoaDon.Id, model.MaPhong.Value, thang, thang.AddMonths(1).AddDays(-1));

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi hệ thống: " + ex.Message });
            }
        }
    }
}
