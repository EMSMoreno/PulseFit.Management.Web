using Microsoft.EntityFrameworkCore;
using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    public class AdminLogRepository : IAdminLogRepository
    {
        private readonly DataContext _context;

        public AdminLogRepository(DataContext context)
        {
            _context = context;
        }

        // Method for adding a new administrative log
        public async Task AddAdminLogAsync(AdminLog log)
        {
            await _context.AdminLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        // Method to retrieve administrative logs by type
        public async Task<IEnumerable<AdminLog>> GetAdminLogsByTypeAsync(string logType)
        {
            return await _context.AdminLogs
               .Where(log => log.Type == logType)
               .ToListAsync();
        }

        // Method to retrieve all administrative logs
        public async Task<IEnumerable<AdminLog>> GetAllAdminLogsAsync()
        {
            return await _context.AdminLogs.ToListAsync();
        }
    }
}
