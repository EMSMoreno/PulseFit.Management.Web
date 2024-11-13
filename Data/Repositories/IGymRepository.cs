using PulseFit.Management.Web.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PulseFit.Management.Web.Data.Repositories
{
    public interface IGymRepository : IGenericRepository<Gym>
    {
        Task<IEnumerable<Gym>> GetAllAsync();
    }
}
