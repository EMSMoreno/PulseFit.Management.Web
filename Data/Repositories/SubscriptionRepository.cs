using Microsoft.EntityFrameworkCore;
using PulseFit.Management.Web.Data;
using PulseFit.Management.Web.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PulseFit.Management.Web.Data.Repositories
{
    public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
    {
        private readonly DataContext _context;

        public SubscriptionRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Subscription>> GetAllActiveSubscriptionsAsync()
        {
            return await _context.Subscriptions
                .Where(s => s.Status == SubscriptionStatus.Active)
                .ToListAsync();
        }

        public async Task<Subscription> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Subscriptions
                .Include(s => s.UserSubscriptions)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Subscriptions.AnyAsync(s => s.Name == name);
        }
    }
}
