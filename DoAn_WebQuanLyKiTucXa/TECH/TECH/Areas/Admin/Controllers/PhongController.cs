using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;
using System.Text.RegularExpressions;
using TECH.General;
using TECH.Data.DatabaseEntity;

namespace TECH.Areas.Admin.Controllers
{
    public class PhongController : BaseController
    {
        private readonly IPhongService _phongService;
        private readonly IDichVuPhongService _dichVuPhongService;
        private readonly INhaService _nhaService;
        private readonly IDichVuService _dichVuService;
        private readonly IThanhVienPhongService _thanhVienPhongService;
        private readonly IKhachHangService _khachHangService;
        private readonly IHopDongService _hopDongService;
        private readonly ILoaiPhongChiTietService _loaiPhongChiTietService;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        public PhongController(IPhongService phongService,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment,
        INhaService nhaService,
            IDichVuPhongService dichVuPhongService,
            IDichVuService dichVuService,
            IThanhVienPhongService thanhVienPhongService,
            IHopDongService hopDongService,
        IKhachHangService khachHangService,
        ILoaiPhongChiTietService loaiPhongChiTietService)
        {
            _phongService = phongService;
            _nhaService = nhaService;
            _dichVuPhongService = dichVuPhongService;
            _dichVuService = dichVuService;
            _thanhVienPhongService = thanhVienPhongService;
            _khachHangService = khachHangService;
            _hopDongService = hopDongService;
            _hostingEnvironment = hostingEnvironment;
            _loaiPhongChiTietService = loaiPhongChiTietService;
        }
        public IActionResult Index()
        {
            var data = _nhaService.GetAll();
            if (data != null && data.Count > 0)
            {
                int nhaId = data[0].Id;
                var model = new List<PhongModelView>();
                if (nhaId > 0)
                {
                    model = _phongService.GetPhongByNha(nhaId);
                    var hopDongs = _hopDongService.GetPhongByHopDong();

                    foreach (var phong in model)
                    {
                        // Đếm số người đang thuê theo phòng
                        phong.SoNguoiDangThue = hopDongs.Count(hd => hd.MaPhong == phong.Id && (hd.IsDeteled != true));

                        // Nếu SLNguoiMax chưa có, thử lấy từ LoaiPhongChiTiet
                        if (phong.MaLoaiPhongChiTiet.HasValue)
                        {
                            var loaiPhong = _loaiPhongChiTietService.GetById(phong.MaLoaiPhongChiTiet.Value);
                            phong.LoaiPhongChiTiet = loaiPhong;

                            // Gán sức chứa từ loại phòng nếu chưa có
                            if (loaiPhong != null)
                            {
                                phong.SLNguoiMax = loaiPhong.SoLuongNguoi;
                            }

                        }

                        phong.SlotConLai = (phong.SLNguoiMax ?? 0) - phong.SoNguoiDangThue;
                        phong.PhongDaDay = phong.SlotConLai <= 0;
                    }
                    data[0].Phongs = model;
                }
            }
            return View(data);
        }

        [HttpGet]
        public IActionResult AddView()
        {
           
            return View();
        }

        [HttpGet]
        public IActionResult BaoCao()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetDichVuPhongByPhongId(int id)
        {
            //var model = new PhongModelView();
            if (id > 0)
            {
                var data = _dichVuPhongService.GetDichVuByPhong(id);
                if (data != null && data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        if (item.MaDV.HasValue && item.MaDV.Value > 0)
                        {
                            item.DichVu = _dichVuService.GetByid(item.MaDV.Value);
                        }
                        if (item.MaPhong.HasValue && item.MaPhong.Value > 0)
                        {
                            item.Phong = _phongService.GetByid(item.MaPhong.Value);
                        }
                    }
                    return Json(new
                    {
                        Data = data
                    });

                }
            }
            return Json(new
            {
                Data = ""
            });
        }

        [HttpGet]
        public JsonResult GetById(int id)
        {
            var model = new PhongModelView();
            if (id > 0)
            {
                model = _phongService.GetByid(id);
            }
            return Json(new
            {
                Data = model
            });
        }

        [HttpGet]
        public JsonResult GetPhongByNha(int id)
        {
            var model = new List<PhongModelView>();
            if (id > 0)
            {
                model = _phongService.GetPhongByNha(id);
            }
            return Json(new
            {
                Data = model
            });
        }
        [HttpGet]
        public JsonResult GetPhongByMaNha(int maNha)
        {
            var model = new List<PhongModelView>();
            if (maNha > 0)
            {
                model = _phongService.GetPhongByNhaStatus(maNha);
            }
            return Json(new
            {
                Data = model
            });
        }

