using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PulseFit.Management.Web.Data;
using PulseFit.Management.Web.Data.Entities;
using PulseFit.Management.Web.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

public class SeedDb
{
    private readonly DataContext _context;
    private readonly IUserHelper _userHelper;
    private readonly RoleManager<IdentityRole> _roleManager;

    public SeedDb(DataContext context, IUserHelper userHelper, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userHelper = userHelper;
        _roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
        await _context.Database.EnsureCreatedAsync();

        // Seed de roles
        await SeedRolesAsync();

        // Seed de usuários com diferentes roles
        await SeedUsersAsync();

        // Seed de especialidades para personal trainers
        SeedSpecialtiesAndSpecializations();

        // Salva todas as alterações
        await _context.SaveChangesAsync();
    }

    private async Task SeedRolesAsync()
    {
        // Verifica e cria roles para a aplicação PulseFit
        await CheckRoleAsync("Admin");
        await CheckRoleAsync("Client");
        await CheckRoleAsync("PersonalTrainer");
        await CheckRoleAsync("Employee");
        await CheckRoleAsync("Nutritionist");
    }

    private async Task CheckRoleAsync(string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            await _roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    private async Task SeedUsersAsync()
    {
        // Creating users with specific roles
        var adminUser = await CreateUserAsync("admin@pulsefit.com", "Admin", "User", "Admin@123!", "Admin");
        var clientUser = await CreateUserAsync("client@pulsefit.com", "Client", "User", "Client@123!", "Client");
        var trainerUser = await CreateUserAsync("trainer@pulsefit.com", "Trainer", "User", "Trainer@123!", "PersonalTrainer");
        var employeeUser = await CreateUserAsync("employee@pulsefit.com", "Employee", "User", "Employee@123!", "Employee");
        var nutritionistUser = await CreateUserAsync("nutritionist@pulsefit.com", "Nutritionist", "User", "Nutri@123!", "Nutritionist");
    }


    private async Task<User> CreateUserAsync(string email, string firstName, string lastName, string password, string role)
    {
        var user = await _userHelper.GetUserByEmailAsync(email);
        if (user == null)
        {
            user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserName = email,
                EmailConfirmed = true,
                DateCreated = DateTime.UtcNow
            };

            await _userHelper.AddUserAsync(user, password);
            await _userHelper.AddUserToRoleAsync(user, role);
        }
        return user;
    }

    private void SeedSpecialtiesAndSpecializations()
    {
        // Verifica se já existem especialidades e especializações para evitar duplicação
        if (_context.Specialties.Any()) return;
        if (_context.Specializations.Any()) return;

        var specialties = new[]
        {
            new Specialty { Name = "Strength Training", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Cardio", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Yoga", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Pilates", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Crossfit", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Nutrition", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Bodybuilding", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Functional Training", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "HIIT", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Martial Arts", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Dance Fitness", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Swimming", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Boxing", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Kickboxing", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Physical Rehabilitation", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Weight Loss", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Sports Conditioning", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Prenatal Training", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Postnatal Training", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Senior Fitness", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Mobility and Flexibility", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Endurance Training", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Group Fitness", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Meditation", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Mindfulness", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Bodyweight Training", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "TRX", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Cycling", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Rowing", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Powerlifting", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Olympic Weightlifting", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Barre", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Aqua Aerobics", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Circuit Training", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Gymnastics", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Climbing", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" },
            new Specialty { Name = "Other", ImageUrl = "/images/specialties/workout.jpg", ImageName = "workout.jpg" }
        };


        // Especializações para Nutricionistas
        var specializations = new[]
        {
            new Specialization { Name = "Weight Loss Management", ImageUrl = "/images/specializations/default.jpg", ImageName = "default.jpg" },
            new Specialization { Name = "Sports Nutrition", ImageUrl = "/images/specializations/default.jpg", ImageName = "default.jpg" },
            new Specialization { Name = "Clinical Nutrition", ImageUrl = "/images/specializations/default.jpg", ImageName = "default.jpg" },
            new Specialization { Name = "Diabetes Management", ImageUrl = "/images/specializations/default.jpg", ImageName = "default.jpg" },
            new Specialization { Name = "Pediatric Nutrition", ImageUrl = "/images/specializations/default.jpg", ImageName = "default.jpg" },
            new Specialization { Name = "Geriatric Nutrition", ImageUrl = "/images/specializations/default.jpg", ImageName = "default.jpg" },
            new Specialization { Name = "Oncology Nutrition", ImageUrl = "/images/specializations/default.jpg", ImageName = "default.jpg" },
            new Specialization { Name = "Digestive Health", ImageUrl = "/images/specializations/default.jpg", ImageName = "default.jpg" },
            new Specialization { Name = "Heart Health", ImageUrl = "/images/specializations/default.jpg", ImageName = "default.jpg" },
            new Specialization { Name = "Plant-Based Nutrition", ImageUrl = "/images/specializations/default.jpg", ImageName = "default.jpg" },
            new Specialization { Name = "Eating Disorders", ImageUrl = "/images/specializations/default.jpg", ImageName = "default.jpg" },
            new Specialization { Name = "Prenatal Nutrition", ImageUrl = "/images/specializations/default.jpg", ImageName = "default.jpg" },
            new Specialization { Name = "Postnatal Nutrition", ImageUrl = "/images/specializations/default.jpg", ImageName = "default.jpg" },
            new Specialization { Name = "Renal Nutrition", ImageUrl = "/images/specializations/default.jpg", ImageName = "default.jpg" },
            new Specialization { Name = "Ketogenic Diets", ImageUrl = "/images/specializations/default.jpg", ImageName = "default.jpg" },
            new Specialization { Name = "Mediterranean Diets", ImageUrl = "/images/specializations/default.jpg", ImageName = "default.jpg" },
            new Specialization { Name = "Food Allergies and Intolerances", ImageUrl = "/images/specializations/default.jpg", ImageName = "default.jpg" },
            new Specialization { Name = "Gluten-Free Diets", ImageUrl = "/images/specializations/default.jpg", ImageName = "default.jpg" },
            new Specialization { Name = "Vegan and Vegetarian Nutrition", ImageUrl = "/images/specializations/default.jpg", ImageName = "default.jpg" },
            new Specialization { Name = "Other", ImageUrl = "/images/specializations/default.jpg", ImageName = "default.jpg" }
        };

        
        _context.Specialties.AddRange(specialties);
        _context.Specializations.AddRange(specializations);
    }
}
