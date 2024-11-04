using System.Collections.Generic;
using System.Threading.Tasks;
using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    public interface ISpecialtyRepository : IGenericRepository<Specialty>
    {
        Task<List<Specialty>> GetSpecialtiesByIdsAsync(List<int> ids);

        Task<List<Specialty>> GetAllAsync();

    }
}
