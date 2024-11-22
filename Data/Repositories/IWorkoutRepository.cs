using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    public interface IWorkoutRepository : IGenericRepository<Workout>
    {
        Task IncrementBookingsAsync(int workoutId);
        Task<IEnumerable<Workout>> GetAllAsync();

    }
}
