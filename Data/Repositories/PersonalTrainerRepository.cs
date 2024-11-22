using Microsoft.EntityFrameworkCore;
using PulseFit.Management.Web.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PulseFit.Management.Web.Data.Repositories
{
    public class PersonalTrainerRepository : GenericRepository<PersonalTrainer>, IPersonalTrainerRepository
    {
        private readonly DataContext _context;

        public PersonalTrainerRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        // Method to get all personal trainers with user data
        public async Task<List<PersonalTrainer>> GetAllWithUsersAsync()
        {
            return await _context.PersonalTrainers
                .Include(p => p.User)  // Includes data related to the User
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PersonalTrainer> GetByIdWithUserAndSpecialtiesAsync(int id)
        {
            return await _context.PersonalTrainers
                .Include(pt => pt.User)
                .Include(pt => pt.Specialties)
                .FirstOrDefaultAsync(pt => pt.Id == id);
        }

        public async Task<string> GetPtNameByIdAsync(string id)
        {
            var pt = await _context.PersonalTrainers
                .Include(p => p.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(p  => p.UserId == id);

            return pt.User.FullName;
        }
    }
}
