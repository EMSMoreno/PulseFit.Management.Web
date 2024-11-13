namespace PulseFit.Management.Web.Models
{
    public class ClientSubscriptionDetailsViewModel
    {
        public UserSubscriptionViewModel Subscription { get; set; }
        public List<PaymentViewModel> Payments { get; set; }
    }
}
