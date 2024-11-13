using System.Collections.Generic;
using System.Threading.Tasks;
using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    public interface IPaymentRepository
    {
        Task<PaymentResult> ProcessPaymentAsync(Payment payment);
        Task<IEnumerable<Payment>> GetPaymentsByUserIdAsync(string userId);
        Task<Payment> GetByIdAsync(int paymentId);

        Task<Payment> GetLatestPaymentForSubscriptionAsync(int subscriptionId);  // Adicionado

        Task<IEnumerable<Payment>> GetPaymentsBySubscriptionIdAsync(int subscriptionId);


    }
}
