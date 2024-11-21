using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PulseFit.Management.Web.Data.Entities
{
    public class UserSubscription : IEntity
    {
        public int Id { get; set; }

        // Relationship with User
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }

        // Relationship with Subscription
        [Required]
        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }

        public bool IsPaid { get; set; }

        // Relationship with Client
        [Required]
        public int ClientId { get; set; }
        public Client Client { get; set; }

        // Subscription start date
        public DateTime StartDate { get; set; }

        // Payment Properties
        public string TransactionId { get; set; }
        public decimal AmountPaid { get; set; }

        // End date dynamically calculated based on Subscription
        public DateTime EndDate { get; set; }

        public SubscriptionStatus Status { get; set; }

        // Calculates the end date based on the Subscription duration
        public void CalculateEndDate()
        {
            DateTime endDate = StartDate;

            switch (Subscription.DurationType)
            {
                case DurationType.Days:
                    endDate = endDate.AddDays(Subscription.DurationValue);
                    break;
                case DurationType.Weeks:
                    endDate = endDate.AddDays(Subscription.DurationValue * 7);
                    break;
                case DurationType.Months:
                    endDate = endDate.AddMonths(Subscription.DurationValue);
                    break;
                case DurationType.Years:
                    endDate = endDate.AddYears(Subscription.DurationValue);
                    break;
            }

            EndDate = endDate;
        }
    }
}
