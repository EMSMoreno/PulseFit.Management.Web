using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PulseFit.Management.Web.Data.Repositories;
using PulseFit.Management.Web.Repositories;

namespace PulseFit.Management.Web.Controllers
{
    [Authorize] 
    public class DashboardController : Controller
    {
        private readonly IAlertRepository _alertRepository;

        public DashboardController(IAlertRepository alertRepository)
        {
            _alertRepository = alertRepository;
        }

        public async Task<IActionResult> Index()
        {
            // Dashboard home page accessible to all authenticated users
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminDashboard()
        {
            return View();
        }

        [Authorize(Roles = "Employee")]
        public IActionResult EmployeeDashboard()
        {
            return View();
        }

        [Authorize(Roles = "PersonalTrainer")]
        public IActionResult PersonalTrainerDashboard()
        {
            return View();
        }

        [Authorize(Roles = "Client")]
        public IActionResult ClientDashboard()
        {
            return View();
        }

        [Authorize(Roles = "Nutritionist")]
        public IActionResult NutritionistDashboard()
        {
            return View();
        }
    }
}
