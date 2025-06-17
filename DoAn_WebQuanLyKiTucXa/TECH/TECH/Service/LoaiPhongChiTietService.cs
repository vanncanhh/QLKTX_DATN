using TECH.Data.DatabaseEntity;
using TECH.Reponsitory;

namespace TECH.Service
{
    public interface ILoaiPhongChiTietService
    {
        LoaiPhongChiTiet? GetById(int id);
    }

    public class LoaiPhongChiTietService : ILoaiPhongChiTietService
    {
        private readonly IRepository<LoaiPhongChiTiet, int> _repository;

        public LoaiPhongChiTietService(IRepository<LoaiPhongChiTiet, int> repository)
        {
            _repository = repository;
        }

        public LoaiPhongChiTiet? GetById(int id)
        {
            return _repository.FindById(id);
        }
    }
}
