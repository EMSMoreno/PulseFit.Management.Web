using PulseFit.Management.Web.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace PulseFit.Management.Web.Data.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        //Repositório para assinaturas que implementa ISubscriptionRepository.
        private readonly DataContext _context;

        public SubscriptionRepository(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Subscription> GetAllActiveSubscriptions()
        {
            return _context.Subscriptions
                           .Where(s => s.Status == Subscription.SubscriptionStatus.Active)
                           .ToList();
        }
    }
}
