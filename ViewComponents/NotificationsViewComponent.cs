using Microsoft.AspNetCore.Mvc;
using PulseFit.Management.Web.Data.Repositories;

namespace PulseFit.Management.Web.ViewComponents
{
    public class NotificationsViewComponent : ViewComponent
    {
        private readonly IAlertRepository _alertRepository;

        public NotificationsViewComponent(IAlertRepository alertRepository)
        {
            _alertRepository = alertRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var unresolvedAlertsCount = (await _alertRepository.GetActiveAlertsAsync()).Count;
            return View(unresolvedAlertsCount);
        }
    }
}
