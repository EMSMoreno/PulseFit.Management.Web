using Microsoft.EntityFrameworkCore;
using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    public class GymRepository : GenericRepository<Gym>, IGymRepository
    {
        private readonly DataContext _context;

        //Repositório para ginásios que implementa IGymRepository.
        public GymRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<string> GetGymNameByIdAsync(int id)
        {
            var gym = await _context.Gyms
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id);

            return gym.Name;
        }
    }
}
