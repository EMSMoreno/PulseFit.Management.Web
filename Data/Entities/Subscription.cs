using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PulseFit.Management.Web.Data.Entities
{
    public class Subscription : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, double.MaxValue, ErrorMessage = "The field {0} must be greater than or equal to {1}.")]
        public decimal Price { get; set; }

        public int MaxWorkouts { get; set; }

        // Properties to set custom subscription duration
        [Required]
        public int DurationValue { get; set; } = 1; // Ex: 1, 7, 30, etc.

        [Required]
        public DurationType DurationType { get; set; } = DurationType.Months; // Days, Weeks, Months or Years

        public SubscriptionType SubscriptionType { get; set; }

        // Exclusivity, limiting the user to a single active subscription of that type
        public bool IsExclusive { get; set; } = false;

        public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;

        public ICollection<UserSubscription> UserSubscriptions { get; set; } = new List<UserSubscription>();

        // Specific benefits
        public bool IsAllGymsAccessible { get; set; } = false;
        public List<Gym> IncludedGyms { get; set; } = new List<Gym>();
        public List<Workout> IncludedWorkouts { get; set; } = new List<Workout>();
        public int MaxPersonalTrainerSessions { get; set; } = 0;
        public bool IncludeNutritionPlans { get; set; } = false;
        public bool IncludeOnlineClasses { get; set; } = false;

        public bool Has24HourAccess { get; set; } = false;
        public bool HasVIPAccess { get; set; } = false;
        public int PerformanceReportFrequencyInMonths { get; set; } = 0;
        
        [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100.")]
        public int DiscountPercentage { get; set; } = 0;

        [NotMapped]
        public decimal CalculatedPrice => Price * (1 - DiscountPercentage / 100m);

        // Image Properties
        public Guid ImageId { get; set; } = Guid.Empty;

        [NotMapped]
        public string ImageUrl => ImageId == Guid.Empty
            ? "/images/noimage.png"
            : $"/uploads/subscription-images/{ImageId}.jpg";
    }

    public enum SubscriptionStatus
    {
        Active,
        Inactive,
        Expired,
        Pending
    }

    public enum SubscriptionType
    {
        General,         // Grants access to all gyms and services.
        Monthly,         // Fixed duration of one month.
        Annual,          // Fixed duration of one year.
        Individual,      // Specific to a single gym.
        OnlineClasses,   // Online classes only.
        Regional,        // Group of selected gyms.
        Corporate,       // For businesses, multiple users under one plan.
        Trial            // Temporary trial period.
    }

    public enum DurationType
    {
        Days,
        Weeks,
        Months,
        Years
    }
}
