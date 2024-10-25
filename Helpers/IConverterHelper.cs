using PulseFit.Management.Web.Data.Entities;
using PulseFit.Management.Web.Models;

namespace PulseFit.Management.Web.Helpers
{
    public interface IConverterHelper
    {
        Task<PersonalTrainer> ToPersonalTrainerAsync(PersonalTrainerViewModel model, Guid imageId, bool isNew);
        PersonalTrainerViewModel ToPersonalTrainerViewModel(PersonalTrainer personalTrainer);

        Task<Employee> ToEmployeeAsync(EmployeeViewModel model, Guid imageId, bool isNew);
        EmployeeViewModel ToEmployeeViewModel(Employee employee);
    }
}
