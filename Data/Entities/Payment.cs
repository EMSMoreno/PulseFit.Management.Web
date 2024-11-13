using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PulseFit.Management.Web.Data.Entities
{
    public class Payment : IEntity
    {
        public int Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string UserId { get; set; }
        public User User { get; set; }

        [Required]
        public PaymentMethod Method { get; set; }

        public PaymentStatus Status { get; set; }
        public string TransactionId { get; set; }
        public string Description { get; set; }

        [ForeignKey("Subscription")]
        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }

        public enum PaymentMethod
        {
            CreditCard,
            PayPal,
            Crypto,
            Cash // Disponível apenas para administradores e funcionários
        }

        public enum PaymentStatus
        {
            Success,
            Failed2,
            Pending
        }
    }
}
