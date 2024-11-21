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
                PhoneNumber = model.PhoneNumber,
                Address = model.Address
            };

            if (imageId != Guid.Empty)
            {
                user.ProfilePictureId = imageId;
            }

            return new Client
            {
                Id = isNew ? 0 : model.Id,
                Birthdate = model.Birthdate,
                RegistrationDate = model.RegistrationDate,
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
                Address = client.User.Address,
                RegistrationDate = client.RegistrationDate,
                Status = client.Status,
                Gender = client.Gender,
                ImageId = client.User.ProfilePictureId ?? Guid.Empty,
            };
        }



        public async Task<Subscription> ToSubscriptionAsync(SubscriptionViewModel model, Guid imageId, bool isNew)
        {
            var gyms = await _context.Gyms.Where(g => model.SelectedGymIds.Contains(g.Id)).ToListAsync();
            var workouts = await _context.Workouts.Where(w => model.SelectedWorkoutIds.Contains(w.Id)).ToListAsync();
            var nutritionPlans = await _context.NutritionPlans.Where(np => model.SelectedNutritionPlanIds.Contains(np.Id)).ToListAsync();
            var onlineClasses = await _context.OnlineClasses.Where(oc => model.SelectedOnlineClassIds.Contains(oc.Id)).ToListAsync();

            // Recupera a subscrição existente, se não for uma nova
            var subscription = isNew
                ? new Subscription()
                : await _context.Subscriptions.SingleOrDefaultAsync(s => s.Id == model.Id);

            // Atualiza as propriedades
            subscription.Name = model.Name;
            subscription.Description = model.Description;
            subscription.Price = model.Price;
            subscription.MaxWorkouts = model.MaxWorkouts;
            subscription.DurationValue = model.DurationValue;
            subscription.DurationType = model.DurationType;
            subscription.Status = model.Status;
            subscription.SubscriptionType = model.SubscriptionType;
            subscription.IsExclusive = model.IsExclusive;
            subscription.IsAllGymsAccessible = model.IsAllGymsAccessible;
            subscription.IncludedGyms = gyms;
            subscription.IncludedWorkouts = workouts;
            subscription.IncludeNutritionPlans = model.IncludeNutritionPlans;
            subscription.IncludeOnlineClasses = model.IncludeOnlineClasses;
            subscription.MaxPersonalTrainerSessions = model.MaxPersonalTrainerSessions;
            subscription.Has24HourAccess = model.Has24HourAccess;
            subscription.HasVIPAccess = model.HasVIPAccess;
            subscription.PerformanceReportFrequencyInMonths = model.PerformanceReportFrequencyInMonths;
            subscription.DiscountPercentage = model.DiscountPercentage;

            // Define o `ImageId` somente se uma nova imagem foi carregada
            if (imageId != Guid.Empty)
            {
                subscription.ImageId = imageId;
            }

            return subscription;
        }


        public SubscriptionViewModel ToSubscriptionViewModel(Subscription subscription)
        {
            return new SubscriptionViewModel
            {
                Id = subscription.Id,
                Name = subscription.Name,
                Description = subscription.Description,
                Price = subscription.Price,
                MaxWorkouts = subscription.MaxWorkouts,
                DurationValue = subscription.DurationValue,
                DurationType = subscription.DurationType,
                Status = subscription.Status,
                SubscriptionType = subscription.SubscriptionType,
                IsExclusive = subscription.IsExclusive,
                IsAllGymsAccessible = subscription.IsAllGymsAccessible,
                SelectedGymIds = subscription.IncludedGyms.Select(g => g.Id).ToList(),
                SelectedWorkoutIds = subscription.IncludedWorkouts.Select(w => w.Id).ToList(),
                IncludeNutritionPlans = subscription.IncludeNutritionPlans,
                IncludeOnlineClasses = subscription.IncludeOnlineClasses,
                MaxPersonalTrainerSessions = subscription.MaxPersonalTrainerSessions,
                Has24HourAccess = subscription.Has24HourAccess,
                HasVIPAccess = subscription.HasVIPAccess,
                PerformanceReportFrequencyInMonths = subscription.PerformanceReportFrequencyInMonths,
                DiscountPercentage = subscription.DiscountPercentage, // Alteração aqui
                ImageId = subscription.ImageId
            };
        }




        public async Task<UserSubscription> ToUserSubscriptionAsync(UserSubscriptionViewModel model, bool isNew)
        {
            var subscription = await _context.Subscriptions.FindAsync(model.SubscriptionId);
            var client = await _context.Clients.FindAsync(model.ClientId);

            // Calcula a data de término com base na duração da subscrição
            DateTime endDate = model.StartDate;
            if (subscription != null)
            {
                endDate = model.StartDate;
                switch (subscription.DurationType)
                {
                    case DurationType.Days:
                        endDate = endDate.AddDays(subscription.DurationValue);
                        break;
                    case DurationType.Weeks:
                        endDate = endDate.AddDays(subscription.DurationValue * 7);
                        break;
                    case DurationType.Months:
                        endDate = endDate.AddMonths(subscription.DurationValue);
                        break;
                    case DurationType.Years:
                        endDate = endDate.AddYears(subscription.DurationValue);
                        break;
                }
            }

            return new UserSubscription
            {
                Id = isNew ? 0 : model.Id,
                Subscription = subscription,
                SubscriptionId = model.SubscriptionId,
                Client = client,
                ClientId = model.ClientId,
                StartDate = model.StartDate,
                EndDate = endDate,
                Status = model.Status,
                IsPaid = model.IsPaid,
                TransactionId = model.TransactionId ?? "MANUAL_" + Guid.NewGuid().ToString(), // Defina um ID de transação padrão se o admin não fornecer
                AmountPaid = model.AmountPaid
            };
        }


        public UserSubscriptionViewModel ToUserSubscriptionViewModel(UserSubscription userSubscription)
        {
            return new UserSubscriptionViewModel
            {
                Id = userSubscription.Id,
                SubscriptionId = userSubscription.SubscriptionId,
                Subscription = ToSubscriptionViewModel(userSubscription.Subscription),
                StartDate = userSubscription.StartDate,
                EndDate = userSubscription.EndDate,
                Status = userSubscription.Status,
                ClientId = userSubscription.ClientId,
                IsPaid = userSubscription.IsPaid,
                TransactionId = userSubscription.TransactionId,
                AmountPaid = userSubscription.AmountPaid
            };
        }



        public async Task<Payment> ToPaymentAsync(PaymentViewModel model, string userId, Guid transactionId, Payment.PaymentStatus status)
        {
            var subscription = await _context.Subscriptions.FindAsync(model.SubscriptionId);

            return new Payment
            {
                Amount = model.Amount,
                PaymentDate = DateTime.UtcNow,
                UserId = userId,
                Subscription = subscription,
                SubscriptionId = model.SubscriptionId,
                Method = model.SelectedMethod,
                Status = status,
                TransactionId = transactionId.ToString(),
                Description = model.Description
            };
        }

        // Converte Payment para PaymentViewModel
        public PaymentViewModel ToPaymentViewModel(Payment payment)
        {
            return new PaymentViewModel
            {
                Id = payment.Id,
                SubscriptionId = payment.SubscriptionId,
                Amount = payment.Amount,
                UserId = payment.UserId,
                SelectedMethod = payment.Method,
                Description = payment.Description,
                PaymentDate = payment.PaymentDate,
                TransactionId = payment.TransactionId
            };
        }

        public Gym ToGym(GymViewModel model, Guid imageId, bool isNew)
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
                GymImageId = gym.GymImageId,
            };
        }

        public Workout ToWorkout(WorkoutViewModel model, Guid imageId, bool isNew)
        {
            var workout = new Workout
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
                MaxCapacity = model.MaxCapacity,
                Status = model.Status,
                InstructorId = model.InstructorId,
                InstructorName = model.InstructorName,
                GymId = model.GymId,
                GymName = model.GymName,
                Bookings = model.Bookings,
                WorkoutImageId = imageId,
            };

            workout.EndDate = workout.StartDate.AddMinutes(workout.Duration);

            return workout;
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
                Bookings = workout.Bookings,
                WorkoutImageId = workout.WorkoutImageId,
            };
        }

        public async Task<Booking> ToBookingAsync(BookingViewModel model, Guid imageId, bool isNew)
        {
            return new Booking
            {
                Id = isNew ? 0 : model.Id,
                ReservationDate = model.ReservationDate,
                Status = model.Status,
                WorkoutId = model.WorkoutId,
                WorkoutName = model.WorkoutName,
                UserName = model.UserName,
                UserId = model.UserId,
                TrainingDate = model.TrainingDate,
                GymId = model.GymId,
                GymName = model.GymName,
            };
        }

        public BookingViewModel ToBookingViewModel(Booking booking)
        {
            return new BookingViewModel
            {
                Id = booking.Id,
                ReservationDate = booking.ReservationDate,
                Status = booking.Status,
                WorkoutId = booking.WorkoutId,
                WorkoutName = booking.WorkoutName,
                UserName = booking.UserName,
                UserId = booking.UserId,
                TrainingDate = booking.TrainingDate,
                GymId = booking.GymId,
                GymName = booking.GymName,
            };
        }

        public Equipment ToEquipment(EquimentViewModel model, Guid imageId, bool isNew)
        {
            return new Equipment
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                Quantity = model.Quantity,
                Type = model.Type,
                Status = model.Status,
                GymId = model.GymId,
                GymName = model.GymName,
                EquipmentImageId = imageId,
            };
        }

        public EquimentViewModel ToEquipmentViewModel(Equipment equipment)
        {
            return new EquimentViewModel
            {
                Id = equipment.Id,
                Name = equipment.Name,
                Quantity = equipment.Quantity,
                Type = equipment.Type,
                Status = equipment.Status,
                GymId = equipment.GymId,
                GymName = equipment.GymName,
                EquipmentImageId = equipment.EquipmentImageId,

            };
        }

        public async Task<WorkoutPlan> ToWorkoutPlanAsync(WorkoutPlanViewModel model, Guid imageId, bool isNew)
        {
            var equipments = await _context.Equipments
                .Where(e => model.EquipmentIds.Contains(e.Id))
                .ToListAsync();

            return new WorkoutPlan
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                Description = model.Description,
                Difficulty = (WorkoutPlan.WorkoutPlanDifficulty)model.Difficulty,
                WorkoutPlanType = (WorkoutPlan.WorkoutPlanTypeList)model.WorkoutPlanType,
                Equipments = equipments,
                WorkoutPlanImageId = imageId,
            };
        }

        public WorkoutPlanViewModel ToWorkoutPlanViewModel(WorkoutPlan workoutPlan)
        {
            return new WorkoutPlanViewModel
            {
                Id = workoutPlan.Id,
                Name = workoutPlan.Name,
                Description = workoutPlan.Description,
                Difficulty = (WorkoutPlanViewModel.WorkoutPlanDifficulty)workoutPlan.Difficulty,
                EquipmentIds = workoutPlan.Equipments.Select(e => e.Id).ToList(),
                Equipments = workoutPlan.Equipments.Select(e => new EquipmentItemViewModel
                {
                    Value = e.Id.ToString(),
                    Text = e.Name,
                    ImageUrl = e.EquipmentImageUrl
                }).ToList(),
                WorkoutPlanImageId = workoutPlan.WorkoutPlanImageId ?? Guid.Empty,
            };
        }
    }
}