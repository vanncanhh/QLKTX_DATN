using Microsoft.AspNetCore.Mvc;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;

namespace TECH.Areas.Admin.Controllers
{
    public class ThietBiController : BaseController
    {
        private readonly IThietBiService _thietBiService;
        public IHttpContextAccessor _httpContextAccessor;

        public ThietBiController(IThietBiService thietBiService, IHttpContextAccessor httpContextAccessor)
        {
            _thietBiService = thietBiService;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            ViewBag.LoaiThietBis = _thietBiService.GetAllLoaiThietBi();
            return View();
        }

        [HttpGet]
        public JsonResult GetById(int id)
        {
            var model = new ThietBiModelView();
            if (id > 0)
            {
                model = _thietBiService.GetById(id);
            }
            return Json(new { Data = model });
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            var model = _thietBiService.GetAll();
            return Json(new { Data = model });
        }

        [HttpGet]
        public IActionResult AddOrUpdate()
        {
            ViewBag.LoaiThietBis = _thietBiService.GetAllLoaiThietBi();
            return View();
        }

        [HttpPost]
        public JsonResult Add(ThietBiModelView model)
        {
            _thietBiService.Add(model);
            _thietBiService.Save();
            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult Update(ThietBiModelView model)
        {
            var result = _thietBiService.Update(model);
            _thietBiService.Save();
            return Json(new { success = result });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _thietBiService.Deleted(id);
            _thietBiService.Save();
            return Json(new { success = result });
        }

        [HttpGet]
        public JsonResult GetAllPaging(ThietBiViewModelSearch search)
        {
            var data = _thietBiService.GetAllPaging(search);
            return Json(new { data = data });
        }

        [HttpGet]
        public JsonResult GetByPhong(int phongId)
        {
            var data = _thietBiService.GetByPhongId(phongId);
            return Json(new { data = data });
        }
    }
}
