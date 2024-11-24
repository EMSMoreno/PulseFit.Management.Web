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
            // Verifica se existe uma subscrição ativa do tipo exclusivo
            return await _context.Subscriptions
                .AnyAsync(s => s.SubscriptionType == subscriptionType
                               && s.IsExclusive
                               && s.Status == SubscriptionStatus.Active);
        }

        public async Task<IEnumerable<Subscription>> GetSubscriptionsByGymLocationAsync(string location)
        {
            // Obter IDs de todos os gyms com a localização fornecida
            var gymIds = await _context.Gyms
                .Where(g => g.Location.Contains(location)) // Filtrar gyms pela localização
                .Select(g => g.Id)
                .ToListAsync();

            // Obter todas as subscrições e incluir gyms associados
            var subscriptions = await _context.Subscriptions
                .Include(s => s.IncludedGyms) // Incluir as associações de gyms
                .ToListAsync();

            // Filtrar subscrições para exibir as relacionadas à localização ou aquelas acessíveis a todos os gyms
            return subscriptions.Where(s =>
                s.IsAllGymsAccessible ||
                s.IncludedGyms.Any(g => gymIds.Contains(g.Id))
            );
        }


    }
}
