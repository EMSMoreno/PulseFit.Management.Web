using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Models
{
    public class ChangeUserViewModel
    {
        // FirstName is required
        [Required]
        // The Display attribute ensures the label appears as "First Name"
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        // LastName is required
        [Required]
        // The Display attribute ensures the label appears as "Last Name"
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        // Field for the user's address with a maximum length of 100 characters.
        // The [MaxLength] attribute defines the error message if the limit is exceeded.
        [MaxLength(100, ErrorMessage = "The field {0} can only contain {1} characters.")]
        public string Address { get; set; }

        // Field for the user's phone number with a maximum length of 20 characters.
        // The [MaxLength] attribute defines the error message if the limit is exceeded.
        [MaxLength(20, ErrorMessage = "The field {0} can only contain {1} characters.")]
        public string PhoneNumber { get; set; }
    }
}
