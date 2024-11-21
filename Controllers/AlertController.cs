using Microsoft.AspNetCore.Mvc;
using PulseFit.Management.Web.Data.Entities;
using PulseFit.Management.Web.Data.Repositories;
using PulseFit.Management.Web.Helpers;
using PulseFit.Management.Web.Models;

namespace PulseFit.Management.Web.Controllers
{
    public class AlertController : Controller
    {
        private readonly IAlertRepository _alertRepository;
        private readonly IUserHelper _userHelper;

        public AlertController(IAlertRepository alertRepository, IUserHelper userHelper)
        {
            _alertRepository = alertRepository;
            _userHelper = userHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Notifications()
        {
            var alerts = await _alertRepository.GetActiveAlertsAsync();
            return View(alerts); // View for displaying notifications
        }

        [HttpGet]
        public async Task<IActionResult> UnresolvedAlerts()
        {
            var alerts = await _alertRepository.GetActiveAlertsAsync();
            return View(alerts); // View for managing unresolved alerts
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AlertViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = await _userHelper.GetEmployeeByUserAsync(User.Identity.Name);
                var alert = new Alert
                {
                    Message = model.Message,
                    CreatedAt = DateTime.UtcNow,
                    IsResolved = false,
                    EmployeeId = employee.Id
                };

                await _alertRepository.CreateAlertAsync(alert);
                return RedirectToAction("ViewAlerts");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ViewAlerts()
        {
            var employee = await _userHelper.GetEmployeeByUserAsync(User.Identity.Name);
            var alerts = await _alertRepository.GetAlertsByEmployeeIdAsync(employee.Id);
            return View(alerts);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsResolved(int id)
        {
            await _alertRepository.MarkAsResolvedAsync(id);
            return RedirectToAction("UnresolvedAlerts");
        }

    }
}
