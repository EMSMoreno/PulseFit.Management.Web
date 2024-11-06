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

        // Método para obter todos os personal trainers com os dados do utilizador
        public async Task<List<PersonalTrainer>> GetAllWithUsersAsync()
        {
            return await _context.PersonalTrainers
                .Include(p => p.User)  // Inclui os dados relacionados ao User
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PersonalTrainer> GetByIdWithUserAndSpecialtiesAsync(int id)
        {
            return await _context.PersonalTrainers
                .Include(pt => pt.User)
                .Include(pt => pt.Specialties) // Certifique-se de incluir as especialidades
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
