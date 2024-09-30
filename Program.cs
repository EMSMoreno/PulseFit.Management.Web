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
//builder.Services.AddScoped<IConverterHelper, ConverterHelper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

async Task CreateRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roleNames = { "Admin", "User", "Pending" }; // Adicione os papéis que deseja criar
    IdentityResult roleResult;

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            // Criar o papel e armazenar no banco de dados
            roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

// Chamar o método para criar os roles
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await CreateRoles(services);
}


app.Run();
