using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PulseFit.Management.Web.Data;
using PulseFit.Management.Web.Data.Entities;
using PulseFit.Management.Web.Data.Repositories;
using PulseFit.Management.Web.Helpers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os servi�os ao cont�iner.
builder.Services.AddControllersWithViews();

// Configura��o do DbContext com SQL Server
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configura��o do Identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;

    options.Password.RequireDigit = true;
    options.Password.RequiredUniqueChars = 1;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;

})
.AddEntityFrameworkStores<DataContext>()
.AddDefaultTokenProviders();

// Configura��o de Autentica��o JWT
builder.Services.AddAuthentication()
    .AddCookie()
    .AddJwtBearer(cfg =>
    {
        cfg.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Tokens:Issuer"],
            ValidAudience = builder.Configuration["Tokens:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Tokens:Key"]))
        };
    });

// Registro de helpers e reposit�rios
builder.Services.AddScoped<IUserHelper, UserHelper>();
builder.Services.AddTransient<IMailHelper, MailHelper>();
builder.Services.AddScoped<IBlobHelper, BlobHelper>();
builder.Services.AddScoped<IAlertRepository, AlertRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IPersonalTrainerRepository, PersonalTrainerRepository>();
builder.Services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
builder.Services.AddScoped<INutritionistRepository, NutritionistRepository>();
builder.Services.AddScoped<ISpecializationRepository, SpecializationRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IConverterHelper, ConverterHelper>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IUserSubscriptionRepository, UserSubscriptionRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IGymRepository, GymRepository>();
builder.Services.AddScoped<IWorkoutRepository, WorkoutRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IGymRepository, GymRepository>();
builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();
builder.Services.AddScoped<IWorkoutPlanRepository, WorkoutPlanRepository>();
builder.Services.AddScoped<INutritionPlanRepository, NutritionPlanRepository>();
builder.Services.AddScoped<IOnlineClassRepository, OnlineClassRepository>();
builder.Services.AddScoped<SeedDb>(); // Registro do SeedDb

var context = new CustomAssemblyLoadContext();
context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.dll"));
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));



// Configura��o de cookies para o Identity
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/NotAuthorized";
});

var app = builder.Build();
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NDaF5cWWtCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWH9ec3RTRWhfWUx3XUY=");

// Configura��o do pipeline de requisi��es HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Errors/Error");
    app.UseHsts();
}

// Configura��o de redirecionamento e arquivos est�ticos
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Rota padr�o
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Executa o seeding dos dados no in�cio da aplica��o
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var seedDb = services.GetRequiredService<SeedDb>();
    await seedDb.SeedAsync();
}

app.Run();
