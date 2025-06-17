using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface ILoaiPhongChiTietRepository : IRepository<LoaiPhongChiTiet, int>
    {

    }
    public class LoaiPhongChiTietRepository : EFRepository<LoaiPhongChiTiet, int>, ILoaiPhongChiTietRepository
    {
        public LoaiPhongChiTietRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
