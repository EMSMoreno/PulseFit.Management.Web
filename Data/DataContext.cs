using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<AdminLog> AdminLogs { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientPreference> ClientPreferences { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Gym> Gyms { get; set; }
        public DbSet<Nutritionist> Nutritionists { get; set; }
        public DbSet<NutritionPlan> NutritionPlans { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<OnlineClass> OnlineClasses { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PersonalTrainer> PersonalTrainers { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserSubscription> UserSubscriptions { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<WorkoutPlan> WorkoutPlans { get; set; }
        public DbSet<WorkoutRating> WorkoutRatings { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<Specialization> Specializations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Delete All Connections using Cascade Delete Rule
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PersonalTrainer>()
                .HasMany(pt => pt.Specialties)
                .WithMany(s => s.PersonalTrainers)
                .UsingEntity(j => j.ToTable("PersonalTrainerSpecialties"));

            // Configuração many-to-many para Nutritionist e Specialization
            modelBuilder.Entity<Nutritionist>()
                .HasMany(n => n.Specializations)
                .WithMany(s => s.Nutritionists)
                .UsingEntity(j => j.ToTable("NutritionistSpecializations"));

            // Configuração one-to-many entre Client e UserSubscription
            modelBuilder.Entity<Client>()
                .HasMany(c => c.UserSubscriptions)
                .WithOne(us => us.Client)
                .HasForeignKey(us => us.ClientId);

            // Configuração many-to-one entre UserSubscription e Subscription
            modelBuilder.Entity<UserSubscription>()
                .HasOne(us => us.Subscription)
                .WithMany(s => s.UserSubscriptions)
                .HasForeignKey(us => us.SubscriptionId);
        }
    }
}