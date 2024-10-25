using PulseFit.Management.Web.Data.Entities;
using PulseFit.Management.Web.Models;
using System;
using System.Threading.Tasks;

namespace PulseFit.Management.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly IUserHelper _userHelper;

        public ConverterHelper(IUserHelper userHelper)
        {
            _userHelper = userHelper;
        }

        // Converte PersonalTrainerViewModel em PersonalTrainer (entidade)
        public async Task<PersonalTrainer> ToPersonalTrainerAsync(PersonalTrainerViewModel model, Guid imageId, bool isNew)
        {
            var user = await _userHelper.GetUserByIdAsync(model.UserId) ?? new User();
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            return new PersonalTrainer
            {
                Id = isNew ? 0 : model.Id,
                UserId = user.Id,
                Specialty = model.Specialty,
                Certification = model.Certification,
                HireDate = model.HireDate,
                Status = model.Status,
                Clients = model.Clients,
                User = user
            };
        }

        public PersonalTrainerViewModel ToPersonalTrainerViewModel(PersonalTrainer personalTrainer)
        {
            return new PersonalTrainerViewModel
            {
                Id = personalTrainer.Id,
                UserId = personalTrainer.UserId,
                FirstName = personalTrainer.User.FirstName,
                LastName = personalTrainer.User.LastName,
                Email = personalTrainer.User.Email,
                PhoneNumber = personalTrainer.User.PhoneNumber,
                Specialty = personalTrainer.Specialty,
                Certification = personalTrainer.Certification,
                HireDate = personalTrainer.HireDate,
                Status = personalTrainer.Status,
                Clients = personalTrainer.Clients,
                ImageId = personalTrainer.User.ProfilePictureId ?? Guid.Empty
            };
        }

        public async Task<Employee> ToEmployeeAsync(EmployeeViewModel model, Guid imageId, bool isNew)
        {
            var user = await _userHelper.GetUserByIdAsync(model.UserId) ?? new User();
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            return new Employee
            {
                Id = isNew ? 0 : model.Id,
                UserId = user.Id,
                EmployeeType = model.EmployeeType,
                HireDate = model.HireDate,
                Status = model.Status,
                Shift = model.Shift,
                User = user
            };
        }

        public EmployeeViewModel ToEmployeeViewModel(Employee employee)
        {
            return new EmployeeViewModel
            {
                Id = employee.Id,
                UserId = employee.UserId,
                FirstName = employee.User.FirstName,
                LastName = employee.User.LastName,
                Email = employee.User.Email,
                PhoneNumber = employee.User.PhoneNumber,
                EmployeeType = employee.EmployeeType,
                HireDate = employee.HireDate,
                Status = employee.Status,
                Shift = employee.Shift,
                ImageId = employee.User.ProfilePictureId ?? Guid.Empty
            };
        }
    }
}