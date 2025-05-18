using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Data.DatabaseEntity;
using TECH.Reponsitory;
using TECH.Utilities;
using Website.Data.DatabaseEntity;

namespace TECH.Service
{
    public interface IThietBiService
    {
        List<ThietBiModelView> GetAll();
        void Add(ThietBiModelView view);
        bool Update(ThietBiModelView view);
        bool Deleted(int id);
        ThietBiModelView? GetById(int id);
        List<LoaiThietBiModelView> GetAllLoaiThietBi();
        List<ThietBiModelView> GetThietBiByLoaiThietBiId(int loaiId);
        PagedResult<ThietBiModelView> GetAllPaging(ThietBiViewModelSearch search);
        List<ThietBiModelView> GetByPhongId(int phongId);
        void AddThietBiPhong(ThietBiPhongModelView model);
        bool DeleteThietBiPhong(int phongId, int thietBiId);
        List<ThietBiPhongModelView> GetThietBiByPhongId(int phongId);
        List<ThietBiPhongModelView> GetThietBiPhong(int phongId);
        bool UpdateThietBiPhong(ThietBiPhongModelView model);
        void Save();
    }
    public class ThietBiService : IThietBiService
    {
        public readonly IThietBiRepository _thietBiRepository;
        public readonly ILoaiThietBiRepository _loaiThietBiRepository;
        private IUnitOfWork _unitOfWork;
        private readonly DataBaseEntityContext _context;
        public ThietBiService(IThietBiRepository thietBiRepository, ILoaiThietBiRepository loaiThietBiRepository, IUnitOfWork unitOfWork, DataBaseEntityContext context)
        {
            _thietBiRepository = thietBiRepository;
            _loaiThietBiRepository = loaiThietBiRepository;
            _unitOfWork = unitOfWork;
            _context = context;
        }
        public void Add(ThietBiModelView view)
        {
            try
            {
                if(view != null)
                {
                    var thietBi = new ThietBi { 
                        Id = view.Id,
                        TenThietBi = view.TenThietBi,
                        GhiChu = view.GhiChu,
                        TinhTrang = view.TinhTrang,
                        MaLoai = view.MaLoai
                    };
                    _thietBiRepository.Add(thietBi);
                }
            }catch (Exception ex)
            {

            }
        }
         
