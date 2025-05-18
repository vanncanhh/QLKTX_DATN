using TECH.Data.DatabaseEntity;
using Website.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IThietBiPhongRepository : IRepository<ThietBiPhong, int>
    {

    }
    public class ThietBiPhongRepository : EFRepository<ThietBiPhong, int>, IThietBiPhongRepository
    {
        public ThietBiPhongRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