        [HttpPost]
        public JsonResult AddPhongByNha(PhongModelView phongModelView)
        {
            _phongService.AddPhongFast(phongModelView);
            _phongService.Save();
            return Json(new
            {
                success = true
            });
        }

        [HttpPost]
        public JsonResult Add(PhongModelView phongModelView)
        {
            _phongService.Add(phongModelView);
            _phongService.Save();
            return Json(new
            {
                success = true
            });
        }
        [HttpGet]
        public JsonResult GetAll()
        {
            var data = _phongService.GetAll();
            return Json(new { Data = data });
        }

        [HttpGet]
        public JsonResult GetDichvuPhongByMaPhong(int maPhong)
        {
            var dichvu = _dichVuService.GetAll();
            if (dichvu != null  && dichvu.Count > 0)
            {
                var dichvuphong = _dichVuPhongService.GetDichVuByPhong(maPhong);
                if (dichvuphong != null && dichvuphong.Count > 0)
                {
                    foreach (var item in dichvu)
                    {
                        if (dichvuphong.Exists(d=>d.MaDV == item.Id))
                        {
                            if (dichvu != null && dichvu.Count > 0)
                            {
                                dichvu = dichvu.Where(d => d.Id != item.Id).ToList();
                            }                           
                        }
                    }
                }
            }
            return Json(new { Data = dichvu });
        }

        [HttpPost]
        public JsonResult Update(PhongModelView phongModelView)
        {
            var result = _phongService.Update(phongModelView);
            _phongService.Save();
            return Json(new
            {
                success = result
            });
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _phongService.Deleted(id);
            _phongService.Save();
            return Json(new
            {
                success = result
            });
        }        

        [HttpGet]
        public JsonResult GetAllPaging(PhongViewModelSearch phongViewModelSearch)
        {
            var data = _phongService.GetAllPaging(phongViewModelSearch);
            if (data != null && data.Results != null && data.Results.Count > 0)
            {
                foreach (var item in data.Results)
                {
                    if (item.MaNha.HasValue && item.MaNha.Value > 0)
                    {
                        var nha = _nhaService.GetByid(item.MaNha.Value);
                        if (nha != null && !string.IsNullOrEmpty(nha.TenNha))
                        {
                            item.TenNha = nha.TenNha;
                        }
                        if (item.LoaiPhong.HasValue)
                        {
                            item.LoaiPhongStr = Common.GetLoaiPhong(item.LoaiPhong.Value);
                        }
                        if (item.TinhTrang.HasValue)
                        {
                            item.TinhTrangStr = Common.GetTinhTrangPhong(item.TinhTrang.Value);
                        }
                    }
                    else
                    {
                        item.TenNha = "";
                    }
                }
                if (phongViewModelSearch != null && !string.IsNullOrEmpty(phongViewModelSearch.name))
                {
                    data.Results = data.Results.Where(p => p.TenNha.Contains(phongViewModelSearch.name) ||
                    p.TenPhong.Contains(phongViewModelSearch.name) ||
                    p.TinhTrangStr.Contains(phongViewModelSearch.name)).ToList();
                }
                if (phongViewModelSearch.status > 0)
                {
                    if (phongViewModelSearch.status == 1)
                    {
                        data.Results = data.Results.Where(p => p.TinhTrang == phongViewModelSearch.status).ToList();
                    }
                    else
                    {
                        data.Results = data.Results.Where(p => p.TinhTrang != 1).ToList();
                    }
                   
                }
            }
           
            return Json(new { data = data });
        }
        // dịch vụ của phòng
        [HttpPost]
        public JsonResult AddDichVuPhong(List<DichVuPhongModelView> DichVuPhongModelViews,int maPhong)
        {
            if (maPhong > 0 && DichVuPhongModelViews != null && DichVuPhongModelViews.Count > 0)
            {                
                _dichVuPhongService.DeletedByMaPhong(maPhong);                                                
                if (DichVuPhongModelViews != null && DichVuPhongModelViews.Count > 0)
                {                   
                    if (DichVuPhongModelViews != null && DichVuPhongModelViews.Count > 0)
                    {
                        foreach (var item in DichVuPhongModelViews)
                        {
                            _dichVuPhongService.Add(item);
                        }
                        _phongService.Save();
                    }                    
                }
            }                      
            return Json(new
            {
                success = true
                
            });
        }
        // xử lý phần khách hàng được thêm vào trong phòng

