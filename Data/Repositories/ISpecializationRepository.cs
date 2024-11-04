using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    public interface ISpecializationRepository : IGenericRepository<Specialization>
    {
        Task<List<Specialization>> GetSpecializationsByIdsAsync(List<int> ids);
        Task<List<Specialization>> GetAllAsync();
    }
}
