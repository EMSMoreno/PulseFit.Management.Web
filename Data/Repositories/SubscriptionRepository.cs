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
                .Include(s => s.IncludedGyms)
                .Include(s => s.IncludedWorkouts)
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
            // Checks if there is an active subscription of the exclusive type
            return await _context.Subscriptions
                .AnyAsync(s => s.SubscriptionType == subscriptionType
                               && s.IsExclusive
                               && s.Status == SubscriptionStatus.Active);
        }

        public async Task<IEnumerable<Subscription>> GetSubscriptionsByGymLocationAsync(string location)
        {
            // Get the IDs of all gyms with the provided location
            var gymIds = await _context.Gyms
                .Where(g => g.Location.Contains(location)) // Filter gyms by location
                .Select(g => g.Id)
                .ToListAsync();

            // Retrieve all subscriptions and include associated gyms
            var subscriptions = await _context.Subscriptions
                .Include(s => s.IncludedGyms) // Include gym associations
                .ToListAsync();

            // Filter subscriptions to show those related to the location or accessible to all gyms
            return subscriptions.Where(s =>
                s.IsAllGymsAccessible ||
                s.IncludedGyms.Any(g => gymIds.Contains(g.Id))
            );
        }



    }
}
