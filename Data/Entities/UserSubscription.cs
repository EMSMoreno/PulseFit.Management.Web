using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PulseFit.Management.Web.Data.Entities
{
    public class UserSubscription : IEntity
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public int SubscriptionId { get; set; }

        [ForeignKey("SubscriptionId")]
        public Subscription Subscription { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public UserSubscriptionStatus Status { get; set; } = UserSubscriptionStatus.Active;

        public enum UserSubscriptionStatus
        {
            Active,
            Expired
        }
    }
}
