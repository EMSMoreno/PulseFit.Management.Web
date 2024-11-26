using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    public interface IUserSubscriptionRepository : IGenericRepository<UserSubscription>
    {
        Task<IEnumerable<UserSubscription>> GetAllAsync(); 

        Task<IEnumerable<UserSubscription>> GetUserSubscriptionsAsync(int clientId);
        Task<UserSubscription> GetUserSubscriptionByIdAsync(int id);

        Task<UserSubscription> GetActiveUserSubscriptionAsync(string userId, int subscriptionId);




    }
}
