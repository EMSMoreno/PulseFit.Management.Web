using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PulseFit.Management.Web.Data.Repositories;
using PulseFit.Management.Web.Helpers;
using PulseFit.Management.Web.Models;

namespace PulseFit.Management.Web.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;

        public SubscriptionController(
            ISubscriptionRepository subscriptionRepository,
            IConverterHelper converterHelper,
            IBlobHelper blobHelper)
        {
            _subscriptionRepository = subscriptionRepository;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
        }

        public async Task<IActionResult> Index()
        {
            var subscriptions = await _subscriptionRepository.GetAllActiveSubscriptionsAsync();
            var viewModels = subscriptions.Select(s => _converterHelper.ToSubscriptionViewModel(s));
            return View(viewModels);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubscriptionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var imageId = model.SubscriptionImageFile != null
                    ? await _blobHelper.UploadBlobAsync(model.SubscriptionImageFile, "subscription-images")
                    : Guid.Empty;

                var subscription = await _converterHelper.ToSubscriptionAsync(model, imageId, isNew: true);
                await _subscriptionRepository.CreateAsync(subscription);
                TempData["SuccessMessage"] = "Subscription created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var subscription = await _subscriptionRepository.GetByIdAsync(id);
            if (subscription == null) return NotFound();

            var model = _converterHelper.ToSubscriptionViewModel(subscription);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubscriptionViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var imageId = model.SubscriptionImageFile != null
                    ? await _blobHelper.UploadBlobAsync(model.SubscriptionImageFile, "subscription-images")
                    : model.ImageId;

                var subscription = await _converterHelper.ToSubscriptionAsync(model, imageId, isNew: false);
                await _subscriptionRepository.UpdateAsync(subscription);
                TempData["SuccessMessage"] = "Subscription updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var subscription = await _subscriptionRepository.GetByIdAsync(id);
            if (subscription == null) return NotFound();

            await _subscriptionRepository.DeleteAsync(subscription);
            TempData["SuccessMessage"] = "Subscription deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
