using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IDotDangKyKTXRepository : IRepository<DotDangKyKTX, int> { }
    public class DotDangKyKTXRepository : EFRepository<DotDangKyKTX, int>, IDotDangKyKTXRepository
    {
        public DotDangKyKTXRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
