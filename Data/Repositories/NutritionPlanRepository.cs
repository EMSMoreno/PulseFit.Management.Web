using Microsoft.EntityFrameworkCore;
using PulseFit.Management.Web.Data;
using PulseFit.Management.Web.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PulseFit.Management.Web.Data.Repositories
{
    public class NutritionPlanRepository : GenericRepository<NutritionPlan>, INutritionPlanRepository
    {
        private readonly DataContext _context;

        public NutritionPlanRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NutritionPlan>> GetAllAsync()
        {
            return await _context.NutritionPlans.ToListAsync();
        }
    }
}
