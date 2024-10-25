using Microsoft.AspNetCore.Http;
using PulseFit.Management.Web.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Employee Type")]
        public EmployeeType EmployeeType { get; set; } // Usa o enum no ViewModel

        [Required]
        public DateTime HireDate { get; set; }

        public Status Status { get; set; }

        public string Shift { get; set; }

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

        public Guid ImageId { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile ImageFile { get; set; }

        public string ProfilePictureUrl => ImageId == Guid.Empty
            ? "/images/default-profile.png"
            : $"https://myblobstorage.blob.core.windows.net/employees-pics/{ImageId}";
    }
}
