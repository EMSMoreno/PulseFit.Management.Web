using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PulseFit.Management.Web.Data.Repositories;
using PulseFit.Management.Web.Repositories;

namespace PulseFit.Management.Web.Controllers
{
    [Authorize] // Garante que apenas utilizadores autenticados podem aceder ao controller
    public class DashboardController : Controller
    {
        private readonly IAlertRepository _alertRepository;

        public DashboardController(IAlertRepository alertRepository)
        {
            _alertRepository = alertRepository;
        }

        public async Task<IActionResult> Index()
        {
            // Página inicial do dashboard acessível para todos os utilizadores autenticados
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminDashboard()
        {
            // Dashboard específico para administradores
            return View();
        }

        [Authorize(Roles = "Employee")]
        public IActionResult EmployeeDashboard()
        {
            // Dashboard específico para funcionários
            return View();
        }

        [Authorize(Roles = "PersonalTrainer")]
        public IActionResult PersonalTrainerDashboard()
        {
            // Dashboard específico para personal trainers
            return View();
        }

        [Authorize(Roles = "Client")]
        public IActionResult ClientDashboard()
        {
            // Dashboard específico para clientes
            return View();
        }

        [Authorize(Roles = "Nutritionist")]
        public IActionResult NutritionistDashboard()
        {
            // Dashboard específico para nutricionistas
            return View();
        }
    }
}