        [HttpGet]
        public JsonResult GetThanhVienPhongByPhongId(int id)
        {
            if (id > 0)
            {
                var data = _thanhVienPhongService.GetThanhVienByPhong(id);
               
                if (data != null && data.Count > 0)
                {
                    var hopdongphong = _hopDongService.GetHopDongByPhong(id);
                    var khachhanghopdong = new ThanhVienPhongModelView();
                    if (hopdongphong != null && hopdongphong.MaKH.HasValue && hopdongphong.MaKH.Value > 0)
                    {
                        var khacHangHopDong = _khachHangService.GetByid(hopdongphong.MaKH.Value);
                        if (khacHangHopDong != null)
                        {
                            khachhanghopdong.KhachHang = khacHangHopDong;
                        }
                        if (!data.Exists(p=>p.MaKH == hopdongphong.MaKH.Value))
                        {
                            khachhanghopdong.MaPhong = data[0].MaPhong;
                            khachhanghopdong.MaKH = khacHangHopDong.Id;
                            data.Add(khachhanghopdong);
                        }                      
                    }
                   
                    foreach (var item in data)
                    {
                        if (item.MaKH.HasValue && item.MaKH.Value > 0)
                        {
                            item.KhachHang = _khachHangService.GetByid(item.MaKH.Value);
                        }
                        if (item.MaPhong.HasValue && item.MaPhong.Value > 0)
                        {
                            item.Phong = _phongService.GetByid(item.MaPhong.Value);
                        }
                    }
                    return Json(new
                    {
                        Data = data
                    });

                }
                else
                {
                    var hopdongphong = _hopDongService.GetHopDongByPhong(id);
                    var khachhanghopdong = new ThanhVienPhongModelView();
                    var lstData = new List<ThanhVienPhongModelView>();
                    if (hopdongphong != null && hopdongphong.MaKH.HasValue && hopdongphong.MaKH.Value > 0)
                    {
                        var khacHangHopDong = _khachHangService.GetByid(hopdongphong.MaKH.Value);
                        if (khacHangHopDong != null)
                        {
                            khachhanghopdong.KhachHang = khacHangHopDong;
                            khachhanghopdong.MaPhong = id;
                            khachhanghopdong.MaKH = khacHangHopDong.Id;
                            lstData.Add(khachhanghopdong);
                            return Json(new
                            {
                                Data = lstData
                            });
                            
                        }
                       
                    }
                }
            }
            return Json(new
            {
                Data = ""
            });
        }
        [HttpPost]
        public IActionResult UploadImage()
        {
            var files = Request.Form.Files;
            if (files != null && files.Count > 0)
            {
                var imageFolder = $@"\category-image\";

                string folder = _hostingEnvironment.WebRootPath + imageFolder;

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                foreach (var itemFile in files)
                {
                    string fileNameFormat = Regex.Replace(itemFile.FileName.ToLower(), @"\s+", "");
                    string filePath = Path.Combine(folder, fileNameFormat);
                    if (!System.IO.File.Exists(filePath))
                    {
                        using (FileStream fs = System.IO.File.Create(filePath))
                        {
                            itemFile.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                }
            }
            return Json(new
            {
                success = true
            });
        }
        [HttpPost]
        public JsonResult AddThanhVienPhong(List<ThanhVienPhongModelView> ThanhVienPhongModelViews, int maPhong)
        {
            // kiểm tra nếu mã phòng tồn tại thì sẽ bắt xóa tất cả các dịch vụ của phòng đó
            if (maPhong > 0)
            {
                _thanhVienPhongService.DeletedByMaPhong(maPhong);
                if (ThanhVienPhongModelViews != null && ThanhVienPhongModelViews.Count > 0)
                {
                    foreach (var item in ThanhVienPhongModelViews)
                    {
                        if (item.MaKH.HasValue && item.MaKH.Value > 0)
                        {
                            _thanhVienPhongService.Add(item);
                        }                        
                    }
                }
            }
            _phongService.Save();
            return Json(new
            {
                success = true
            });
        }
        [HttpGet]
        public IActionResult GetKhachThueByPhong(int phongId)
        {
            var hopDongs = _hopDongService.GetPhongByHopDong()
                .Where(x => x.MaPhong == phongId)
                .ToList();

            var listKhach = new List<object>();

            foreach (var hd in hopDongs)
            {
                var khach = _khachHangService.GetByid(hd.MaKH ?? 0);
                if (khach != null)
                {
                    listKhach.Add(new
                    {
                        Ten = khach.TenKH,
                        SoDienThoai = khach.SoDienThoai,
                        Email = khach.Email,
                        NgayBatDau = hd.NgayBatDau?.ToString("dd/MM/yyyy"),
                        NgayKetThuc = hd.NgayKetThuc?.ToString("dd/MM/yyyy")
                    });
                }
            }

            return Json(new
            {
                success = listKhach.Any(),
                data = listKhach
            });
        }

    }
}
