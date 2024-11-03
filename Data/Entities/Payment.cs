using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PulseFit.Management.Web.Data.Entities
{
    public class Payment : IEntity
    {
        public int Id { get; set; }

        [Range(0.01, 10000.00, ErrorMessage = "Value must be between {1} and {2}.")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        public string UserId { get; set; }

        public User User { get; set; }

        public PaymentMethod PaymentMethodSelected { get; set; }

        public PaymentStatus Status { get; set; }

        public string TransactionId { get; set; }

        // Novo campo para associar o pagamento a uma assinatura específica
        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }

        public enum PaymentMethod
        {
            CreditCard,
            PayPal,
            Crypto
        }

        public enum PaymentStatus
        {
            Success,
            Failed,
            Pending
        }
    }
}