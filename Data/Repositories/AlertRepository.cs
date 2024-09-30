using Microsoft.EntityFrameworkCore;
using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    public class AlertRepository : GenericRepository<Alert>, IAlertRepository
    {
        private readonly DataContext _context;

        public AlertRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Alert>> GetActiveAlertsAsync()
        {
            return await _context.Alerts
                .Where(a => !a.IsResolved)
                .ToListAsync();
        }
    }
}
