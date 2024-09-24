using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public DateTime BirthDate { get; set; }

        [Required]
        public string UserType { get; set; } // Admin, Cliente, Personal Trainer, etc.

        public string Address { get; set; }

        public string Phone { get; set; }
    }
}
