using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    public class WorkoutRepository : GenericRepository<Workout>, IWorkoutRepository
    {
        private readonly DataContext _context;

        public WorkoutRepository(DataContext context) : base(context)
        {
            _context = context;
        }


    }
}
