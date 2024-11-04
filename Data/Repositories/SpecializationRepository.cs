using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PulseFit.Management.Web.Data;
using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    public class SpecializationRepository : GenericRepository<Specialization>, ISpecializationRepository
    {
        private readonly DataContext _context;

        public SpecializationRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Specialization>> GetSpecializationsByIdsAsync(List<int> ids)
        {
            return await _context.Specializations
                .Where(s => ids.Contains(s.Id))
                .ToListAsync();
        }

        public async Task<List<Specialization>> GetAllAsync()
        {
            return await _context.Specializations.ToListAsync();
        }
    }
}
