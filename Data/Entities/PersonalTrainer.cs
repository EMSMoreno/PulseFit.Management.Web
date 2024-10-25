using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Data.Entities
{
    public class PersonalTrainer : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Specialty { get; set; }

        public string? Certification { get; set; } // Permitindo null caso não tenha certificação

        public List<Client> Clients { get; set; } = new List<Client>(); // Inicializando lista

        public DateTime HireDate { get; set; }

        public Status Status { get; set; } = Status.Active;

        // FK para User
        [Required]
        public string UserId { get; set; }

        public User User { get; set; }
    }
}
