using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    public interface IPaymentRepository
    {
        PaymentResult ProcessPayment(Payment payment);

        IEnumerable<Payment> GetPaymentsByUserId(int userId);
    }
}