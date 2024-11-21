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

        // Role Seed
        await SeedRolesAsync();

        // Different Role Users Seed
        await SeedUsersAsync();

        // Personal Trainers Specialities Seed
        SeedSpecialtiesAndSpecializations();

        // Save Changes
        await _context.SaveChangesAsync();
    }

    private async Task SeedRolesAsync()
    {
        // Verifies and Creates Roles
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
        // Checks whether specialties and specializations already exist to avoid duplication
        if (_context.Specialties.Any()) return;
        if (_context.Specializations.Any()) return;

        var specialties = new[]
        {
            new Specialty { Name = "Strength Training", ImageUrl = "/images/specialties/strength_training.png", ImageName = "strength_training.png" },
            new Specialty { Name = "Cardio", ImageUrl = "/images/specialties/cardio.png", ImageName = "cardio.png" },
            new Specialty { Name = "Yoga", ImageUrl = "/images/specialties/yoga.png", ImageName = "yoga.png" },
            new Specialty { Name = "Pilates", ImageUrl = "/images/specialties/pilates.png", ImageName = "pilates.png" },
            new Specialty { Name = "Crossfit", ImageUrl = "/images/specialtiesl/crossfit.png", ImageName = "crossfit.png" },
            //new Specialty { Name = "Nutrition", ImageUrl = "/images/specializations/clinical_nutrition.png", ImageName = "clinical_nutrition.png" },
            new Specialty { Name = "Bodybuilding", ImageUrl = "/images/specialties/body_building.png", ImageName = "body_building.png" },
            new Specialty { Name = "Functional Training", ImageUrl = "/images/specialties/functional_training.png", ImageName = "functional_training.png" },
            new Specialty { Name = "HIIT", ImageUrl = "/images/specialties/HIIT_training.png", ImageName = "HIIT_training.png" },
            new Specialty { Name = "Martial Arts", ImageUrl = "/images/specialties/martial_arts.png", ImageName = "martial_arts.png" },
            new Specialty { Name = "Dance Fitness", ImageUrl = "/images/specialties/dance.png", ImageName = "dance.png" },
            new Specialty { Name = "Swimming", ImageUrl = "/images/specialties/swimming.png", ImageName = "swimming.png" },
            new Specialty { Name = "Boxing", ImageUrl = "/images/specialties/boxing.png", ImageName = "boxing.png" },
            new Specialty { Name = "Kickboxing", ImageUrl = "/images/specialties/kickboxing.png", ImageName = "kickboxing.png" },
            new Specialty { Name = "Physical Rehabilitation", ImageUrl = "/images/specialties/rehabilitation_training.jpg", ImageName = "rehabilitation_training.jpg" },
            new Specialty { Name = "Weight Loss", ImageUrl = "/images/specializations/weight_loss.png", ImageName = "weight_loss.png" },
            new Specialty { Name = "Sports Conditioning", ImageUrl = "/images/specialties/sports_conditioning.png", ImageName = "sports_conditioning.png" },
            new Specialty { Name = "Prenatal Training", ImageUrl = "/images/specialties/prenatal_training.png", ImageName = "prenatal_training.png" },
            new Specialty { Name = "Postnatal Training", ImageUrl = "/images/specialties/postnatal_training.png", ImageName = "postnatal_training.png" },
            new Specialty { Name = "Senior Fitness", ImageUrl = "/images/specialties/senior_training.png", ImageName = "senior_training.png" },
            new Specialty { Name = "Mobility and Flexibility", ImageUrl = "/images/specialties/mobility_training.png", ImageName = "mobility_training.png" },
            new Specialty { Name = "Endurance Training", ImageUrl = "/images/specialties/endurance-training.png", ImageName = "endurance-training.png" },
            new Specialty { Name = "Group Fitness", ImageUrl = "/images/specialties/group_fitness.png", ImageName = "group_fitness.png" },
            new Specialty { Name = "Meditation", ImageUrl = "/images/specialties//meditation.png", ImageName = "meditation.png" },
            new Specialty { Name = "Mindfulness", ImageUrl = "/images/specialties/mindfulness.png", ImageName = "mindfulness.png" },
            new Specialty { Name = "Bodyweight Training", ImageUrl = "/images/specialties/bodyweight-training.png", ImageName = "bodyweight-training.png" },
            new Specialty { Name = "TRX", ImageUrl = "/images/specialties/trx.png", ImageName = "trx.png" },
            new Specialty { Name = "Cycling", ImageUrl = "/images/specialties/cycling.png", ImageName = "cycling.png" },
            new Specialty { Name = "Rowing", ImageUrl = "/images/specialties/rowing.png", ImageName = "rowing.png" },
            new Specialty { Name = "Powerlifting", ImageUrl = "/images/specialties/powerlifting.png", ImageName = "powerlifting.png" },
            new Specialty { Name = "Olympic Weightlifting", ImageUrl = "/images/specialties/olympic_weightlifting.png", ImageName = "olympic_weightlifting.png" },
            new Specialty { Name = "Barre", ImageUrl = "/images/specialties/barre.png", ImageName = "barre.png" },
            new Specialty { Name = "Aqua Aerobics", ImageUrl = "/images/specialties/aqua_aerobics.png", ImageName = "aqua_aerobics.png" },
            new Specialty { Name = "Circuit Training", ImageUrl = "/images/specialties/circuit_training.png", ImageName = "circuit_training.png" },
            new Specialty { Name = "Gymnastics", ImageUrl = "/images/specialties/gymnastics.png", ImageName = "gymnastics.png" },
            new Specialty { Name = "Climbing", ImageUrl = "/images/specialties/climbing.png", ImageName = "climbing.png" },
            new Specialty { Name = "Other", ImageUrl = "/images/specialties/other_exercise.png", ImageName = "other_exercise.png" }
        };

        // Specializations for Nutritionists
        var specializations = new[]
        {
            new Specialization { Name = "Weight Loss Management", ImageUrl = "/images/specializations/weight_loss.png", ImageName = "weight_loss.png" },
            new Specialization { Name = "Sports Nutrition", ImageUrl = "/images/specializations/sports_nutrition.png", ImageName = "sports_nutrition.png" },
            new Specialization { Name = "Clinical Nutrition", ImageUrl = "/images/specializations/clinical_nutrition.png", ImageName = "clinical_nutrition.png" },
            new Specialization { Name = "Diabetes Management", ImageUrl = "/images/specializations/diabetes_nutrition.png", ImageName = "diabetes_nutrition.png" },
            new Specialization { Name = "Pediatric Nutrition", ImageUrl = "/images/specializations/pediatric_nutrition.png", ImageName = "pediatric_nutrition.png" },
            new Specialization { Name = "Geriatric Nutrition", ImageUrl = "/images/specializations/geriatric_nutrition.png", ImageName = "geriatric_nutrition.png" },
            new Specialization { Name = "Oncology Nutrition", ImageUrl = "/images/specializations/oncology_nutrition.png", ImageName = "oncology_nutrition.png" },
            new Specialization { Name = "Digestive Health", ImageUrl = "/images/specializations/digestive_health.png", ImageName = "digestive_health.png" },
            new Specialization { Name = "Heart Health", ImageUrl = "/images/specializations/hearth_health.png", ImageName = "hearth_health.png" },
            new Specialization { Name = "Plant-Based Nutrition", ImageUrl = "/images/specializations/plantbased_nutrition.png", ImageName = "plantbased_nutrition.png" },
            new Specialization { Name = "Eating Disorders", ImageUrl = "/images/specializations/eating_disorders.png", ImageName = "eating_disorders.png" },
            new Specialization { Name = "Prenatal Nutrition", ImageUrl = "/images/specializations/prenatal_nutrition.png", ImageName = "prenatal_nutrition.png" },
            new Specialization { Name = "Postnatal Nutrition", ImageUrl = "/images/specializations/postnatal_nutrition.png", ImageName = "postnatal_nutrition.png" },
            new Specialization { Name = "Renal Nutrition", ImageUrl = "/images/specializations/renal_nutrition.png", ImageName = "renal_nutrition.png" },
            new Specialization { Name = "Ketogenic Diets", ImageUrl = "/images/specializations/ketogenic_diets.png", ImageName = "ketogenic_diets.png" },
            new Specialization { Name = "Mediterranean Diets", ImageUrl = "/images/specializations/mediterranean_diets.png", ImageName = "mediterranean_diets.png" },
            new Specialization { Name = "Food Allergies and Intolerances", ImageUrl = "/images/specializations/food_allergies_intolerant.png", ImageName = "food_allergies_intolerant.png" },
            new Specialization { Name = "Gluten-Free Diets", ImageUrl = "/images/specializations/glutenfree_diets.png", ImageName = "glutenfree_diets.png" },
            new Specialization { Name = "Vegan and Vegetarian Nutrition", ImageUrl = "/images/specializations/vegan_vegetarian_diets.png", ImageName = "vegan_vegetarian_diets.png" },
            new Specialization { Name = "Other", ImageUrl = "/images/specializations/other_diets.png", ImageName = "other_diets.png" }
        };

        _context.Specialties.AddRange(specialties);
        _context.Specializations.AddRange(specializations);
    }
}
