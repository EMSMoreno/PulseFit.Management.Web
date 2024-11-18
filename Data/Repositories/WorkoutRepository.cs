using Microsoft.EntityFrameworkCore;
using PulseFit.Management.Web.Data;
using PulseFit.Management.Web.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PulseFit.Management.Web.Data.Repositories
{
    public class WorkoutRepository : GenericRepository<Workout>, IWorkoutRepository
    {
        private readonly DataContext _context;

        public WorkoutRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task IncrementBookingsAsync(int workoutId)
        {
            var workout = await _context.Workouts.FindAsync(workoutId);
            if (workout == null)
            {
                throw new Exception("Workout not found.");
            }

            workout.Bookings += 1;

            _context.Workouts.Update(workout);
            await _context.SaveChangesAsync();
        }
    }
}
