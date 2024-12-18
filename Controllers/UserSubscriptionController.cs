﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using PulseFit.Management.Web.Data.Entities;
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
        private readonly IPaymentRepository _paymentRepository;
        private readonly IConverterHelper _converterHelper;
        private readonly ILogger<UserSubscriptionController> _logger;
        private readonly IConverter _pdfConverter;

        public UserSubscriptionController(
            IUserSubscriptionRepository userSubscriptionRepository,
            IClientRepository clientRepository,
            ISubscriptionRepository subscriptionRepository,
            IPaymentRepository paymentRepository,
            IConverterHelper converterHelper,
            ILogger<UserSubscriptionController> logger,
            IConverter pdfConverter)
        {
            _userSubscriptionRepository = userSubscriptionRepository;
            _clientRepository = clientRepository;
            _subscriptionRepository = subscriptionRepository;
            _paymentRepository = paymentRepository;
            _converterHelper = converterHelper;
            _logger = logger;
            _pdfConverter = pdfConverter;
        }

        #region Admin Methods

        // Displays client list to Admin with option to add/view subscriptions
        public async Task<IActionResult> Index()
        {
            if (!User.IsInRole("Admin")) return Unauthorized();

            var clients = await _clientRepository.GetAllClientsWithRoleAsync("Client");
            var userSubscriptions = await _userSubscriptionRepository.GetAllAsync();

            var viewModels = clients.Select(client =>
            {
                var subscription = userSubscriptions.FirstOrDefault(us => us.ClientId == client.Id);
                var clientName = client.User != null ? $"{client.User.FirstName} {client.User.LastName}" : "Unnamed Client";

                return new UserSubscriptionViewModel
                {
                    ClientId = client.Id,
                    Client = new ClientViewModel
                    {
                        FirstName = client.User?.FirstName,
                        LastName = client.User?.LastName
                    },
                    Subscription = subscription != null
                        ? new SubscriptionViewModel { Name = subscription.Subscription.Name }
                        : new SubscriptionViewModel { Name = "No Subscription" },
                    Status = subscription?.Status ?? SubscriptionStatus.Inactive
                };
            }).ToList();

            return View(viewModels);
        }

        // Displays all subscriptions for a specific client
        public async Task<IActionResult> Details(int clientId)
        {
            if (!User.IsInRole("Admin")) return Unauthorized();

            var userSubscriptions = await _userSubscriptionRepository.GetUserSubscriptionsAsync(clientId);
            if (userSubscriptions == null || !userSubscriptions.Any())
            {
                _logger.LogWarning("No subscriptions found for Client ID {clientId}", clientId);
                return NotFound();
            }

            // For each subscription, includes payments
            var viewModels = userSubscriptions.Select(us =>
            {
                var subscriptionVm = _converterHelper.ToUserSubscriptionViewModel(us);
                subscriptionVm.Payments = _paymentRepository
                    .GetPaymentsBySubscriptionIdAsync(us.SubscriptionId).Result
                    .Where(p => p.UserId == us.UserId)
                    .Select(p => new PaymentViewModel
                    {
                        Id = p.Id,
                        Amount = p.Amount,
                        PaymentDate = p.PaymentDate,
                        TransactionId = p.TransactionId
                    }).ToList();

                return subscriptionVm;
            }).ToList();

            return View(viewModels);
        }

        // Redirects the Admin to the customer subscription flow
        public async Task<IActionResult> SelectSubscriptionForClient(int clientId)
        {
            if (!User.IsInRole("Admin")) return Unauthorized();

            var client = await _clientRepository.GetByIdAsync(clientId);
            if (client == null)
            {
                _logger.LogWarning("Client ID {clientId} not found", clientId);
                return NotFound();
            }

            TempData["ClientId"] = clientId;
            return RedirectToAction("Index", "Subscription");
        }

        // Redirects to payment method selection
        public async Task<IActionResult> ProceedToPayment(int subscriptionId, int clientId)
        {
            if (!User.IsInRole("Admin")) return Unauthorized();

            var client = await _clientRepository.GetByIdAsync(clientId);
            if (client == null)
            {
                TempData["ErrorMessage"] = "Client not found.";
                return RedirectToAction("Index");
            }

            var subscription = await _subscriptionRepository.GetByIdAsync(subscriptionId);
            if (subscription == null)
            {
                TempData["ErrorMessage"] = "Subscription not found.";
                return RedirectToAction("SelectSubscriptionForClient", new { clientId });
            }

            // Redirects to the customer's payment method with the specified ClientId
            return RedirectToAction("SelectPaymentMethod", "Payment", new { subscriptionId, clientId });
        }

        #endregion

        #region Métodos de Cliente

        public async Task<IActionResult> ClientSubscriptions()
        {
            if (!User.Identity.IsAuthenticated) return Unauthorized();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var client = await _clientRepository.GetClientIdByUserIdAsync(userId);
            if (client == null) return Unauthorized();

            // Search for signed-in customer signatures and payments
            var userSubscriptions = await _userSubscriptionRepository.GetUserSubscriptionsAsync(client.Value);
            var viewModels = userSubscriptions.Select(us =>
            {
                // Filter payments by UserId
                var payments = _paymentRepository.GetPaymentsBySubscriptionIdAsync(us.SubscriptionId)
                                                  .Result
                                                  .Where(p => p.UserId == userId)
                                                  .ToList();

                return new ClientSubscriptionDetailsViewModel
                {
                    Subscription = _converterHelper.ToUserSubscriptionViewModel(us),
                    Payments = payments.Select(p => new PaymentViewModel
                    {
                        Id = p.Id,
                        Amount = p.Amount,
                        PaymentDate = p.PaymentDate,
                        TransactionId = p.TransactionId,
                        SubscriptionName = us.Subscription.Name
                    }).ToList()
                };
            }).ToList();

            return View(viewModels);
        }

        public async Task<IActionResult> GenerateInvoicePdf(int paymentId)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null) return NotFound("Payment not found.");

            var subscription = await _subscriptionRepository.GetByIdAsync(payment.SubscriptionId);
            if (subscription == null) return NotFound("Subscription not found.");

            var client = await _clientRepository.GetByUserIdAsync(payment.UserId);
            if (client == null) return NotFound("Client not found.");

            string htmlContent = $@"
