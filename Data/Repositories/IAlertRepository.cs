using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    public interface IAlertRepository : IGenericRepository<Alert>
    {
        Task<IEnumerable<Alert>> GetActiveAlertsAsync();

        // Methods Added
        Task<IEnumerable<Alert>> GetAllAlertsAsync();
        Task CreateAlertAsync(Alert alert);
        Task MarkAlertAsResolvedAsync(int alertId);
    }
}
