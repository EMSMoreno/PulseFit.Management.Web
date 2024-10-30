namespace PulseFit.Management.Web.Data.Entities
{
    public class UserSubscription : IEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; } 

        public User User { get; set; }

        public int SubscriptionId { get; set; }

        public Subscription Subscription { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public UserSubscriptionStatus Status { get; set; }

        public enum UserSubscriptionStatus
        {
            Active,
            Expired
        }
    }
}
