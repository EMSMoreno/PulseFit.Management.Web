using PulseFit.Management.Web.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace PulseFit.Management.Web.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Employee type is required.")]
        [Display(Name = "Employee Type")]
        public EmployeeType EmployeeType { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Hire Date")]
        public DateTime? HireDate { get; set; } // Optional hire date

        [Required(ErrorMessage = "Status is required.")]
        public Status Status { get; set; } = Status.Active;

        [Required(ErrorMessage = "Shift type is required.")]
        [Display(Name = "Shift Type")]
        public ShiftType Shift { get; set; }

        // User-related properties
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
        public Guid ImageId { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile? ProfilePictureFile { get; set; }

        public string ProfilePictureUrl => ImageId == Guid.Empty
            ? "/images/noimage.png"
            : $"/uploads/employees-pics/{ImageId}.jpg";
    }
}
