using Microsoft.AspNetCore.Mvc;
using TECH.Areas.Admin.Models;
using TECH.Service;

namespace TECH.Areas.Admin.Controllers
{
    public class ThietBiPhongController : BaseController
    {
        private readonly IThietBiService _thietBiService;
        private readonly IPhongService _phongService;

        public ThietBiPhongController(IThietBiService thietBiService, IPhongService phongService)
        {
            _thietBiService = thietBiService;
            _phongService = phongService;
        }

        public IActionResult Index()
        {
            ViewBag.PhongList = _phongService.GetAll();
            ViewBag.ThietBiList = _thietBiService.GetAll(); // Dùng để chọn thiết bị khi add
            return View();
        }

        // Lấy danh sách thiết bị của phòng
        [HttpGet]
        public JsonResult GetThietBiPhongByPhongId(int phongId)
        {
            if (phongId <= 0)
                return Json(new { success = false, message = "Phòng không hợp lệ" });

            var data = _thietBiService.GetThietBiByPhongId(phongId);
            return Json(new { success = true, data = data });
        }

        [HttpGet]
        public JsonResult AddOrUpdate(int phongId, int thietBiId)
        {
            if (phongId <= 0 || thietBiId <= 0)
                return Json(new { success = false, message = "Thiếu mã phòng hoặc thiết bị" });

            var item = _thietBiService.GetThietBiPhong(phongId)
                                      .FirstOrDefault(x => x.MaThietBi == thietBiId);
            if (item == null)
                return Json(new { success = false, message = "Không tìm thấy thiết bị trong phòng" });

            return Json(new { success = true, data = item });
        }

        // Thêm thiết bị phòng
        [HttpPost]
        public JsonResult AddOrUpdate(ThietBiPhongModelView model)
        {
            if (!ModelState.IsValid || model.MaPhong <= 0 || model.MaThietBi <= 0)
                return Json(new { success = false, message = "Dữ liệu không hợp lệ" });

            var dsThietBi = _thietBiService.GetThietBiPhong(model.MaPhong);
            var existing = dsThietBi.FirstOrDefault(x => x.MaThietBi == model.MaThietBi);

            if (existing == null)
            {
                _thietBiService.AddThietBiPhong(model);
                _thietBiService.Save();
                return Json(new { success = true, message = "Đã thêm thiết bị vào phòng" });
            }
            else
            {
                existing.NgayCap = model.NgayCap;
                existing.GhiChu = model.GhiChu;
                var updated = _thietBiService.UpdateThietBiPhong(existing);
                _thietBiService.Save();
                return Json(new { success = updated, message = updated ? "Cập nhật thành công" : "Cập nhật thất bại" });
            }
        }

        // Xóa thiết bị khỏi phòng
        [HttpPost]
        public JsonResult DeleteThietBiPhong(int phongId, int thietBiId)
        {
            if (phongId <= 0 || thietBiId <= 0)
                return Json(new { success = false, message = "Dữ liệu không hợp lệ" });

            var success = _thietBiService.DeleteThietBiPhong(phongId, thietBiId);
            if (success) _thietBiService.Save();

            return Json(new { success = success, message = success ? "Xoá thành công" : "Không tìm thấy để xoá" });
        }

    }
}
