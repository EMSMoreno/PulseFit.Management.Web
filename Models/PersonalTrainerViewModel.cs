using Microsoft.AspNetCore.Http;
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
        public string Specialty { get; set; }

        public string? Certification { get; set; }

        public List<Client> Clients { get; set; } = new List<Client>();

        [Required]
        public DateTime HireDate { get; set; }

        public Status Status { get; set; }

        // Propriedades do User
        public string UserId { get; set; }

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

        public string ProfilePictureUrl => ImageId == Guid.Empty ? "/images/default-profile.png" : $"https://myblobstorage.blob.core.windows.net/profile-pics/{ImageId}";
    }
}
