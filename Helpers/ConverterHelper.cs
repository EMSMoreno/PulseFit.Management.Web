using Microsoft.EntityFrameworkCore;
using PulseFit.Management.Web.Data;
using PulseFit.Management.Web.Data.Entities;
using PulseFit.Management.Web.Models;
using System;
using System.Threading.Tasks;

namespace PulseFit.Management.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;

        public ConverterHelper(IUserHelper userHelper, DataContext context)
        {
            _userHelper = userHelper;
            _context = context;
        }

        public async Task<PersonalTrainer> ToPersonalTrainerAsync(PersonalTrainerViewModel model, Guid imageId, bool isNew)
        {
            var user = await _userHelper.GetUserByIdAsync(model.UserId) ?? new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            // Verifica se o imageId não está vazio e atualiza o ProfilePictureId
            if (imageId != Guid.Empty)
            {
                user.ProfilePictureId = imageId;
            }

            var specialties = await _context.Specialties
                .Where(s => model.SpecialtyIds.Contains(s.Id))
                .ToListAsync();

            return new PersonalTrainer
            {
                Id = isNew ? 0 : model.Id,
                UserId = user.Id,
                Specialties = specialties,
                Certification = model.Certification,
                HireDate = model.HireDate,
                Status = model.Status,
                User = user
            };
        }

        // Converte PersonalTrainer para PersonalTrainerViewModel
        public PersonalTrainerViewModel ToPersonalTrainerViewModel(PersonalTrainer personalTrainer)
        {
            return new PersonalTrainerViewModel
            {
                Id = personalTrainer.Id,
                FirstName = personalTrainer.User.FirstName,
                LastName = personalTrainer.User.LastName,
                Email = personalTrainer.User.Email,
                PhoneNumber = personalTrainer.User.PhoneNumber,
                Certification = personalTrainer.Certification,
                HireDate = personalTrainer.HireDate,
                Status = personalTrainer.Status,
                SpecialtyIds = personalTrainer.Specialties.Select(s => s.Id).ToList(),
                Specialties = personalTrainer.Specialties.Select(s => new SpecialtyItemViewModel
                {
                    Value = s.Id.ToString(),
                    Text = s.Name,
                    ImageUrl = s.ImageUrl
                }).ToList(),
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

            if (imageId != Guid.Empty)
            {
                user.ProfilePictureId = imageId;
            }

            return new Employee
            {
                Id = isNew ? 0 : model.Id,
                UserId = user.Id,
                EmployeeType = model.EmployeeType,
                HireDate = model.HireDate,  // aceita valor `null`
                Status = model.Status,
                Shift = model.Shift, // tipo `ShiftType`
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

        // Converte NutritionistViewModel em Nutritionist
        public async Task<Nutritionist> ToNutritionistAsync(NutritionistViewModel model, Guid imageId, bool isNew)
        {
            var user = await _userHelper.GetUserByIdAsync(model.UserId) ?? new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            // Verifica se o imageId não está vazio e atualiza o ProfilePictureId
            if (imageId != Guid.Empty)
            {
                user.ProfilePictureId = imageId;
            }

            var specializations = await _context.Specializations
                .Where(s => model.SpecializationIds.Contains(s.Id))
                .ToListAsync();

            return new Nutritionist
            {
                Id = isNew ? 0 : model.Id,
                UserId = user.Id,
                Specializations = specializations,
                ExperienceYears = model.ExperienceYears,
                Status = model.Status,
                User = user
            };
        }

        // Converte Nutritionist para NutritionistViewModel
        public NutritionistViewModel ToNutritionistViewModel(Nutritionist nutritionist)
        {
            return new NutritionistViewModel
            {
                Id = nutritionist.Id,
                FirstName = nutritionist.User.FirstName,
                LastName = nutritionist.User.LastName,
                Email = nutritionist.User.Email,
                PhoneNumber = nutritionist.User.PhoneNumber,
                ExperienceYears = nutritionist.ExperienceYears,
                Status = nutritionist.Status,
                SpecializationIds = nutritionist.Specializations.Select(s => s.Id).ToList(),
                Specializations = nutritionist.Specializations.Select(s => new SpecialtyItemViewModel
                {
                    Value = s.Id.ToString(),
                    Text = s.Name,
                    ImageUrl = s.ImageUrl
                }).ToList(),
                ImageId = nutritionist.User.ProfilePictureId ?? Guid.Empty
            };
        }

        public async Task<Client> ToClientAsync(ClientViewModel model, Guid imageId, bool isNew)
        {
            var user = await _userHelper.GetUserByIdAsync(model.UserId) ?? new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            if (imageId != Guid.Empty)
            {
                user.ProfilePictureId = imageId;
            }

            return new Client
            {
                Id = isNew ? 0 : model.Id,
                Birthdate = model.Birthdate,
                Address = model.Address,
                RegistrationDate = model.RegistrationDate,
                SubscriptionPlanId = model.SubscriptionPlanId,
                Status = model.Status,
                Gender = model.Gender,
                UserId = user.Id,
                User = user
            };
        }

        public ClientViewModel ToClientViewModel(Client client)
        {
            return new ClientViewModel
            {
                Id = client.Id,
                FirstName = client.User.FirstName,
                LastName = client.User.LastName,
                Email = client.User.Email,
                PhoneNumber = client.User.PhoneNumber,
                Birthdate = client.Birthdate,
                Address = client.Address,
                RegistrationDate = client.RegistrationDate,
                SubscriptionPlanId = client.SubscriptionPlanId,
                Status = client.Status,
                Gender = client.Gender,
                ImageId = client.User.ProfilePictureId ?? Guid.Empty,
            };
        }

        // Converte SubscriptionViewModel para Subscription
        public async Task<Subscription> ToSubscriptionAsync(SubscriptionViewModel model, Guid imageId, bool isNew)
        {
            var subscription = new Subscription
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                MaxWorkouts = model.MaxWorkouts,
                DurationMonths = model.DurationMonths,
                Status = model.Status,
                ImageId = imageId != Guid.Empty ? imageId : model.ImageId
            };

            return subscription;
        }

        // Converte Subscription para SubscriptionViewModel
        public SubscriptionViewModel ToSubscriptionViewModel(Subscription subscription)
        {
            return new SubscriptionViewModel
            {
                Id = subscription.Id,
                Name = subscription.Name,
                Description = subscription.Description,
                Price = subscription.Price,
                MaxWorkouts = subscription.MaxWorkouts,
                DurationMonths = subscription.DurationMonths,
                Status = subscription.Status,
                ImageId = subscription.ImageId
            };
        }

        public async Task<UserSubscription> ToUserSubscriptionAsync(UserSubscriptionViewModel model, bool isNew)
        {
            var subscription = await _context.Subscriptions.FindAsync(model.SubscriptionId);
            var client = await _context.Clients.FindAsync(model.ClientId);

            return new UserSubscription
            {
                Id = isNew ? 0 : model.Id,
                Subscription = subscription,
                SubscriptionId = model.SubscriptionId,
                Client = client,
                ClientId = model.ClientId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Status = model.Status
            };
        }


        // Converte UserSubscription para UserSubscriptionViewModel
        public UserSubscriptionViewModel ToUserSubscriptionViewModel(UserSubscription userSubscription)
        {
            return new UserSubscriptionViewModel
            {
                Id = userSubscription.Id,
                SubscriptionId = userSubscription.SubscriptionId,
                Subscription = ToSubscriptionViewModel(userSubscription.Subscription),
                StartDate = userSubscription.StartDate,
                EndDate = userSubscription.EndDate,
                Status = userSubscription.Status
            };
        }

        public async Task<Gym> ToGym(GymViewModel model, Guid imageId, bool isNew)
        {
            return new Gym
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                Location = model.Location,
                Capacity = model.Capacity,
                OpeningTime = model.OpeningTime,
                ClosingTime = model.ClosingTime,
                CreationDate = model.CreationDate,
                Status = model.Status,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                DayOff = model.DayOff,
                GymImageId = imageId,
            };
        }

        public GymViewModel ToGymViewModel(Gym gym)
        {
            return new GymViewModel
            {
                Id = gym.Id,
                Name = gym.Name,
                Location = gym.Location,
                Capacity = gym.Capacity,
                OpeningTime = gym.OpeningTime,
                ClosingTime = gym.ClosingTime,
                CreationDate = gym.CreationDate,
                Status = gym.Status,
                Email = gym.Email,
                PhoneNumber = gym.PhoneNumber,
                DayOff = gym.DayOff,
            };
        }

        public async Task<Workout> ToWorkoutAsync(WorkoutViewModel model, Guid imageId, bool isNew)
        {
            return new Workout
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                Description = model.Description,
                Duration = model.Duration,
                Type = model.Type,
                IndividualType = model.IndividualType,
                GroupType = model.GroupType,
                Popularity = model.Popularity,
                DifficultyLevel = model.DifficultyLevel,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                MaxCapacity = model.MaxCapacity,
                Status = model.Status,
                InstructorId = model.InstructorId,
                InstructorName = model.InstructorName,
                GymId = model.GymId,
                GymName = model.GymName,
                Bookings = model.Bookings,
                WorkoutImageId = imageId,
            };
        }

        public WorkoutViewModel ToWorkoutViewModel(Workout workout)
        {
            return new WorkoutViewModel
            {
                Id = workout.Id,
                Name = workout.Name,
                Description = workout.Description,
                Duration = workout.Duration,
                Type = workout.Type,
                IndividualType = workout.IndividualType,
                GroupType = workout.GroupType,
                Popularity = workout.Popularity,
                DifficultyLevel = workout.DifficultyLevel,
                StartDate = workout.StartDate,
                EndDate = workout.EndDate,
                MaxCapacity = workout.MaxCapacity,
                Status = workout.Status,
                InstructorId = workout.InstructorId,
                InstructorName = workout.InstructorName,
                GymId = workout.GymId,
                GymName = workout.GymName,
                Bookings = workout.Bookings
            };
        }
    }
}