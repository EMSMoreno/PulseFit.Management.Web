using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using PulseFit.Management.Web.Data.Repositories;
using PulseFit.Management.Web.Helpers;
using PulseFit.Management.Web.Models;

namespace PulseFit.Management.Web.Controllers
{
    public class UserSubscriptionController : Controller
    {
        private readonly IUserSubscriptionRepository _userSubscriptionRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IConverterHelper _converterHelper;
        private readonly ILogger<UserSubscriptionController> _logger;

        public UserSubscriptionController(
            IUserSubscriptionRepository userSubscriptionRepository,
            IClientRepository clientRepository,
            ISubscriptionRepository subscriptionRepository,
            IConverterHelper converterHelper,
            ILogger<UserSubscriptionController> logger)
        {
            _userSubscriptionRepository = userSubscriptionRepository;
            _clientRepository = clientRepository;
            _subscriptionRepository = subscriptionRepository;
            _converterHelper = converterHelper;
            _logger = logger;
        }

        // GET: UserSubscription/Details/5
        public async Task<IActionResult> Details(int clientId)
        {
            var userSubscriptions = await _userSubscriptionRepository.GetUserSubscriptionsAsync(clientId);
            if (userSubscriptions == null || !userSubscriptions.Any())
            {
                _logger.LogWarning("No subscriptions found for Client ID {clientId}", clientId);
                return NotFound();
            }

            var viewModels = userSubscriptions.Select(us => _converterHelper.ToUserSubscriptionViewModel(us));
            return View(viewModels);
        }

        // GET: UserSubscription/Create
        public async Task<IActionResult> Create(int clientId)
        {
            var client = await _clientRepository.GetByIdAsync(clientId);
            if (client == null)
            {
                _logger.LogWarning("Client ID {clientId} not found", clientId);
                return NotFound();
            }

            var model = new UserSubscriptionViewModel
            {
                ClientId = clientId,
                SubscriptionOptions = (await _subscriptionRepository.GetAllActiveSubscriptionsAsync())
                    .Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() }).ToList()
            };
            return View(model);
        }

        // POST: UserSubscription/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserSubscriptionViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userSubscription = await _converterHelper.ToUserSubscriptionAsync(model, isNew: true);
                    await _userSubscriptionRepository.CreateAsync(userSubscription);
                    TempData["SuccessMessage"] = "Subscription added successfully.";
                    return RedirectToAction(nameof(Details), new { clientId = model.ClientId });
                }
                catch (System.Exception ex)
                {
                    _logger.LogError(ex, "Error creating subscription for Client ID {clientId}", model.ClientId);
                    ModelState.AddModelError("", "An error occurred while creating the subscription.");
                }
            }

            // Recarregar as opções de assinatura caso haja um erro de validação
            model.SubscriptionOptions = (await _subscriptionRepository.GetAllActiveSubscriptionsAsync())
                .Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() }).ToList();
            return View(model);
        }

        // GET: UserSubscription/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var userSubscription = await _userSubscriptionRepository.GetByIdAsync(id);
            if (userSubscription == null)
            {
                _logger.LogWarning("Subscription ID {id} not found", id);
                return NotFound();
            }

            var model = _converterHelper.ToUserSubscriptionViewModel(userSubscription);
            return View(model);
        }

        // POST: UserSubscription/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userSubscription = await _userSubscriptionRepository.GetByIdAsync(id);
            if (userSubscription == null)
            {
                _logger.LogWarning("Subscription ID {id} not found for deletion", id);
                return NotFound();
            }

            try
            {
                await _userSubscriptionRepository.DeleteAsync(userSubscription);
                TempData["SuccessMessage"] = "Subscription deleted successfully.";
                return RedirectToAction(nameof(Details), new { clientId = userSubscription.ClientId });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error deleting subscription ID {id}", id);
                ModelState.AddModelError("", "An error occurred while deleting the subscription.");
                return View(_converterHelper.ToUserSubscriptionViewModel(userSubscription));
            }
        }
    }
}
