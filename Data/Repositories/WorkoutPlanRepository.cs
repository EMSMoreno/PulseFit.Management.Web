using Microsoft.EntityFrameworkCore;
using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    public class WorkoutPlanRepository : GenericRepository<WorkoutPlan>, IWorkoutPlanRepository
    {
        private readonly DataContext _context;

        public WorkoutPlanRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<WorkoutPlan> GetWorkoutPlanByIdWithEquipmentsAsync(int id)
        {
            return await _context.WorkoutPlans
                .Include(w => w.Equipments)
                .FirstOrDefaultAsync(w => w.Id == id);
        }
    }
}
