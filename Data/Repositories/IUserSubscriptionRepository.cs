using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    public interface IUserSubscriptionRepository : IGenericRepository<UserSubscription>
    {
        Task<IEnumerable<UserSubscription>> GetUserSubscriptionsAsync(int clientId);
        Task<UserSubscription> GetUserSubscriptionByIdAsync(int id);
    }
}
