using PulseFit.Management.Web.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using PulseFit.Management.Web.Models.Validations;

namespace PulseFit.Management.Web.Models
{
    public class ClientViewModel
    {
        public int Id { get; set; }
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
        [RegularExpression(@"^9[1236]\d{7}$", ErrorMessage = "Invalid Portuguese phone number.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Birthdate is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Birthdate")]
        [BirthdateValidation(minimumAge: 16, maximumAge: 100, ErrorMessage = "Birthdate must reflect an age between 16 and 100 years.")]
        public DateTime? Birthdate { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [MaxLength(100, ErrorMessage = "Address cannot exceed 100 characters.")]
        public string Address { get; set; }

        [Display(Name = "Registration Date")]
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Status is required.")]
        public Status Status { get; set; } = Status.Active;

        // Image Properties
        public Guid ImageId { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile? ProfilePictureFile { get; set; }

        public string ProfilePictureUrl => ImageId == Guid.Empty
            ? "/images/noimage.png"
            : $"/uploads/clients-pics/{ImageId}.jpg";
    }

}
