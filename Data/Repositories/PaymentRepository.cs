using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Payment>> GetPaymentsByUserIdAsync(string userId)
        {
            return await _context.Payments
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<PaymentResult> ProcessPaymentAsync(Payment payment)
        {
            try
            {
                await _context.Payments.AddAsync(payment);
                await _context.SaveChangesAsync();

                return new PaymentResult
                {
                    IsSuccess = true,
                    Message = "Payment processed successfully.",
                    TransactionId = payment.TransactionId
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

        public async Task<Payment> GetByIdAsync(int paymentId)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.Id == paymentId);
        }

        public async Task<Payment> GetLatestPaymentForSubscriptionAsync(int subscriptionId)
        {
            // Gets the most recent payment for the given subscription
            return await _context.Payments
                .Where(p => p.SubscriptionId == subscriptionId)
                .OrderByDescending(p => p.PaymentDate)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsBySubscriptionIdAsync(int subscriptionId)
        {
            return await _context.Payments
                .Where(p => p.SubscriptionId == subscriptionId)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

    }
}
