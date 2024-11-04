using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    public interface INutritionistRepository : IGenericRepository<Nutritionist>
    {
        Task<List<Nutritionist>> GetAllWithUsersAsync();
        Task<Nutritionist> GetByIdWithUserAndSpecializationsAsync(int id);
    }
}
