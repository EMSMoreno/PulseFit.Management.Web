using PulseFit.Management.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Data.Entities
{
    public class Client : IEntity
    {
        public int Id { get; set; }

        [Required]
        public DateTime? Birthdate { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        public Status Status { get; set; } = Status.Active;

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        public ICollection<UserSubscription> UserSubscriptions { get; set; } = new List<UserSubscription>();


    }

    public enum Gender
    {
        Male,
        Female,
        Other
    }
}
