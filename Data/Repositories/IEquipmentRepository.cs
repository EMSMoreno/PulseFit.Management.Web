using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    public interface IEquipmentRepository : IGenericRepository<Equipment>
    {
        Task<IEnumerable<Equipment>> GetEquipmentsByGymIdAsync(int gymId);

        Task<Equipment?> GetEquipmentByIdAsync(int id);

        Task<List<Equipment>> GetEquipmentsListByIdsAsync(List<int> ids);

        Task<List<Equipment>> GetAllAsync();
    }
}