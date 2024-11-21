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

        Task<Nutritionist> ToNutritionistAsync(NutritionistViewModel model, Guid imageId, bool isNew);
        NutritionistViewModel ToNutritionistViewModel(Nutritionist nutritionist);

        Task<Client> ToClientAsync(ClientViewModel model, Guid imageId, bool isNew);
        ClientViewModel ToClientViewModel(Client client);

        // Converte SubscriptionViewModel para Subscription
        Task<Subscription> ToSubscriptionAsync(SubscriptionViewModel model, Guid imageId, bool isNew);

        // Converte Subscription para SubscriptionViewModel
        SubscriptionViewModel ToSubscriptionViewModel(Subscription subscription);

        // Converte UserSubscriptionViewModel para UserSubscription
        Task<UserSubscription> ToUserSubscriptionAsync(UserSubscriptionViewModel model, bool isNew);

        // Converte UserSubscription para UserSubscriptionViewModel
        UserSubscriptionViewModel ToUserSubscriptionViewModel(UserSubscription userSubscription);

        Task<Payment> ToPaymentAsync(PaymentViewModel model, string userId, Guid transactionId, Payment.PaymentStatus status);
        PaymentViewModel ToPaymentViewModel(Payment payment);

        Gym ToGym(GymViewModel model,  Guid imageId, bool isNew);
        GymViewModel ToGymViewModel(Gym gym);


        Workout ToWorkout(WorkoutViewModel model, Guid imageId, bool isNew);
        WorkoutViewModel ToWorkoutViewModel(Workout workout);


        Task<Booking> ToBookingAsync(BookingViewModel model, Guid imageId, bool isNew);
        BookingViewModel ToBookingViewModel(Booking booking);


        Equipment ToEquipment(EquimentViewModel model, Guid imageId, bool isNew);
        EquimentViewModel ToEquipmentViewModel(Equipment equipment);


        Task<WorkoutPlan> ToWorkoutPlanAsync(WorkoutPlanViewModel model, Guid imageId, bool isNew);
        WorkoutPlanViewModel ToWorkoutPlanViewModel(WorkoutPlan workoutPlan);
    }
}
