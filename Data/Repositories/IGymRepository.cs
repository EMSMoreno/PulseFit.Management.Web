using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    public interface IGymRepository : IGenericRepository<Gym>
    {
        //GetGymsByLocation(), GetGymById(int id)
        Task<string> GetGymNameByIdAsync(int id);
    }
}
