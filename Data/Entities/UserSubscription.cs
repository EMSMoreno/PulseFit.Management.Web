namespace PulseFit.Management.Web.Data.Entities
{
    public class UserSubscription : IEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Tipo alterado para string
        public User User { get; set; }
        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
    }
}
