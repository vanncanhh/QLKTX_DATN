using Microsoft.AspNetCore.Mvc;
using TECH.Areas.Admin.Models.Search;
using TECH.Areas.Admin.Models;
using TECH.Data.DatabaseEntity;
using TECH.Reponsitory;
using TECH.Service;

namespace TECH.Areas.Admin.Controllers
{
    public class DotDangKyKTXController : BaseController
    {
        private readonly IDotDangKyKTXService _dotDangKyService;

        public DotDangKyKTXController(IDotDangKyKTXService dotDangKyService)
        {
            _dotDangKyService = dotDangKyService;
        }

        public IActionResult Index()
        {
            var search = new DotDangKyKTXSearch
            {
                PageIndex = 1,
                PageSize = 100
            };
            var result = _dotDangKyService.GetAllPaging(search);
            return View(result.Results);
        }

        public IActionResult Create()
        {
            return View(new DotDangKyKTXModelView());
        }

        [HttpPost]
        public IActionResult Create(DotDangKyKTXModelView model)
        {
            if (ModelState.IsValid)
            {
                _dotDangKyService.Add(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var model = _dotDangKyService.GetById(id);
            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(DotDangKyKTXModelView model)
        {
            if (ModelState.IsValid)
            {
                _dotDangKyService.Update(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult ToggleTrangThai(int id)
        {
            _dotDangKyService.ToggleTrangThai(id);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            _dotDangKyService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
