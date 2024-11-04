using Microsoft.EntityFrameworkCore;
using PulseFit.Management.Web.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PulseFit.Management.Web.Data.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly DataContext _context;

        public EmployeeRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        // Método para obter todos os employees com os dados do utilizador
        public async Task<List<Employee>> GetAllWithUsersAsync()
        {
            return await _context.Employees
                .Include(e => e.User)  // Inclui os dados relacionados ao User
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Employee> GetByIdWithUserAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.User) // Inclui o User para evitar NullReferenceException
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

    }
}
