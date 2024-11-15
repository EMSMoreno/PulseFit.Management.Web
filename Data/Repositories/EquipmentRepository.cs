using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    public class EquipmentRepository : GenericRepository<Equipment>, IEquipmentRepository
    {
        private readonly DataContext _context;

        public EquipmentRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public Task<Equipment?> GetEquipmentByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Equipment>> GetEquipmentsByGymIdAsync(int gymId)
        {
            throw new NotImplementedException();
        }
    }
}
