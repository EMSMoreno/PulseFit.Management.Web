using PulseFit.Management.Web.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PulseFit.Management.Web.Data.Repositories
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        Task<List<Client>> GetAllWithUsersAsync();
        Task<Client> GetByIdWithUserAsync(int id);

        Task<IEnumerable<Client>> GetAllClientsWithRoleAsync(string roleName);

        Task<int?> GetClientIdByUserIdAsync(string userId);

        Task<Client> GetByUserIdAsync(string userId);

    }
}
