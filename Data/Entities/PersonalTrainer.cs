using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Data.Entities
{
    public class PersonalTrainer : IEntity
    {
        public int Id { get; set; }

        public List<Specialty> Specialties { get; set; } = new List<Specialty>(); // Inicializando lista

        public CertificationType? Certification { get; set; } // Usando enum para certificações

        public List<Client> Clients { get; set; } = new List<Client>(); // Inicializando lista

        public DateTime? HireDate { get; set; }

        public Status Status { get; set; } = Status.Active;

        // FK para User
        [Required]
        public string UserId { get; set; }

        public User User { get; set; }
    }

    // Enum para as certificações do Personal Trainer
    public enum CertificationType
    {
        None,
        CEFAD_CPT,             // Professional Training Course in Personal Training - CEFAD
        Gnosies_PT,            // Personal Trainer - Gnosies
        ESDRM,                 // Degree in Sports and Wellness - ESDRM
        IPDJ_Coach,           // Sports Coach - IPDJ
        IPDJ_Trainer,         // Sports Trainer - IPDJ
        Sports_Degree,        // Degree in Sports
        Sports_Masters,       // Master's in Sports
        NSCA_CPT,             // Certified Personal Trainer by NSCA
        ACE_CPT,              // Certified Personal Trainer by ACE
        RYT_200,              // Registered Yoga Teacher (200 hours)
        FMS,                  // Functional Movement Screening
        CPR_AED,              // CPR and AED Certification
        FirstAid              // First Aid
    }
}
