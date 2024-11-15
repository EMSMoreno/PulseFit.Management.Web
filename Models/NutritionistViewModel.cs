using Microsoft.AspNetCore.Http;
using PulseFit.Management.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Models
{
    public class NutritionistViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "At least one specialization must be selected.")]
        [Display(Name = "Specializations")]
        public List<int> SpecializationIds { get; set; } = new List<int>(); // IDs of selected specializations

        public List<SpecialtyItemViewModel> Specializations { get; set; } = new List<SpecialtyItemViewModel>();

        [Required(ErrorMessage = "Experience years are required.")]
        [Range(1, 50, ErrorMessage = "Experience should be between 1 and 50 years.")]
        [Display(Name = "Years of Experience")]
        public int ExperienceYears { get; set; }

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
        [RegularExpression(@"^9[1236]\d{7}$", ErrorMessage = "Invalid Portuguese phone number.")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        // Profile picture properties
        [Display(Name = "Profile Picture")]
        public IFormFile? ProfilePictureFile { get; set; }

        public Guid ImageId { get; set; }

        public string ProfilePictureUrl => ImageId == Guid.Empty
            ? "/images/noimage.png"
            : $"/uploads/nutritionists-pics/{ImageId}.jpg";
    }
}
