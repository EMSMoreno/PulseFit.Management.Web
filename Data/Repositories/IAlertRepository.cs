using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    public interface IAlertRepository : IGenericRepository<Alert>
    {
        Task CreateAlertAsync(Alert alert);
        Task<List<Alert>> GetActiveAlertsAsync();
        Task<Alert> GetAlertByIdAsync(int id);
        Task MarkAsResolvedAsync(int id);
        Task<List<Alert>> GetAlertsByEmployeeIdAsync(int employeeId);
    }
}
