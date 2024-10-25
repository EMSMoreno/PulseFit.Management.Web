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

        public async Task CreateAlertAsync(Alert alert)
        {
            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Alert>> GetActiveAlertsAsync()
        {
            return await _context.Alerts
                .Where(a => !a.IsResolved)
                .ToListAsync();
        }

        public async Task<IEnumerable<Alert>> GetAllAlertsAsync()
        {
            return await _context.Alerts.Where(a => !a.IsResolved).ToListAsync();
        }

        public async Task MarkAlertAsResolvedAsync(int alertId)
        {
            var alert = await _context.Alerts.FindAsync(alertId);
            if (alert != null)
            {
                alert.IsResolved = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
