namespace PulseFit.Management.Web.Data.Entities
{
    public class Payment : IEntity
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public string PaymentMethod { get; set; } // Ex: Credit Card, PayPal, Bitcoin

        public string Status { get; set; } // Ex: Success, Failed

        public string TransactionId { get; set; }
    }
}
