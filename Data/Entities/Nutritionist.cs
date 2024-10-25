using System;
using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Data.Entities
{
    public class Nutritionist : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Specialization { get; set; }

        public int ExperienceYears { get; set; }

        public Status Status { get; set; } = Status.Active;

        // FK para User
        [Required]
        public string UserId { get; set; }

        public User User { get; set; }
    }
}