        public bool Deleted(int id)
        {
            try
            {
                var dataServer = _thietBiRepository.FindById(id);
                if (dataServer != null)
                {
                    _thietBiRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }

        public List<ThietBiModelView> GetAll()
        {
            var dsThietBi = _thietBiRepository.FindAll().ToList();
            var dsLoai = _loaiThietBiRepository.FindAll().ToList();
            var result = dsThietBi.Select(tb => new ThietBiModelView
            {
                Id = tb.Id,
                TenThietBi = tb.TenThietBi,
                TinhTrang = tb.TinhTrang,
                GhiChu = tb.GhiChu,
                MaLoai = tb.MaLoai,
                LoaiThietBi = dsLoai.FirstOrDefault(loai => loai.Id == tb.MaLoai)?.TenLoai
            }).ToList();

            return result;
        }

        public ThietBiModelView? GetById(int id)
        {
            var thietBi = _thietBiRepository.FindAll(tb => tb.Id == id).FirstOrDefault();
            if (thietBi == null) return null;

            var loai = _loaiThietBiRepository.FindAll(l => l.Id == thietBi.MaLoai).FirstOrDefault();

            return new ThietBiModelView
            {
                Id = thietBi.Id,
                TenThietBi = thietBi.TenThietBi,
                TinhTrang = thietBi.TinhTrang,
                GhiChu = thietBi.GhiChu,
                MaLoai = thietBi.MaLoai,
                LoaiThietBi = loai?.TenLoai
            };
        }

        public bool Update(ThietBiModelView model)
        {
            var thietBi = _thietBiRepository.FindAll(tb => tb.Id == model.Id).FirstOrDefault();
            if (thietBi == null) return false;

            thietBi.TenThietBi = model.TenThietBi;
            thietBi.TinhTrang = model.TinhTrang;
            thietBi.GhiChu = model.GhiChu;
            thietBi.MaLoai = model.MaLoai;

            _thietBiRepository.Update(thietBi);
            return true;
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public List<LoaiThietBiModelView> GetAllLoaiThietBi()
        {
            var loais = _loaiThietBiRepository.FindAll().ToList();
            var result = loais.Select(l => new LoaiThietBiModelView
            {
                Id = l.Id,
                TenLoai = l.TenLoai,
                GhiChu = l.GhiChu
            }).ToList();

            return result;
        }
        public List<ThietBiModelView> GetThietBiByLoaiThietBiId(int loaiId)
        {
            var thietBis = _thietBiRepository.FindAll(tb => tb.MaLoai == loaiId).ToList();
            var loai = _loaiThietBiRepository.FindAll(l => l.Id == loaiId).FirstOrDefault();

            var result = thietBis.Select(tb => new ThietBiModelView
            {
                Id = tb.Id,
                TenThietBi = tb.TenThietBi,
                TinhTrang = tb.TinhTrang,
                GhiChu = tb.GhiChu,
                MaLoai = tb.MaLoai,
                LoaiThietBi = loai?.TenLoai
            }).ToList();

            return result;
        }
        public PagedResult<ThietBiModelView> GetAllPaging(ThietBiViewModelSearch search)
        {
            try
            {
                var query = _thietBiRepository.FindAll();

                if (!string.IsNullOrEmpty(search.Keyword))
                {
                    query = query.Where(c => c.TenThietBi.ToLower().Trim().Contains(search.Keyword.ToLower().Trim()));
                }

                if (search.MaPhong.HasValue)
                {
                    var dsThietBiPhong = _context.ThietBiPhongs
                        .Where(tp => tp.MaPhong == search.MaPhong.Value)
                        .Select(tp => tp.MaThietBi)
                        .ToList();

                    query = query.Where(tb => dsThietBiPhong.Contains(tb.Id));
                }

                var totalRow = query.Count();
                query = query.Skip((search.PageIndex - 1) * search.PageSize).Take(search.PageSize);

                var dsLoai = _loaiThietBiRepository.FindAll().ToList();
                var tbList = query.ToList();
                var data = tbList.Select(tb => new ThietBiModelView
                {
                    Id = tb.Id,
                    TenThietBi = tb.TenThietBi,
                    TinhTrang = tb.TinhTrang,
                    GhiChu = tb.GhiChu,
                    MaLoai = tb.MaLoai,
                    LoaiThietBi = dsLoai.FirstOrDefault(loai => loai.Id == tb.MaLoai)?.TenLoai
                }).ToList();

                return new PagedResult<ThietBiModelView>
                {
                    Results = data,
                    CurrentPage = search.PageIndex,
                    PageSize = search.PageSize,
                    RowCount = totalRow
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<ThietBiModelView> GetByPhongId(int phongId)
        {
            var dsThietBiPhong = _context.ThietBiPhongs
                .Where(tp => tp.MaPhong == phongId)
                .ToList();

            var dsThietBiIds = dsThietBiPhong.Select(tp => tp.MaThietBi).ToList();
            var dsThietBi = _thietBiRepository.FindAll(tb => dsThietBiIds.Contains(tb.Id)).ToList();
            var dsLoai = _loaiThietBiRepository.FindAll().ToList();

            var result = dsThietBi.Select(tb => new ThietBiModelView
            {
                Id = tb.Id,
                TenThietBi = tb.TenThietBi,
                TinhTrang = tb.TinhTrang,
                GhiChu = tb.GhiChu,
                MaLoai = tb.MaLoai,
                LoaiThietBi = dsLoai.FirstOrDefault(loai => loai.Id == tb.MaLoai)?.TenLoai
            }).ToList();

            return result;
        }
        public List<ThietBiPhongModelView> GetThietBiByPhongId(int phongId)
        {
            var query = from tbp in _context.ThietBiPhongs
                        join tb in _context.ThietBis on tbp.MaThietBi equals tb.Id
                        join loai in _context.LoaiThietBis on tb.MaLoai equals loai.Id
                        where tbp.MaPhong == phongId
                        select new ThietBiPhongModelView
                        {
                            MaPhong = tbp.MaPhong,
                            MaThietBi = tbp.MaThietBi,
                            NgayCap = tbp.NgayCap,
                            GhiChu = tbp.GhiChu,
                            TenThietBi = tb.TenThietBi,
                            LoaiThietBi = loai.TenLoai
                        };

            return query.ToList();
        }
        public void AddThietBiPhong(ThietBiPhongModelView model)
        {
            var entity = new ThietBiPhong
            {
                MaPhong = model.MaPhong,
                MaThietBi = model.MaThietBi,
                NgayCap = model.NgayCap,
                GhiChu = model.GhiChu
            };
            _context.ThietBiPhongs.Add(entity);
        }

        public bool DeleteThietBiPhong(int phongId, int thietBiId)
        {
            var entity = _context.ThietBiPhongs.FirstOrDefault(x => x.MaPhong == phongId && x.MaThietBi == thietBiId);
            if (entity != null)
            {
                _context.ThietBiPhongs.Remove(entity);
                return true;
            }
            return false;
        }
        public List<ThietBiPhongModelView> GetThietBiPhong(int phongId)
        {
            var dsThietBiPhong = _context.ThietBiPhongs
                .Where(tp => tp.MaPhong == phongId)
                .ToList();

            var dsThietBiIds = dsThietBiPhong.Select(tp => tp.MaThietBi).ToList();
            var dsThietBi = _thietBiRepository.FindAll(tb => dsThietBiIds.Contains(tb.Id)).ToList();
            var dsLoai = _loaiThietBiRepository.FindAll().ToList();

            var result = dsThietBiPhong.Select(tp =>
            {
                var thietBi = dsThietBi.FirstOrDefault(tb => tb.Id == tp.MaThietBi);
                var loai = dsLoai.FirstOrDefault(lo => lo.Id == thietBi?.MaLoai);
                return new ThietBiPhongModelView
                {
                    Id = tp.Id, // Nếu bạn thêm Id trong model
                    MaPhong = tp.MaPhong,
                    MaThietBi = tp.MaThietBi,
                    NgayCap = tp.NgayCap,
                    GhiChu = tp.GhiChu,
                    TenThietBi = thietBi?.TenThietBi,
                    LoaiThietBi = loai?.TenLoai,
                    TinhTrang = thietBi?.TinhTrang
                };
            }).ToList();

            return result;
        }
        public bool UpdateThietBiPhong(ThietBiPhongModelView model)
        {
            var entity = _context.ThietBiPhongs.FirstOrDefault(x => x.MaPhong == model.MaPhong && x.MaThietBi == model.MaThietBi);
            if (entity == null) return false;
            entity.NgayCap = model.NgayCap;
            entity.GhiChu = model.GhiChu;
            _context.ThietBiPhongs.Update(entity);
            return true;
        }
    }
}
