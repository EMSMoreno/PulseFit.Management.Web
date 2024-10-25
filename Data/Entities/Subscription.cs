using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PulseFit.Management.Web.Data.Entities
{
    public class Subscription : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; } // Opcional

        
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, double.MaxValue, ErrorMessage = "The field {0} must be greater than or equal to {1}.")]
        public decimal Price { get; set; }

        public int MaxWorkouts { get; set; }

        public int DurationMonths { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;

        public enum SubscriptionStatus
        {
            Active,
            Inactive,
            Expired
        }
    }
}
