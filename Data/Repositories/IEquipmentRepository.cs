using System.Collections.Generic;
using System.Threading.Tasks;
using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    public interface IEquipmentRepository
    {
        Task<IEnumerable<Equipment>> GetEquipmentsByGymIdAsync(int gymId);

        Task AddEquipmentAsync(Equipment equipment);

        Task UpdateEquipmentAsync(Equipment equipment);

        Task RemoveEquipmentAsync(int equipmentId);

        Task<Equipment?> GetEquipmentByIdAsync(int id); // Returns Equipment or null if not found
    }
}