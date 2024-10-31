using Microsoft.AspNetCore.Mvc;
using PulseFit.Management.Web.Data.Entities;
using PulseFit.Management.Web.Data.Repositories;

namespace PulseFit.Management.Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentController(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public IActionResult ProcessPayment(Payment payment)
        {
            var result = _paymentRepository.ProcessPayment(payment);

            if (result.IsSuccess)
            {
                TempData["PaymentMessage"] = "Payment made successfully!";
                TempData["MessageType"] = "success";
            }
            else
            {
                TempData["PaymentMessage"] = $"Failed to process payment: {result.Message}";
                TempData["MessageType"] = "error";
            }

            return View();
        }

        public IActionResult GetPaymentsByUserId(int userId)
        {
            var payments = _paymentRepository.GetPaymentsByUserId(userId);
            return View(payments);
        }
    }
}
