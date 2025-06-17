using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Data.DatabaseEntity;
using TECH.Reponsitory;
using TECH.Utilities;

namespace TECH.Service
{
    public interface IDotDangKyKTXService
    {
        PagedResult<DotDangKyKTXModelView> GetAllPaging(DotDangKyKTXSearch search);
        DotDangKyKTXModelView? GetById(int id);
        void Add(DotDangKyKTXModelView model);
        bool Update(DotDangKyKTXModelView model);
        bool Delete(int id);
        bool ToggleTrangThai(int id);
        void Save();
        DotDangKyKTX? GetDotDangKyDangMo();
    }
    public class DotDangKyKTXService : IDotDangKyKTXService
    {
        private readonly IRepository<DotDangKyKTX, int> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DotDangKyKTXService(IRepository<DotDangKyKTX, int> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public DotDangKyKTX? GetDotDangKyDangMo()
        {
            return _repository.FindAll()
                .FirstOrDefault(x => x.TrangThai == true &&
                                     x.ThoiGianBatDau <= DateTime.Now &&
                                     x.ThoiGianKetThuc >= DateTime.Now);
        }

        public PagedResult<DotDangKyKTXModelView> GetAllPaging(DotDangKyKTXSearch search)
        {
            var query = _repository.FindAll();

            if (!string.IsNullOrEmpty(search.Keyword))
                query = query.Where(x => x.TenDot.Contains(search.Keyword));

            int totalRow = query.Count();

            var data = query.OrderByDescending(x => x.ThoiGianBatDau)
                            .Skip((search.PageIndex - 1) * search.PageSize)
                            .Take(search.PageSize)
                            .Select(x => new DotDangKyKTXModelView
                            {
                                Id = x.Id,
                                TenDot = x.TenDot,
                                ThoiGianBatDau = x.ThoiGianBatDau,
                                ThoiGianKetThuc = x.ThoiGianKetThuc,
                                MoTa = x.MoTa,
                                TrangThai = x.TrangThai
                            }).ToList();

            return new PagedResult<DotDangKyKTXModelView>
            {
                Results = data,
                CurrentPage = search.PageIndex,
                PageSize = search.PageSize,
                RowCount = totalRow
            };
        }

        public DotDangKyKTXModelView? GetById(int id)
        {
            var entity = _repository.FindById(id);
            if (entity == null) return null;

            return new DotDangKyKTXModelView
            {
                Id = entity.Id,
                TenDot = entity.TenDot,
                ThoiGianBatDau = entity.ThoiGianBatDau,
                ThoiGianKetThuc = entity.ThoiGianKetThuc,
                MoTa = entity.MoTa,
                TrangThai = entity.TrangThai
            };
        }

        public void Add(DotDangKyKTXModelView model)
        {
            var entity = new DotDangKyKTX
            {
                TenDot = model.TenDot,
                ThoiGianBatDau = model.ThoiGianBatDau,
                ThoiGianKetThuc = model.ThoiGianKetThuc,
                MoTa = model.MoTa,
                TrangThai = true
            };
            _repository.Add(entity);
            Save();
        }

        public bool Update(DotDangKyKTXModelView model)
        {
            var entity = _repository.FindById(model.Id);
            if (entity == null) return false;

            entity.TenDot = model.TenDot;
            entity.ThoiGianBatDau = model.ThoiGianBatDau;
            entity.ThoiGianKetThuc = model.ThoiGianKetThuc;
            entity.MoTa = model.MoTa;
            entity.TrangThai = model.TrangThai;

            _repository.Update(entity);
            Save();
            return true;
        }

        public bool Delete(int id)
        {
            var entity = _repository.FindById(id);
            if (entity == null) return false;

            _repository.Remove(entity);
            Save();
            return true;
        }

        public bool ToggleTrangThai(int id)
        {
            var entity = _repository.FindById(id);
            if (entity == null) return false;

            entity.TrangThai = !entity.TrangThai;
            _repository.Update(entity);
            Save();
            return true;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
