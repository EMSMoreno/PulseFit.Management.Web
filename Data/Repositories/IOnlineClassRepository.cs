using PulseFit.Management.Web.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PulseFit.Management.Web.Data.Repositories
{
    public interface IOnlineClassRepository : IGenericRepository<OnlineClass>
    {
        Task<IEnumerable<OnlineClass>> GetAllAsync();
    }
}
