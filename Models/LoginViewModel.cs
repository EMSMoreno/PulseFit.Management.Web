using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Models
{
    public class LoginViewModel
    {
        // Username is required
        [Required(ErrorMessage = "The Username is required.")]
        // Username must be a valid email address
        [EmailAddress(ErrorMessage = "The Username must be a valid email address.")]
        public string Username { get; set; }

        // Password is required
        [Required(ErrorMessage = "The Password is required.")]
        // Password must have a minimum length of 6 characters
        [MinLength(6, ErrorMessage = "The Password must be at least 6 characters long.")]
        public string Password { get; set; }

        // This property allows the user to stay logged in without re-entering credentials
        public bool RememberMe { get; set; }
    }
}
