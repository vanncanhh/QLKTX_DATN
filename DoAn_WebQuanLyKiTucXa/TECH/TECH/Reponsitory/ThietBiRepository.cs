using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IThietBiRepository : IRepository<ThietBi, int>
    {

    }
    public class ThietBiRepository : EFRepository<ThietBi, int>, IThietBiRepository
    {
        public ThietBiRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
