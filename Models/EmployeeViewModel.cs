using PulseFit.Management.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Employee Type")]
        public EmployeeType EmployeeType { get; set; }

        public DateTime? HireDate { get; set; } // Data de contratação opcional

        [Required]
        public Status Status { get; set; }

        [Required]
        [Display(Name = "Shift Type")]
        public ShiftType Shift { get; set; }

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
        public IFormFile? ProfilePictureFile { get; set; }

        public string ProfilePictureUrl => ImageId == Guid.Empty
            ? "/images/noimage.png"
            : $"/uploads/employees-pics/{ImageId}.jpg";
    }
}
