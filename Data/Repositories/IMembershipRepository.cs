using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Repositories
{
    public interface IMembershipRepository
    {
        Task<IEnumerable<Membership>> GetPendingFeesAsync();

        Task<IEnumerable<Membership>> GetMembershipHistoryAsync(int userId);
    }
}