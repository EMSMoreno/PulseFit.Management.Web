using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using PulseFit.Management.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Models
{
    public class PersonalTrainerViewModel
    {
        public int Id { get; set; }

        [Required]
        public List<int> SpecialtyIds { get; set; } = new List<int>(); // IDs das especialidades selecionadas

        public List<SpecialtyItemViewModel> Specialties { get; set; } = new List<SpecialtyItemViewModel>();

        public CertificationType? Certification { get; set; }

        public List<Client> Clients { get; set; } = new List<Client>();

        [Required]
        public DateTime? HireDate { get; set; }

        public Status Status { get; set; }

        // Propriedades do User
        public string? UserId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile? ProfilePictureFile { get; set; }

        public Guid ImageId { get; set; }

        public string ProfilePictureUrl => ImageId == Guid.Empty
            ? "/images/noimage.png"
            : $"/uploads/personaltrainers-pics/{ImageId}.jpg";
    }
}
