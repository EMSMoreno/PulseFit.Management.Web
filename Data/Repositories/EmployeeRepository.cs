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

        // Method to get all employees with user data
        public async Task<List<Employee>> GetAllWithUsersAsync()
        {
            return await _context.Employees
                .Include(e => e.User)  // Includes data related to the User
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Employee> GetByIdWithUserAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.User) // Include User to avoid NullReferenceException
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        // Gets an employee by UserId
        public async Task<Employee> GetEmployeeByUserIdAsync(string userId)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.UserId == userId);
        }
    }
}