<html>
    <body style='font-family: Arial, sans-serif; background-color: #121212; color: #ffffff; padding: 20px;'>
        <header style='text-align: center; margin-bottom: 20px;'>
<img src=""https://i.postimg.cc/26n1YzgM/pulsefitlogo.png"" alt=""PulseFit Logo"" style=""width: 150px; display: block; margin: 0 auto;"">
            <hr style='border: 1px solid #00ffff;'>
        </header>
        <h1 style='color: #00ffff; text-align: center; margin-bottom: 20px;'>Invoice</h1>
        <table style='width: 100%; border-collapse: collapse; margin: 20px 0;'>
            <tr>
                <th style='background-color: #222; color: #00ffff; border: 1px solid #444; padding: 8px; text-align: left;'>Client</th>
                <td style='border: 1px solid #444; padding: 8px;'>{client.User.FirstName} {client.User.LastName}</td>
            </tr>
            <tr>
                <th style='background-color: #222; color: #00ffff; border: 1px solid #444; padding: 8px; text-align: left;'>Subscription</th>
                <td style='border: 1px solid #444; padding: 8px;'>{subscription.Name}</td>
            </tr>
            <tr>
                <th style='background-color: #222; color: #00ffff; border: 1px solid #444; padding: 8px; text-align: left;'>Start Date</th>
                <td style='border: 1px solid #444; padding: 8px;'>{payment.PaymentDate.ToShortDateString()}</td>
            </tr>
            <tr>
                <th style='background-color: #222; color: #00ffff; border: 1px solid #444; padding: 8px; text-align: left;'>End Date</th>
                <td style='border: 1px solid #444; padding: 8px;'>{payment.PaymentDate.AddMonths(1).ToShortDateString()}</td>
            </tr>
            <tr>
                <th style='background-color: #222; color: #00ffff; border: 1px solid #444; padding: 8px; text-align: left;'>Amount Paid</th>
                <td style='border: 1px solid #444; padding: 8px;'>{payment.Amount} €</td>
            </tr>
            <tr>
                <th style='background-color: #222; color: #00ffff; border: 1px solid #444; padding: 8px; text-align: left;'>Transaction ID</th>
                <td style='border: 1px solid #444; padding: 8px;'>{payment.TransactionId}</td>
            </tr>
            <tr>
                <th style='background-color: #222; color: #00ffff; border: 1px solid #444; padding: 8px; text-align: left;'>Payment Date</th>
                <td style='border: 1px solid #444; padding: 8px;'>{payment.PaymentDate.ToShortDateString()}</td>
            </tr>
        </table>
        <footer style='text-align: center; font-size: 12px; margin-top: 20px;'>
            <p>PulseFit - All Rights Reserved © 2024</p>
        </footer>
    </body>
</html>";


            var pdf = _pdfConverter.Convert(new HtmlToPdfDocument
            {
                GlobalSettings = new GlobalSettings { PaperSize = PaperKind.A4, Orientation = Orientation.Portrait },
                Objects = { new ObjectSettings { HtmlContent = htmlContent, WebSettings = { DefaultEncoding = "utf-8" } } }
            });

            return File(pdf, "application/pdf", $"Invoice_{payment.TransactionId}.pdf");
        }

        #endregion

        #region Auxiliary Methods

        private async Task<List<SelectListItem>> LoadSubscriptionOptionsAsync()
        {
            var subscriptions = await _subscriptionRepository.GetAllActiveSubscriptionsAsync();
            return subscriptions.Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() }).ToList();
        }

        private async Task<bool> HasActiveSubscription(int clientId, int subscriptionId)
        {
            var existingSubscriptions = await _userSubscriptionRepository.GetUserSubscriptionsAsync(clientId);
            return existingSubscriptions.Any(us => us.SubscriptionId == subscriptionId && us.Status == SubscriptionStatus.Active);
        }

        private DateTime CalculateEndDate(DateTime startDate, DurationType durationType, int durationValue)
        {
            return durationType switch
            {
                DurationType.Days => startDate.AddDays(durationValue),
                DurationType.Weeks => startDate.AddDays(durationValue * 7),
                DurationType.Months => startDate.AddMonths(durationValue),
                DurationType.Years => startDate.AddYears(durationValue),
                _ => startDate
            };
        }

        #endregion
    }
}
