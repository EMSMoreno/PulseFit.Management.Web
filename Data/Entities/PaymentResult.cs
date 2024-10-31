namespace PulseFit.Management.Web.Data.Entities
{
    public class PaymentResult
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public string TransactionId { get; set; }
    }
}
