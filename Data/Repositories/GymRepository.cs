using Microsoft.EntityFrameworkCore;
using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    public class GymRepository : GenericRepository<Gym>, IGymRepository
    {
        private readonly DataContext _context;

        //Repository for gyms that implements IGymRepository.
        public GymRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<string> GetGymImageAsync(int id)
        {
            var gym = await _context.Gyms.FirstOrDefaultAsync(g => g.Id == id);
            if (gym == null)
            {
                return null;
            }

            return gym.GymImageUrl;
        }

        public async Task<string> GetGymNameByIdAsync(int id)
        {
            var gym = await _context.Gyms
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id);

            return gym.Name;
        }

        public async Task<IEnumerable<Gym>> GetAllAsync()
        {
            return await _context.Gyms.ToListAsync();
        }
    }
}
