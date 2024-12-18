﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
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
        private readonly IGymRepository _gymRepository;
        private readonly IWorkoutRepository _workoutRepository;
        private readonly ILogger<SubscriptionController> _logger;

        public SubscriptionController(
            ISubscriptionRepository subscriptionRepository,
            IConverterHelper converterHelper,
            IBlobHelper blobHelper,
            IGymRepository gymRepository,
            IWorkoutRepository workoutRepository,
            ILogger<SubscriptionController> logger)
        {
            _subscriptionRepository = subscriptionRepository;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
            _gymRepository = gymRepository;
            _workoutRepository = workoutRepository;
            _logger = logger;
        }

        // GET: Subscription/Index
        public async Task<IActionResult> Index(string location)
        {
            var subscriptions = !string.IsNullOrEmpty(location)
                ? await _subscriptionRepository.GetSubscriptionsByGymLocationAsync(location)
                : await _subscriptionRepository.GetAllActiveSubscriptionsAsync();

            var viewModels = subscriptions.Select(s => _converterHelper.ToSubscriptionViewModel(s)).ToList();

            // Fill the ViewBag with unique gym locations
            ViewBag.GymLocations = (await _gymRepository.GetAllAsync())
                .Select(g => g.Location)
                .Distinct()
                .OrderBy(l => l)
                .ToList();

            // Pass the selected location
            ViewBag.SelectedLocation = location;

            return View(viewModels);
        }





        // GET: Subscription/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var subscription = await _subscriptionRepository.GetByIdWithDetailsAsync(id);
            if (subscription == null)
            {
                _logger.LogWarning("Subscription ID {id} not found", id);
                return NotFound();
            }

            var model = _converterHelper.ToSubscriptionViewModel(subscription);
            return View(model);
        }

        // POST: Subscribe Now -> Redirect to Payment
        [HttpPost]
        public IActionResult SubscribeNow(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Redirects to login, preserving return
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("SubscribeNow", new { id }) });
            }

            // Redirects to payment method selection for logged in customer
            return RedirectToAction("SelectPaymentMethod", "Payment", new { subscriptionId = id });
        }

        // GET: Subscription/Create (Admin Only)
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (!User.IsInRole("Admin")) return Unauthorized();

            var model = new SubscriptionViewModel();
            await PopulateOptions(model);
            return View(model);
        }

        // POST: Subscription/Create (Admin Only)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubscriptionViewModel model)
        {
            if (!User.IsInRole("Admin")) return Unauthorized();

            if (ModelState.IsValid)
            {
                if (await ValidateExclusiveSubscription(model))
                {
                    ModelState.AddModelError("", "An exclusive subscription of this type already exists.");
                    await PopulateOptions(model);
                    return View(model);
                }

                var imageId = model.SubscriptionImageFile != null
                    ? await _blobHelper.UploadBlobAsync(model.SubscriptionImageFile, "subscription-images")
                    : Guid.Empty;

                var subscription = await _converterHelper.ToSubscriptionAsync(model, imageId, isNew: true);
                await _subscriptionRepository.CreateAsync(subscription);

                TempData["SuccessMessage"] = "Subscription created successfully.";
                return RedirectToAction(nameof(Index));
            }

            await PopulateOptions(model);
            return View(model);
        }

        // GET: Subscription/Edit/5 (Admin Only)
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!User.IsInRole("Admin")) return Unauthorized();

            var subscription = await _subscriptionRepository.GetByIdWithDetailsAsync(id);
            if (subscription == null)
            {
                _logger.LogWarning("Subscription ID {id} not found", id);
                return NotFound();
            }

            var model = _converterHelper.ToSubscriptionViewModel(subscription);
            await PopulateOptions(model);

            return View(model);
        }

        // POST: Subscription/Edit
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubscriptionViewModel model)
        {
            if (!User.IsInRole("Admin")) return Unauthorized();

            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                if (await ValidateExclusiveSubscription(model))
                {
                    ModelState.AddModelError("", "An exclusive subscription of this type already exists.");
                    await PopulateOptions(model);
                    return View(model);
                }

                // Use the existing image ID if a new one is not uploaded
                var imageId = model.SubscriptionImageFile != null
                    ? await _blobHelper.UploadBlobAsync(model.SubscriptionImageFile, "subscription-images")
                    : model.ImageId;

                var subscription = await _converterHelper.ToSubscriptionAsync(model, imageId, isNew: false);
                await _subscriptionRepository.UpdateAsync(subscription);

                TempData["SuccessMessage"] = "Subscription updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            await PopulateOptions(model);
            return View(model);
        }

        // GET: Subscription/Delete/5 (Admin Only)
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!User.IsInRole("Admin")) return Unauthorized();

            var subscription = await _subscriptionRepository.GetByIdWithDetailsAsync(id);
            if (subscription == null)
            {
                _logger.LogWarning("Subscription ID {id} not found", id);
                return NotFound();
            }

            return View(_converterHelper.ToSubscriptionViewModel(subscription));
        }

        // POST: Subscription/Delete (Admin Only)
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!User.IsInRole("Admin")) return Unauthorized();

            try
            {
                var subscription = await _subscriptionRepository.GetByIdAsync(id);
                if (subscription == null)
                {
                    _logger.LogWarning("Subscription ID {id} not found for deletion", id);
                    return NotFound();
                }

                await _subscriptionRepository.DeleteAsync(subscription);
                TempData["SuccessMessage"] = "Subscription deleted successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Subscription ID {id}", id);
                TempData["ErrorMessage"] = "Error deleting subscription. Please try again.";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateOptions(SubscriptionViewModel model)
        {
            var gyms = await _gymRepository.GetAllAsync();
            model.GymOptions = gyms.Select(g => new SelectListItem
            {
                Value = g.Id.ToString(),
                Text = $"{g.Name} ({g.Location})"
            }).ToList();

            var workouts = await _workoutRepository.GetAllAsync();
            model.WorkoutOptions = workouts.Select(w => new SelectListItem
            {
                Value = w.Id.ToString(),
                Text = w.Name
            }).ToList();
        }






        private async Task<List<SelectListItem>> GetSelectListItems<T>(Task<IEnumerable<T>> items, string dataValueField, string dataTextField)
        {
            var itemList = await items;
            return itemList.Select(item => new SelectListItem
            {
                Value = item.GetType().GetProperty(dataValueField).GetValue(item, null)?.ToString(),
                Text = item.GetType().GetProperty(dataTextField).GetValue(item, null)?.ToString()
            }).ToList();
        }

        private async Task<bool> ValidateExclusiveSubscription(SubscriptionViewModel model)
        {
            if (model.IsExclusive)
            {
                return await _subscriptionRepository.ExistsExclusiveSubscriptionAsync(model.SubscriptionType);
            }
            return false;
        }
    }
}
