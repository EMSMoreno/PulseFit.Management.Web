using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Data.Entities
{
    public class Payment : IEntity
    {
        public int Id { get; set; }
        [Range(0.01, 10000.00)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string UserId { get; set; } // Tipo alterado para string
        public User User { get; set; }
        public string PaymentMethod { get; set; } // Ex: Credit Card, PayPal, Bitcoin
        public string Status { get; set; } // Ex: Success, Failed
        public string TransactionId { get; set; }
    }
}
