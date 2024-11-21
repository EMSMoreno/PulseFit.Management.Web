using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Data.Entities
{
    public class Nutritionist : IEntity
    {
        public int Id { get; set; }

        // Relationship with Specializations, similar to how Personal Trainers have Specialties
        public List<Specialization> Specializations { get; set; } = new List<Specialization>();

        public int ExperienceYears { get; set; }

        public Status Status { get; set; } = Status.Active;

        // FK to User
        [Required]
        public string UserId { get; set; }

        public User User { get; set; }
    }
}
