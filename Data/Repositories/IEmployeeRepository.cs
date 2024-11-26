using PulseFit.Management.Web.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PulseFit.Management.Web.Data.Repositories
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<List<Employee>> GetAllWithUsersAsync();
        Task<Employee> GetByIdWithUserAsync(int id); 

        Task<Employee> GetEmployeeByUserIdAsync(string userId);


    }
}
