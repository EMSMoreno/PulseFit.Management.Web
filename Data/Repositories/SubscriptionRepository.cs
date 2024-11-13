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

        public async Task<IEnumerable<Subscription>> GetSubscriptionsByGymAsync(int gymId)
        {
            return await _context.Subscriptions
                .Include(s => s.IncludedGyms)
                .Where(s => s.IsAllGymsAccessible || s.IncludedGyms.Any(g => g.Id == gymId))
                .ToListAsync();
        }

        public async Task<bool> ExistsExclusiveSubscriptionAsync(SubscriptionType subscriptionType)
        {
            // Verifica se existe uma subscrição ativa do tipo exclusivo
            return await _context.Subscriptions
                .AnyAsync(s => s.SubscriptionType == subscriptionType
                               && s.IsExclusive
                               && s.Status == SubscriptionStatus.Active);
        }
    }
}
