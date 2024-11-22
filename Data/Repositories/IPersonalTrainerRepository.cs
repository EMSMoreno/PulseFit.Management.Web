using PulseFit.Management.Web.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PulseFit.Management.Web.Data.Repositories
{
    public interface IPersonalTrainerRepository : IGenericRepository<PersonalTrainer>
    {
        Task<List<PersonalTrainer>> GetAllWithUsersAsync();
        Task<PersonalTrainer> GetByIdWithUserAndSpecialtiesAsync(int id);

        Task<string> GetPtNameByIdAsync(string id);

    }
}
