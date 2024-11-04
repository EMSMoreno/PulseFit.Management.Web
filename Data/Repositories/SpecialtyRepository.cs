using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PulseFit.Management.Web.Data;
using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    public class SpecialtyRepository : GenericRepository<Specialty>, ISpecialtyRepository
    {
        private readonly DataContext _context;

        public SpecialtyRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Specialty>> GetSpecialtiesByIdsAsync(List<int> ids)
        {
            return await _context.Specialties
                .Where(s => ids.Contains(s.Id))
                .ToListAsync();
        }

        public async Task<List<Specialty>> GetAllAsync()
        {
            return await _context.Specialties.ToListAsync();
        }
    }
}
