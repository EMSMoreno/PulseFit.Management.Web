using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    public interface IWorkoutPlanRepository : IGenericRepository<WorkoutPlan>
    {
        public Task<WorkoutPlan> GetWorkoutPlanByIdWithEquipmentsAsync(int id);
    }
}
