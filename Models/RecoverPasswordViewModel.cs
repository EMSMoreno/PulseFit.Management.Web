using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Models
{
    public class RecoverPasswordViewModel
    {
        // The [Required] attribute indicates that this field is mandatory and must be filled.
        // If the field is empty, an error message will be displayed.
        [Required(ErrorMessage = "Email is required.")]
        // The [EmailAddress] attribute validates that the entered value is in a valid email address format.
        // Ensures the email format is correct (e.g., user@example.com).
        [EmailAddress(ErrorMessage = "The email format is invalid.")]
        // Property to store the user's email address.
        public string Email { get; set; }
    }
}
