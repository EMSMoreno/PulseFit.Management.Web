using Microsoft.AspNetCore.Mvc;
using PulseFit.Management.Web.Data.Entities;
using PulseFit.Management.Web.Data.Repositories;

namespace PulseFit.Management.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IAlertRepository _alertRepository;
        private readonly IGymRepository _gymRepository;

        public DashboardController(IAlertRepository alertRepository, IGymRepository gymRepository)
        {
            _alertRepository = alertRepository;
            _gymRepository = gymRepository;
        }

        public async Task<IActionResult> Index()
        {
            var alerts = await _alertRepository.GetAllAlertsAsync();
            return View(alerts); // Pass unresolved alerts to View
        }

        [HttpPost]
        public async Task<IActionResult> CreateAlert(string message, int userId)
        {
            var alert = new Alert
            {
                Message = message,
                CreatedAt = DateTime.UtcNow,
                IsResolved = false,
                UserId = userId
            };
            await _alertRepository.CreateAlertAsync(alert);
            TempData["Message"] = "Alerta criado com sucesso.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ViewAlerts()
        {
            var alerts = await _alertRepository.GetAllAlertsAsync();
            return View(alerts);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAlertAsResolved(int id)
        {
            await _alertRepository.MarkAlertAsResolvedAsync(id);
            TempData["Message"] = "Alert resolved successfully.";
            return RedirectToAction("ViewAlerts");
        }
    }
}
