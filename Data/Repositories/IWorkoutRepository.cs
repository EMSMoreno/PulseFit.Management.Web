using PulseFit.Management.Web.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PulseFit.Management.Web.Data.Repositories
{
    public interface IWorkoutRepository : IGenericRepository<Workout>
    {
        Task<IEnumerable<Workout>> GetAllAsync();
    }
}
