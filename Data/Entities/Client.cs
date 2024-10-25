using System;
using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Data.Entities
{
    public class Client : IEntity
    {
        public int Id { get; set; }

        public DateTime Birthdate { get; set; }

        [MaxLength(100)]
        public string Address { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        public Subscription SubscriptionPlan { get; set; }

        public int SubscriptionPlanId { get; set; }

        public Status Status { get; set; } = Status.Active;

        // FK para User (removendo redundâncias com a entidade User)
        [Required]
        public string UserId { get; set; }

        public User User { get; set; }
    }
}
