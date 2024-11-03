using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PulseFit.Management.Web.Data;
using PulseFit.Management.Web.Data.Entities;
using PulseFit.Management.Web.Data.Repositories;
using PulseFit.Management.Web.Helpers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DbContext Configuration
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Identity Configuration

builder.Services.AddIdentity<User, IdentityRole>(cfg =>
{
    // Configure token provider for authentication
    cfg.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;

    // Require email confirmation for login
    cfg.SignIn.RequireConfirmedEmail = true;

    // Ensure unique email for each user
    cfg.User.RequireUniqueEmail = true;

    // Password settings
    cfg.Password.RequireDigit = false;
    cfg.Password.RequiredUniqueChars = 0;
    cfg.Password.RequireUppercase = false;
    cfg.Password.RequireLowercase = false;
    cfg.Password.RequireNonAlphanumeric = false;
    cfg.Password.RequiredLength = 6;
})

.AddDefaultTokenProviders()

.AddEntityFrameworkStores<DataContext>();

// Authentication services
builder.Services.AddAuthentication()

    // Enable cookie-based authentication
    .AddCookie()

    // Enable JWT authentication
    .AddJwtBearer(cfg =>
    {
        // Configure JWT token validation parameters
        cfg.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Tokens:Issuer"],

            ValidAudience = builder.Configuration["Tokens:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(

                Encoding.UTF8.GetBytes(builder.Configuration["Tokens:Key"]))
        };
    });

// Generic repository
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Inject UserHelper and MailHelper
builder.Services.AddScoped<IUserHelper, UserHelper>();
builder.Services.AddTransient<IMailHelper, MailHelper>();
builder.Services.AddScoped<IBlobHelper, BlobHelper>();
builder.Services.AddScoped<IAlertRepository, AlertRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IPersonalTrainerRepository, PersonalTrainerRepository>();
builder.Services.AddScoped<IConverterHelper, ConverterHelper>();
builder.Services.AddScoped<IWorkoutRepository, WorkoutRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();

// tópico 7

builder.Services.AddScoped<IAdminLogRepository, AdminLogRepository>(); // tópico 8

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/NotAuthorized";

    options.AccessDeniedPath = "/Account/NotAuthorized";
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Errors/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//Quando nao encontrar a página vai à procura de um error
app.UseStatusCodePagesWithReExecute("/error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userHelper = services.GetRequiredService<IUserHelper>();

    // Lista dos roles que devem existir na aplicação
    string[] roles = { "Admin", "Employee", "Client", "PersonalTrainer", "Nutritionist", "Pending", "Anonymous" };

    // Verifica e cria os roles se necessário
    foreach (var role in roles)
    {
        await userHelper.CheckRoleAsync(role);
    }
}



app.Run();
