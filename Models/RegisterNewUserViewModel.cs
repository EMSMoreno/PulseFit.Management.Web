using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Models
{
    public class RegisterNewUserViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [MaxLength(100)]
        public string Address { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string Confirm { get; set; }

        // Novo campo para a imagem de perfil
        [Display(Name = "Profile Picture")]
        public IFormFile? ProfilePicture { get; set; }
    }
}
