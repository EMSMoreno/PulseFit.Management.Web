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

        [Required(ErrorMessage = "At least one specialty must be selected.")]
        [Display(Name = "Specialties")]
        public List<int> SpecialtyIds { get; set; } = new List<int>(); // IDs of selected specialties

        public List<SpecialtyItemViewModel> Specialties { get; set; } = new List<SpecialtyItemViewModel>();

        [Display(Name = "Certification Type")]
        public CertificationType? Certification { get; set; } // Optional certification type

        [Display(Name = "Clients")]
        public List<Client> Clients { get; set; } = new List<Client>();

        [Required(ErrorMessage = "Hire date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Hire Date")]
        public DateTime? HireDate { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public Status Status { get; set; } = Status.Active;

        // User properties
        public string? UserId { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        // Profile picture properties
        [Display(Name = "Profile Picture")]
        public IFormFile? ProfilePictureFile { get; set; }

        public Guid ImageId { get; set; }

        public string ProfilePictureUrl => ImageId == Guid.Empty
            ? "/images/noimage.png"
            : $"/uploads/personaltrainers-pics/{ImageId}.jpg";
    }
}
