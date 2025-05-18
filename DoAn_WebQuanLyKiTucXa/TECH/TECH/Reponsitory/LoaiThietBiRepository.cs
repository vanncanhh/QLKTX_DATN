using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface ILoaiThietBiRepository : IRepository<LoaiThietBi, int>
    {

    }
    public class LoaiThietBiRepository : EFRepository<LoaiThietBi, int>, ILoaiThietBiRepository
    {
        public LoaiThietBiRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
