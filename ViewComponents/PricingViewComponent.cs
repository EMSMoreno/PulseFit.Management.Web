using Microsoft.AspNetCore.Mvc;
using PulseFit.Management.Web.Data.Entities;
using PulseFit.Management.Web.Data.Repositories;
using PulseFit.Management.Web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PulseFit.Management.Web.ViewComponents
{
    public class PricingViewComponent : ViewComponent
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public PricingViewComponent(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var subscriptions = await _subscriptionRepository.GetAllActiveSubscriptionsAsync();
            var generalSubscriptions = subscriptions
                .Where(s => s.SubscriptionType == SubscriptionType.General)
                .Take(3)
                .Select(s => new SubscriptionViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Price = s.Price,
                    DiscountPercentage = s.DiscountPercentage,
                    ImageId = s.ImageId,
                })
                .ToList();

            return View(generalSubscriptions);
        }
    }
}
