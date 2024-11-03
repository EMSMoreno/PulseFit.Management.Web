using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PulseFit.Management.Web.Data.Entities;
using PulseFit.Management.Web.Data.Repositories;
using PulseFit.Management.Web.ViewModels;

namespace PulseFit.Management.Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly UserManager<User> _userManager;

        public PaymentController(IPaymentRepository paymentRepository, UserManager<User> userManager, ISubscriptionRepository subscriptionRepository)
        {
            _paymentRepository = paymentRepository;
            _subscriptionRepository = subscriptionRepository;
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult ProcessPayment(Payment payment)
        {
            try
            {
                // Processar pagamento via API externa se necessário.
                payment.Status = Payment.PaymentStatus.Pending;
                _paymentRepository.ProcessPayment(payment);

                return Json(new { success = true, message = "Pagamento realizado com sucesso." });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Falha ao processar pagamento. Tente novamente." });
            }
        }

        [HttpGet]
        public IActionResult History(string userId)
        {
            var payments = _paymentRepository.GetPaymentsByUserId(userId);
            return View("History", payments);
        }

        [HttpGet]
        public IActionResult Subscription()
        {
            // Obter a lista de assinaturas disponíveis
            var availableSubscriptions = _subscriptionRepository.GetAllActiveSubscriptions().ToList();

            // Criar o modelo de view
            var model = new PaymentViewModel
            {
                AvailableSubscriptions = availableSubscriptions
            };

            return View(model);
        }
    }
}
