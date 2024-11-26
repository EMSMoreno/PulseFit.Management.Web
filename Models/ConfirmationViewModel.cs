using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Models
{
    public class ConfirmationViewModel
    {
        public string TransactionId { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public SubscriptionStatus Status { get; set; }
        public string SubscriptionName { get; set; } 
        public decimal Amount { get; set; } 
    }
}
