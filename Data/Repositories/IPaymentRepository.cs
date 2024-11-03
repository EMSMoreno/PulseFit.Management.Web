using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    public interface IPaymentRepository
    {
        void ProcessPayment(Payment payment);

        IEnumerable<Payment> GetPaymentsByUserId(string userId);
    }
}