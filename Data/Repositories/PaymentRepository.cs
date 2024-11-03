using PulseFit.Management.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PulseFit.Management.Web.Data.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly DataContext _context;

        public PaymentRepository(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Payment> GetPaymentsByUserId(string userId)
        {
            return _context.Payments.Where(p => p.UserId == userId).ToList();
        }

        public void ProcessPayment(Payment payment)
        {
            _context.Payments.Add(payment);
            _context.SaveChanges();
        }
    }
}
