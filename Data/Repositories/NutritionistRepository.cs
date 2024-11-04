using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PulseFit.Management.Web.Data;
using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    public class NutritionistRepository : GenericRepository<Nutritionist>, INutritionistRepository
    {
        private readonly DataContext _context;

        public NutritionistRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Nutritionist>> GetAllWithUsersAsync()
        {
            return await _context.Nutritionists
                .Include(n => n.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Nutritionist> GetByIdWithUserAndSpecializationsAsync(int id)
        {
            return await _context.Nutritionists
                .Include(n => n.User)
                .Include(n => n.Specializations)
                .FirstOrDefaultAsync(n => n.Id == id);
        }
    }
}
