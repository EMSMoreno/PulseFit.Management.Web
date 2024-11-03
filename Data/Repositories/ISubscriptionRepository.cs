using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    public interface ISubscriptionRepository
    {
        //Interface para o repositório de assinaturas.
        IEnumerable<Subscription> GetAllActiveSubscriptions();
    }
}
