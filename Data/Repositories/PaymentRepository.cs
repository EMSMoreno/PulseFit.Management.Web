using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly DataContext _context;

        public PaymentRepository(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Payment> GetPaymentsByUserId(int userId)
        {
            return _context.Payments
            .Where(p => p.UserId == userId.ToString())
            .ToList();
        }

        public PaymentResult ProcessPayment(Payment payment)
        {
            if (payment.Amount <= 0)
            {
                return new PaymentResult
                {
                    IsSuccess = false,
                    Message = "Payment amount must be greater than zero."
                };
            }

            try
            {
                _context.Payments.Add(payment);
                _context.SaveChanges();

                return new PaymentResult
                {
                    IsSuccess = true,
                    Message = "Payment processed successfully."
                };
            }
            catch (Exception ex)
            {
                return new PaymentResult
                {
                    IsSuccess = false,
                    Message = $"An error occurred while processing the payment: {ex.Message}"
                };
            }
        }
    }
}