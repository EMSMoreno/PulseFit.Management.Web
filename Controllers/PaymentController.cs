using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using PulseFit.Management.Web.Data.Entities;
using PulseFit.Management.Web.Data.Repositories;
using PulseFit.Management.Web.Helpers;
using PulseFit.Management.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PulseFit.Management.Web.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMailHelper _mailHelper;
        private readonly IUserSubscriptionRepository _userSubscriptionRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IUserHelper _userHelper;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(
            IPaymentRepository paymentRepository,
            IMailHelper mailHelper,
            IUserSubscriptionRepository userSubscriptionRepository,
            ISubscriptionRepository subscriptionRepository,
            IClientRepository clientRepository,
            IUserHelper userHelper,
            ILogger<PaymentController> logger)
        {
            _paymentRepository = paymentRepository;
            _mailHelper = mailHelper;
            _userSubscriptionRepository = userSubscriptionRepository;
            _subscriptionRepository = subscriptionRepository;
            _clientRepository = clientRepository;
            _userHelper = userHelper;
            _logger = logger;
        }

        public async Task<IActionResult> SelectPaymentMethod(int subscriptionId, int? clientId = null)
        {
            _logger.LogInformation("Loading SelectPaymentMethod for Subscription ID: {SubscriptionId}", subscriptionId);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // If the administrator is assigning the subscription to a specific client, use the provided ClientId
            if (clientId.HasValue && User.IsInRole("Admin"))
            {
                TempData["ClientId"] = clientId.Value;
                userId = null; //To ensure we don't use the Admin ID
            }

            var existingSubscription = await _userSubscriptionRepository.GetActiveUserSubscriptionAsync(userId, subscriptionId);

            if (existingSubscription != null)
            {
                TempData["ErrorMessage"] = "You already have an active subscription of this type.";
                return RedirectToAction("ClientSubscriptions", "UserSubscription");
            }

            var subscription = await _subscriptionRepository.GetByIdAsync(subscriptionId);
            if (subscription == null)
            {
                _logger.LogWarning("Subscription not found for ID: {SubscriptionId}", subscriptionId);
                TempData["ErrorMessage"] = "Subscription not found.";
                return RedirectToAction("Index", "Subscription");
            }

            var effectivePrice = subscription.CalculatedPrice;
            var availableMethods = new List<string> { "Credit Card", "PayPal", "Crypto" };
            if (User.IsInRole("Admin")) availableMethods.Add("Cash");

            var viewModel = new PaymentViewModel
            {
                SubscriptionId = subscriptionId,
                SubscriptionName = subscription.Name,
                Amount = effectivePrice,
                PaymentMethodOptions = availableMethods.Select(m => new SelectListItem { Text = m, Value = m }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessPayment(PaymentViewModel model)
        {
            _logger.LogInformation("Starting ProcessPayment for Subscription ID: {SubscriptionId} with Amount: {Amount}", model.SubscriptionId, model.Amount);

            if (!User.Identity.IsAuthenticated)
            {
                _logger.LogWarning("User is not authenticated.");
                TempData["ErrorMessage"] = "User authentication required.";
                return RedirectToAction("Login", "Account");
            }

            // If there is a ClientId in TempData, use it to assign the subscription
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int? clientId = TempData["ClientId"] as int?;

            if (clientId.HasValue && User.IsInRole("Admin"))
            {
                var client = await _clientRepository.GetByIdAsync(clientId.Value);
                if (client == null)
                {
                    _logger.LogError("Client not found for Client ID: {ClientId}", clientId.Value);
                    ModelState.AddModelError("", "Client not found for this User.");
                    return View("SelectPaymentMethod", model);
                }
                userId = client.UserId; // Use Client ID
            }
            else
            {
                clientId = await _clientRepository.GetClientIdByUserIdAsync(userId);
            }

            if (clientId == null)
            {
                _logger.LogError("Client not found for User ID: {UserId}", userId);
                ModelState.AddModelError("", "Client not found for this User.");
                return View("SelectPaymentMethod", model);
            }

            var existingSubscription = await _userSubscriptionRepository.GetActiveUserSubscriptionAsync(userId, model.SubscriptionId);
            if (existingSubscription != null)
            {
                _logger.LogWarning("User already has an active subscription for Subscription ID: {SubscriptionId}", model.SubscriptionId);
                ModelState.AddModelError("", "You already have an active subscription of this type.");
                return View("SelectPaymentMethod", model);
            }

            var subscription = await _subscriptionRepository.GetByIdAsync(model.SubscriptionId);
            DateTime endDate = DateTime.UtcNow;

            switch (subscription.DurationType)
            {
                case DurationType.Days:
                    endDate = endDate.AddDays(subscription.DurationValue);
                    break;
                case DurationType.Weeks:
                    endDate = endDate.AddDays(subscription.DurationValue * 7);
                    break;
                case DurationType.Months:
                    endDate = endDate.AddMonths(subscription.DurationValue);
                    break;
                case DurationType.Years:
                    endDate = endDate.AddYears(subscription.DurationValue);
                    break;
            }

            var transactionId = Guid.NewGuid().ToString();
            var payment = new Payment
            {
                Amount = model.Amount,
                UserId = userId,
                SubscriptionId = model.SubscriptionId,
                Method = model.SelectedMethod,
                Status = model.Amount == 0 ? Payment.PaymentStatus.Success : Payment.PaymentStatus.Pending,
                Description = string.IsNullOrEmpty(model.Description) ? "No description provided" : model.Description,
                TransactionId = transactionId,
                PaymentDate = DateTime.UtcNow
            };

            var paymentResult = await _paymentRepository.ProcessPaymentAsync(payment);
            if (paymentResult.IsSuccess || payment.Amount == 0)
            {
                var newUserSubscription = new UserSubscription
                {
                    UserId = userId,
                    ClientId = clientId.Value,
                    SubscriptionId = model.SubscriptionId,
                    StartDate = DateTime.UtcNow,
                    EndDate = endDate,
                    Status = SubscriptionStatus.Active,
                    IsPaid = true,
                    TransactionId = transactionId
                };

                await _userSubscriptionRepository.CreateAsync(newUserSubscription);

                return RedirectToAction("Confirmation", new { paymentId = payment.Id });
            }
            else
            {
                _logger.LogError("Payment failed for User ID: {UserId} with Error Message: {Message}", userId, paymentResult.Message);
                ModelState.AddModelError("", paymentResult.Message);
            }

            return View("SelectPaymentMethod", model);
        }

        public async Task<IActionResult> Confirmation(int paymentId)
        {
            _logger.LogInformation("Loading Confirmation page for Payment ID: {PaymentId}", paymentId);

            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null)
            {
                _logger.LogWarning("Payment not found for Payment ID: {PaymentId}", paymentId);
                TempData["ErrorMessage"] = "Payment not found.";
                return RedirectToAction("Index", "Subscription");
            }

            var subscription = await _subscriptionRepository.GetByIdAsync(payment.SubscriptionId);
            var userSubscription = await _userSubscriptionRepository.GetActiveUserSubscriptionAsync(payment.UserId, payment.SubscriptionId);

            if (subscription == null || userSubscription == null)
            {
                _logger.LogWarning("Subscription record not found for Payment ID: {PaymentId}", paymentId);
                TempData["ErrorMessage"] = "Subscription record not found.";
                return RedirectToAction("Index", "Subscription");
            }

            var effectivePrice = subscription.CalculatedPrice;
            var confirmationViewModel = new ConfirmationViewModel
            {
                TransactionId = payment.TransactionId,
                PaymentDate = payment.PaymentDate,
                ExpirationDate = userSubscription.EndDate,
                Status = userSubscription.Status,
                SubscriptionName = subscription.Name,
                Amount = effectivePrice,
            };

            // Build email placeholders
            var placeholders = new Dictionary<string, string>
            {
                { "TransactionId", payment.TransactionId },
                { "SubscriptionName", subscription.Name },
                { "Amount", effectivePrice.ToString("C2") },
                { "ExpirationDate", userSubscription.EndDate.ToShortDateString() },
                { "PaymentDate", payment.PaymentDate.ToString("f") }
            };

            string emailTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "Views/Emails/PaymentConfirmationTemplate.html");
            string emailBody = _mailHelper.LoadAndProcessEmailTemplate(emailTemplatePath, placeholders);

            // Get user email using new method
            var userEmail = await _userHelper.GetUserEmailByPaymentIdAsync(paymentId);
            if (string.IsNullOrEmpty(userEmail))
            {
                _logger.LogError("No email found for Payment ID {PaymentId}", paymentId);
                ModelState.AddModelError("", "Could not retrieve user email for confirmation.");
                return View(confirmationViewModel);
            }

            // Send confirmation email
            var emailResponse = _mailHelper.SendEmail(userEmail, "Payment Confirmation", emailBody);
            if (!emailResponse.IsSuccess)
            {
                _logger.LogError("Error sending confirmation email for Payment ID {PaymentId}", paymentId);
                ModelState.AddModelError("", "An error occurred while sending the confirmation email.");
            }

            return View(confirmationViewModel);
        }
    }
}
