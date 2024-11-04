using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    public interface IAdminLogRepository
    {
        Task AddAdminLogAsync(AdminLog log);

        Task<IEnumerable<AdminLog>> GetAllAdminLogsAsync();

        Task<IEnumerable<AdminLog>> GetAdminLogsByTypeAsync(string logType);
    }
}