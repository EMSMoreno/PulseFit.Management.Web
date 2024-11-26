using Microsoft.AspNetCore.Mvc.Rendering;
using PulseFit.Management.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Models
{
    public class UserSubscriptionViewModel
    {
        public int Id { get; set; }

        // ID and details of the subscription
        [Required]
        public int SubscriptionId { get; set; }
        public SubscriptionViewModel Subscription { get; set; }

        // ID and details of the client
        [Required]
        public int ClientId { get; set; }
        public ClientViewModel Client { get; set; } // Can be added to represent the client

        // Start and end dates
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Subscription and payment status
        public SubscriptionStatus Status { get; set; }
        public bool IsPaid { get; set; }

        // Payment properties
        public string TransactionId { get; set; }
        public decimal AmountPaid { get; set; }

        // Checks if the subscription is active
        public bool IsActive => Status == SubscriptionStatus.Active && EndDate >= DateTime.UtcNow;

        // Helper properties for display
        public string SubscriptionName => Subscription?.Name;
        public decimal SubscriptionPrice => Subscription?.Price ?? 0; // Accesses subscription price

        // List of subscription options for dropdowns or selection
        public IEnumerable<SelectListItem> SubscriptionOptions { get; set; }

        // Helper method to calculate the end date based on the subscription duration
        public void CalculateEndDate()
        {
            DateTime calculatedEndDate = StartDate;

            if (Subscription != null)
            {
                switch (Subscription.DurationType)
                {
                    case DurationType.Days:
                        calculatedEndDate = calculatedEndDate.AddDays(Subscription.DurationValue);
                        break;
                    case DurationType.Weeks:
                        calculatedEndDate = calculatedEndDate.AddDays(Subscription.DurationValue * 7);
                        break;
                    case DurationType.Months:
                        calculatedEndDate = calculatedEndDate.AddMonths(Subscription.DurationValue);
                        break;
                    case DurationType.Years:
                        calculatedEndDate = calculatedEndDate.AddYears(Subscription.DurationValue);
                        break;
                }
            }

            EndDate = calculatedEndDate;
        }

        // List of payments associated with the subscription
        public List<PaymentViewModel> Payments { get; set; } = new List<PaymentViewModel>();
    }
}
